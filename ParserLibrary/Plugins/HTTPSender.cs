/******************************************************************
 * File: HTTPSender.cs
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
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary;
using PluginBase;
using UniElLib;
using YamlDotNet.Serialization;

namespace Plugins;

public  class IHTTPSender: ISender, ISelfTested
{
    [YamlIgnore]
    HttpClient client;
    public string certName = "";
    public string certPassword = "";
//        public string certThumbprint= "E77587679318FED87BB040F00D76AB461B962D95";
    public List<string> certThumbprints = new List<string> { "A77587679318FED87BB040F00D76AB461B962D95" };
    public double timeoutSendInMilliseconds = 5000;
    public List<HTTPReceiver.KestrelServer.Header> headers;
    public IHTTPSender()
    {
        createMetrics();
        //     InitClient();
    }

    private void InitClient()
    {
        var handler = new HttpClientHandler();
        handler.MaxConnectionsPerServer = 256;
        handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

        if (certName != "")
        {
            X509Certificate2 certificate = new X509Certificate2(certName, certPassword);
            /*        X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet |
                        X509KeyStorageFlags.Exportable);*/
            handler.ClientCertificates.Add(certificate);
        }
        client = new HttpClient(handler);
        client.Timeout = TimeSpan.FromMilliseconds(timeoutSendInMilliseconds);
    }
    private bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
    {
        if(certThumbprints.Count >0 && !certThumbprints.Contains(certificate.Thumbprint))
        {
            Logger.log("Invalid server certificate {Incorrect}, valid thumbprints are: {valid}.", Serilog.Events.LogEventLevel.Error, "any", certificate.Thumbprint, certThumbprints);
            return false;
        }
        return true;
        return sslErrors == SslPolicyErrors.None;

        // It is possible inpect the certificate provided by server
        Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
        Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
        Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
        Console.WriteLine($"Issuer: {certificate.Issuer}");
        Console.WriteLine($"Subject: {certificate.Subject}");

        // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
        Console.WriteLine($"Errors: {sslErrors}");
        return sslErrors == SslPolicyErrors.None;
    }
    [YamlIgnore]
    public int index = 0;


    public string ResponseType = "application/json";
    bool init = false;
    object syncro = new object();

    // public override async Task<string> send(string JsonBody, Step.ContextItem context)
    // {
    //     return await  internSend(JsonBody);
    // }
    protected async Task<bool> testGet()
    {
        if (!init)
        {
            lock (syncro)
            {
                if (!init)
                {
                    InitClient();
                    init = true;
                }
            }
        }

        try
        {
            var result = await client.GetAsync(urls[index]);
            if (!result.IsSuccessStatusCode)
            {
                Logger.log("Error get http request {res} url:{url}", Serilog.Events.LogEventLevel.Error, result.StatusCode.ToString(), urls[index]);

            }
            return true;
        }
        catch (Exception e63)
        {
            Logger.log("Error get", e63, Serilog.Events.LogEventLevel.Error);
            return false;
        }


    }


    public async Task<string> internSend(string body)
    {
        if(!init)
        {
            lock (syncro)
            {
                if (!init)
                {
                    InitClient();
                    init = true;
                }
            }
        }
        int kolRetry = 0;
        int indexDelay = 0;
        //            var myContent = JsonConvert.SerializeObject(data);
        // Затем вам нужно будет создать объект контента для отправки этих данных, я буду использовать объект ByteArrayContent , но вы можете использовать или создать другой тип, если хотите.

        /*            var buffer = System.Text.Encoding.UTF8.GetBytes(body);
                    var byteContent = new ByteArrayContent(buffer);
                    HttpContent content;
                    content.
        */
        restart:
        kolRetry++;
        HttpResponseMessage result = null;
        var stringContent = new StringContent(body, UnicodeEncoding.UTF8, ResponseType); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
        if (0 == 1)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Pragma", "no-cache");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Java/12.0.1");

            client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        }
        //stringContent.Headers
        //            client.DefaultRequestHeaders
        //            "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2"
        //          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue)= "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2"
        //        stringContent.Headers.Add("Accept", "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2");
        //stringContent.Headers.
        /*if(headers!= null)
            {
                foreach (var head in headers.Where(ii => ii.Key != "SOAPAction" && ii.Key != "Connection"))
                {
                    try
                    {
                        stringContent.Headers.Add(head.Key, head.Value);
                    }
                    catch(Exception e77)
                    {

                    }
                }
            }*/
        //stringContent.Headers.Add()
        try
        {
            result = await client.PostAsync(urls[index], stringContent);
            if(!result.IsSuccessStatusCode)
            {
                Logger.log("Error send http request {res}", Serilog.Events.LogEventLevel.Error,result.StatusCode.ToString());

            }

        }
        catch(Exception e63)
        {
            if (kolRetry / urls.Length >= timeoutsBetweenRetryInMilli.Length)
                throw;
            Logger.log("Error send", e63, Serilog.Events.LogEventLevel.Error);
            indexDelay = nextStepCalc(kolRetry, indexDelay);
            goto restart;

        }
        if (result!= null && result.IsSuccessStatusCode)
        {
               
            var response = await result.Content.ReadAsStringAsync();
            IEnumerable<string> values;
            //string rules = "";
            if (result.Headers.TryGetValues("InactiveRules", out values))
            {
                if (values.Count() >0 && !string.IsNullOrEmpty(values.First()))
                response=response.Substring(0,response.Length - 1)+((response.Length>2)?",":"")+"{\"Inactive\":["+string.Join(",",values.First().Split(";").Select(ii=>$"\"{ii}\""))+"]}]";
                //rules = values.First();
            }
            //if (result.Headers["AAA"])
            return response;
        }
        if (kolRetry / urls.Length >= timeoutsBetweenRetryInMilli.Length)
            return "";
        indexDelay = nextStepCalc(kolRetry, indexDelay);
        goto restart;

    }

    private int nextStepCalc(int kolRetry, int indexDelay)
    {
        if (++index >= urls.Length)
            index = 0;
        if (kolRetry % urls.Length == 0 && indexDelay < timeoutsBetweenRetryInMilli.Length)
        {
            Thread.Sleep(timeoutsBetweenRetryInMilli[indexDelay]);
            indexDelay++;
        }

        return indexDelay;
    }

    public string[] urls = { @"https://195.170.67:51200/Rec" };
    public string url
    {
        set
        {
            urls = new string[] { value };
            timeoutsBetweenRetryInMilli = new int[] { 100 };
        }
        get
        {
            return (urls== null || urls.Length ==0)?"":urls[0];
        }
    }

    TypeContent ISender.typeContent => TypeContent.internal_list;//throw new NotImplementedException();
    
    public int[] timeoutsBetweenRetryInMilli = { 100};
    private ISenderHost _host;
    static Metrics.MetricCount sendToRex = new Metrics.MetricCount("sendToRexSuc",  "Sended transactions to DummySystem1 time exec"); 
    static Metrics.MetricCount sendToRexErr = new Metrics.MetricCount("sendToRexErr", "Sended transactions to DummySystem1 with error");
    //static Metrics.MetricCount metricExecRex = new Metrics.MetricCount("RexTimeExecution", "Rex time execution");
    protected string getVal(UniElLib.AbstrParser.UniEl el)
    {
        if(el.childs.Count ==0)
            return $"\"{el.Value}\"";
        else
        {
            return "{"+String.Join(",",el.childs.Select(ii=>$"\"{ii.Name}\":\"{ii.Value}\""))+"}";
        }
    }


    ISenderHost ISender.host
    {
        get => _host;
        set => _host = value;
    }

    public virtual async Task<string> send(UniElLib.AbstrParser.UniEl root, ContextItem context)
    {
        // await base.sendInternal(root,context);
        string str = formBody(root);
        _host.sendActivity?.SetTag("context.url", _host.SavePipelineContext(str));

        /*            foreach (var el in root.childs)
                        {
                            str += ((first?"":",")+"\"" + el.Name + "\":" + getVal(el );
                            first = false;
                        }
                        str += "}";*/

        /*            if (owner.debugMode)
                            Console.WriteLine("Send:" + str);*/
        DateTime time1 = DateTime.Now;
        try
        {

            var ans = await internSend(str);
            _host.sendActivity?.AddTag("answer", ans);
            _host.sendActivity?.AddTag("send.url", this.url);


            //                Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Information,this, str, ans);
            //              createMetrics();
            if (sendToRex != null)
                sendToRex.Add(time1);
            return ans;
        }
        catch (Exception e77)
        {
            //                createMetrics();
            if (sendToRex != null)
                sendToRexErr.Add(time1);
            Logger.log($"Error to send {this.GetType().Name} url {this.url}", e77, Serilog.Events.LogEventLevel.Error);
            throw;
        }
    }

    protected virtual string formBody(UniElLib.AbstrParser.UniEl root)
    {
        if (ResponseType == "application/xml" || ResponseType == "text/xml")
            return root.childs[0].toXML();
        else
            return root.toJSON();
    }

    private void createMetrics()
    {
        /* if (sendToRex == null)
            {
                sendToRex = Pipeline.metrics.getMetric("sendToRex", false, true, "Sended transactions to DummySystem1");
                sendToRexErr = Pipeline.metrics.getMetric("sendToRex", true, false, "Sended transactions to DummySystem1");
            }*/
    }

    public async Task<(bool,string,Exception)> isOK()
    {
        string details;
        bool isSuccess = true;
        Exception exc = null;
        details = "Make http get to " + this.urls[0];
        try
        {
            DateTime time1 = DateTime.Now;

            var ans = await testGet(); 
            Logger.log(time1,"{Sender}-testGet:{ans}" ,"SelfTest",Serilog.Events.LogEventLevel.Information,this,ans);
            if (ans == false)
                isSuccess = false;
        }
        catch(Exception e77)
        {
            isSuccess = false;
            exc = e77;
        }
        //            if(ans)
        return (isSuccess,details,exc);
    }

    public virtual Task<string> send(string JsonBody, ContextItem context)
    {
        return internSend(JsonBody);
    }

    public virtual string getTemplate(string key)
    {
        return "";
    }

    public virtual void setTemplate(string key,string body)
    {
    }
    
    public virtual void Init()
    {
        // metricUpTimeError = new Metrics.MetricHistogram("iu_outbound_errors_total", "handle performance receiver", new double[] { 30, 100, 500, 1000, 5000, 10000 });
        // metricUpTimeError.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
        //
        // metricUpTime = new Metrics.MetricHistogram("iu_outbound_request_duration_msec", "handle performance receiver");
        // metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
    }
}