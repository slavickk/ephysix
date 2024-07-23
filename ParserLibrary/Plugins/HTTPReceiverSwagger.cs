/******************************************************************
 * File: HTTPReceiverSwagger.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
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
using ParserLibrary;
using PluginBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UniElLib;
using Microsoft.AspNetCore.Http.Features;
using NLog.Fluent;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Plugins
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
        /// JWT issuer signing certificate subject name that the server uses to verify the JWT token.
        /// </summary>
        public string jwtIssueSigningCertSubject;

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
            // Find the signing certificate by common name in the local certificate store
            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, subjectCommonName, false);
            
            // Ensure we have found exactly one certificate
            if (certificates.Count == 1) return certificates[0];
            
            Logger.log($"Found {certificates.Count} certificates with subject 'CN={subjectCommonName}' in the local machine store. Expected exactly one.", LogEventLevel.Error);
            throw new Exception($"Found {certificates.Count} certificates with subject 'CN={subjectCommonName}' in the local machine store. Expected exactly one.");
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
                            if (jwtIssueSigningCertSubject != null)
                            {
                                app.UseAuthentication();
                                app.UseAuthorization();
                                app.UseEndpoints(ep => ep.MapControllers().RequireAuthorization());
                            }
                            else
                            {
                                app.UseEndpoints(ep => ep.MapControllers());
                            }
                        });

                });
        }
        
        private readonly CancellationTokenSource _cts = new();

        async Task IReceiver.start()
        {
            if (!string.IsNullOrEmpty(this.mockBody))
            {
                Logger.log("HTTPReceiverSwagger: Mock mode is enabled, returning the mock body " + this.mockBody);
                await _host.signal(this.mockBody, "hz");
                return;
            }
            
            if (!string.IsNullOrEmpty(this.mockFile))
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
                
                if (!string.IsNullOrEmpty(certSubject))
                {
                    Logger.log("HTTPReceiverSwagger: Configuring HTTPS with certificate subject " + certSubject);
                    webBuilder.UseKestrel(opt =>
                        opt.Listen(ipAddress, port,
                            options => options.UseHttps(FindMatchingCertificateBySubject(certSubject))));
                }
                else
                {
                    webBuilder.UseKestrel(opt => opt.Listen(ipAddress, port));
                //    webBuilder.
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

                    // Configure JWT validation if jwtIssueSigningKey is provided
                    if (!string.IsNullOrEmpty(jwtIssueSigningCertSubject))
                    {
                        Logger.log("HTTPReceiverSwagger: jwtIssueSigningKey is given in pipeline definition, configuring JWT validation.");
                        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateAudience = false,
                                    ValidateIssuer = false,  // but the signature is still validated
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new X509SecurityKey(FindMatchingCertificateBySubject(jwtIssueSigningCertSubject))
                                };
                            });
                    }
                    else
                    {
                        services.AddAuthentication(options =>
                        {
                            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        });
                        Logger.log("HTTPReceiverSwagger: Not using JWT validation because jwtIssueSigningKey has not been given in pipeline definition.");
                    }
                });
            }).Build();
            var diagnosticSource = host.Services.GetRequiredService<DiagnosticListener>();
            using var badRequestListener = new BadRequestEventListener(diagnosticSource, (badRequestExceptionFeature) =>
            {
                Log.Error( "Bad request received");
            });
            Logger.log("HTTPReceiverSwagger: Starting the host");
            await host.RunAsync(_cts.Token);
        }

        Task IReceiver.stop()
        {
            Logger.log("HTTPReceiverSwagger: Signalling cancellation to the receiver host", LogEventLevel.Debug);
            _cts.Cancel();
            return Task.CompletedTask;
        }
        
        class BadRequestEventListener : IObserver<KeyValuePair<string, object>>, IDisposable
        {
            private readonly IDisposable _subscription;
            private readonly Action<IBadRequestExceptionFeature> _callback;

            public BadRequestEventListener(DiagnosticListener diagnosticListener, Action<IBadRequestExceptionFeature> callback)
            {
                _subscription = diagnosticListener.Subscribe(this!, IsEnabled);
                _callback = callback;
            }
            private static readonly Predicate<string> IsEnabled = (provider) => provider switch
            {
                "Microsoft.AspNetCore.Server.Kestrel.BadRequest" => true,
                _ => true
            };
            public void OnNext(KeyValuePair<string, object> pair)
            {
                Logger.log($"key {pair.Key}");
                if (pair.Key == "Microsoft.AspNetCore.Hosting.EndRequest")
                {

                }
                if (pair.Key == "Microsoft.AspNetCore.Hosting.UnhandledException")
                {

                }

                if (pair.Value is IFeatureCollection featureCollection)
                {
                    var badRequestFeature = featureCollection.Get<IBadRequestExceptionFeature>();

                    if (badRequestFeature is not null)
                    {
                        _callback(badRequestFeature);
                    }
                }
            }
            public void OnError(Exception error) 
            {
            }
            public void OnCompleted() 
            {
            }
            public virtual void Dispose() => _subscription.Dispose();
        }
        async Task IReceiver.sendResponse(string response, object context)
        {
            if (this._debugMode)
            {
                Logger.log("Send response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", this._host.IDStep, response);
                Logger.log("Response: {response}", Serilog.Events.LogEventLevel.Debug, response);
            }

            if (context is ContextItem { context: SyncroItem item })
            {
                Logger.log(
                    "HTTPReceiverSwagger: This is a workaround sendResponse branch with context being SyncroItem wrapped in Step.ContextItem. " +
                    "Most likely this branch will be unused.");
                item.answer = response;
                Interlocked.Increment(ref item.srabot);
                item.semaphore.Set();
            }
            
            // The context may be SyncroItem item directly
            if (context is SyncroItem syncroItem)
            {
                Logger.log("HTTPReceiverSwagger: Main sendResponse branch with context being SyncroItem directly");
                syncroItem.answer = response;
                Interlocked.Increment(ref syncroItem.srabot);
                syncroItem.semaphore.Set();
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
