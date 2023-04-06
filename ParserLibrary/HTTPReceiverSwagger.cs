﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.VisualStudio.Threading;
using System.Text.Json.Serialization;
using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NSwag.CodeGeneration.CSharp;
using Serilog.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Security.Cryptography.X509Certificates;
using PluginBase;

namespace ParserLibrary
{
    public partial class HTTPReceiverSwagger : IReceiver
    {
        /// <summary>
        /// Address to listen on. If not set, the server listens on localhost.
        /// </summary>
        public string address = "localhost";
        
        /// <summary>
        /// Port to listen on. If not set, the server listens on port 8080.
        /// </summary>
        public int port = 8080;

        public string swaggerSpecPath = null;
        
        /// <summary>
        /// Path to save the generated server code to. If not set, the server code is not saved.
        /// </summary>
        public string serverCodePath = null;

        /// <summary>
        /// Server certificate subject name.
        /// When given, enables HTTPS in Kestrel and is used to locate the certificate in the local machine certificate store.
        /// When omitted, Kestrel is not configured to use HTTPS.
        /// </summary>
        public string certSubject;
        
        /// <summary>
        /// File containing the mock response to return.
        /// If set, the receiver will return the contents of this file instead of waiting for requests.
        /// </summary>
        public string mockFile;

        /// <summary>
        /// Body of the mock response to return.
        /// If set, the receiver will return this body instead of waiting for requests.
        /// </summary>
        public string mockBody;

        IHostBuilder _hostBuilder;
        
        public string ResponseType = "application/json";
        private IReceiverHost _host;
        public bool cantTryParse; // This one comes from the YAML definition of the receiver
        private bool _debugMode;

        private X509Certificate2 FindMatchingCertificateBySubject(string subjectCommonName)
        {
            // This is a bit reworked copy of the example from
            // https://learn.microsoft.com/en-us/azure/service-fabric/service-fabric-tutorial-dotnet-app-enable-https-endpoint
            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

            // TODO: clarify the order in which the certificates are enumerated;
            // if it is not deterministic, consider sorting the certificates by expiration date.
            foreach (var cert in store.Certificates)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(subjectCommonName, cert.GetNameInfo(X509NameType.SimpleName, forIssuer: false))
                    && DateTime.Now < cert.NotAfter
                    && DateTime.Now >= cert.NotBefore)
                {
                    return cert;
                }
            }

            throw new Exception($"Could not find a match for a certificate with subject 'CN={subjectCommonName}'.");
        }
        
        public HTTPReceiverSwagger()
        {
            Logger.log("HTTPReceiverSwagger: Creating a host to listen on the port " + port);
            
            // Create a new host listening on the given port
            _hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureServices(services =>
                        {
                            services.AddSwaggerGen(c =>
                            {
                                c.SwaggerDoc("v1",
                                    new OpenApiInfo
                                    {
                                        Title = $"HTTPReceiverSwagger API based on {this.swaggerSpecPath}",
                                        Version = "v1"
                                    });
                            });
                        })
                        .Configure(app =>
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI(c =>
                            {
                                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Integration Utility v1");
                            });
                            app.UseRouting();
                            app.UseEndpoints(ep => ep.MapControllers());
                        });

                });
        }

        async Task IReceiver.start()
        {
            if (this.mockBody != null)
            {
                Logger.log("HTTPReceiverSwagger: Mock mode is enabled, returning the mock body " + this.mockBody);
                await _host.signal(this.mockBody, "hz");
                return;
            }
            
            if (this.mockFile != null)
            {
                Logger.log("HTTPReceiverSwagger: Mock mode is enabled, reading the mock file " + this.mockFile);
                var mockResponse = File.ReadAllText(this.mockFile);
                Logger.log("HTTPReceiverSwagger: Mock response: " + mockResponse);
                await _host.signal(mockResponse, "hz");
                return;
            }
            
            // No mocks given, start the server.
            // Finish the host configuration.
            Logger.log("HTTPReceiverSwagger: finish building the host");
            
            var serverPart = await CompileControllerAssemblyPartAsync();

            // Dynamically implement the IController interface for Controller and add it to the service container
            var requestHandler = new RequestHandler(this);
            var controllerImplAssembly = requestHandler.ImplementController(serverPart.Assembly);

            var host = _hostBuilder.ConfigureWebHost(webBuilder =>
            {
                // Parse this.address to IPAddress
                IPAddress ipAddress;
                if (string.IsNullOrEmpty(address))
                {
                    Logger.log("HTTPReceiverSwagger: address is not set, listening on localhost");
                    ipAddress = IPAddress.Loopback;
                }
                else
                {
                    if (address == "localhost")
                        ipAddress = IPAddress.Loopback;
                    else if (!IPAddress.TryParse(address, out ipAddress))
                        throw new Exception("Invalid IP address to listen on: " + address);
                }
                
                if (certSubject != null)
                {
                    Logger.log("HTTPReceiverSwagger: Configuring HTTPS with certificate subject " + certSubject);
                    webBuilder.UseKestrel(opt =>
                        opt.Listen(ipAddress, port,
                            options => options.UseHttps(FindMatchingCertificateBySubject(certSubject))));
                }
                else
                {
                    webBuilder.UseKestrel(opt => opt.Listen(ipAddress, port));
                    Logger.log(
                        "HTTPReceiverSwagger: Not using HTTPS because certSubject has not been given in pipeline definition.");
                }

                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllers()
                        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                        .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(serverPart));

                    // Register the IController implementation in the service container
                    var implementationClass = controllerImplAssembly.GetType("ControllerImpl");
                    Debug.Assert(implementationClass != null,
                        "ControllerImpl not found in the dynamically generated controller assembly. This is not supposed to happen.");

                    var controllerInterface= serverPart.Assembly.DefinedTypes.First(t => t.Name == "IController");
                    services.AddSingleton(controllerInterface, implementationClass);
                    
                    // Register the RequestHandler in the service container
                    services.AddSingleton(requestHandler);
                });
            }).Build();

            Logger.log("HTTPReceiverSwagger: Starting the host");
            await host.RunAsync();
        }

        async Task IReceiver.sendResponse(string response, object context)
        {
            if (this._debugMode)
            {
                Logger.log("Send response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", this._host.IDStep, response);
                Logger.log("Response: {response}", Serilog.Events.LogEventLevel.Debug, response);
            }

            if (context is SyncroItem item)
            {
                item.answer = response;
                Interlocked.Increment(ref item.srabot);
                item.semaphore.Set();
            }
        }

        private async Task signal1(string body,SyncroItem semaphoreItem)
        {
            await this._host.signal(body, semaphoreItem);
            // semaphoreItem.semaphore.
            if (semaphoreItem.srabot==0)
            {
                if (this._debugMode)
                {
                    Logger.log("Answer without response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", this._host.IDStep, "-");

                }
                semaphoreItem.semaphore.Set();
            }
        }
        public class SyncroItem
        {
            public int srabot = 0;
            public int unwait = 0;
            public string answer="";
            public List<KestrelServer.Header> headers = new List<KestrelServer.Header>(); 
            public AsyncAutoResetEvent semaphore = new AsyncAutoResetEvent();
        }
        public class KestrelServer: Kestrel.KestrelServerImplement
        {
            HTTPReceiverSwagger owner;
            public KestrelServer(HTTPReceiverSwagger owner)
            {
                this.owner = owner;
            }


            public static long CountExecuted = 0;
            public static long CountOpened = 0;
            public static Metrics.MetricCount metricCountOpened = new Metrics.MetricCount("HTTPOpenConnectCount", "opened http connection at same time ");
            public static Metrics.MetricCount metricErrors = new Metrics.MetricCount("HTTPErrorCount", "Error http request ");
            public static Metrics.MetricCount metricCountExecuted = new Metrics.MetricCount("HTTPExecutedConnections", "All http executed connection's ");
            public static Metrics.MetricCount metricTimeExecuted = new Metrics.MetricCount("HTTPExecutedTime", "All http executed connection's time");
            public async Task<string> GetMetrics()
            {
                return Metrics.metric.getPrometeusMetric();
                //            return 1;
            }

            public class Header
            {
                public string Key;
                public string? Value;
            }
           // List<Header> headers = new List<Header>();
            public override async Task ReceiveRequest(HttpContext httpContext)
            {
                throw new NotImplementedException("This method is not supposed to be called and should be deleted eventually.");
            }
        }
        static string template = "";


        public static string GetSwaggerHtmlBody(string json_body)
        {
            if(template =="")
            {
                using(StreamReader sr = new StreamReader("SwaggerTemplate.txt"))
                {
                    template = sr.ReadToEnd();
                }
            }
            return template.Replace("{0}", json_body);
        }
        
        IReceiverHost IReceiver.host
        {
            get => _host;
            set => _host = value;
        }

        bool IReceiver.cantTryParse => this.cantTryParse;

        bool IReceiver.debugMode
        {
            get => _debugMode;
            set => _debugMode = value;
        }
    }

}
