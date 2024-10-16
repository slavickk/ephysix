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
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PluginBase;
using YamlDotNet.Serialization;
using static ParserLibrary.Step;
using UniElLib;
using Serilog;
using static Plugins.HTTPReceiverSwagger;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ParserLibrary;

public  class HTTPSender:Sender,ISelfTested
{
    [YamlIgnore]
    HttpClient client;
    public string certName = "";
    public bool allowAutoRedirect; 
    public string certPassword = "";
    public enum Method { POST, PUT,GET,DELETE}
    public Method method = Method.POST;

  //  public string headers = "";
//        public string certThumbprint= "E77587679318FED87BB040F00D76AB461B962D95";
    public List<string> certThumbprints = new List<string> { "A77587679318FED87BB040F00D76AB461B962D95" };
    public double timeoutSendInMilliseconds = 5000;
    public Dictionary<string, string> headers  = new Dictionary<string, string>() { { "aa", "bb" } };


    public string template;

    public override string getTemplate(string key)
    {
        return template;
    }

    // public List<HTTPReceiver.KestrelServer.Header> headers;
    public HTTPSender()
    {
       /* headers = new List<HTTPReceiver.KestrelServer.Header> 
        { 
            new HTTPReceiver.KestrelServer.Header() {Key="SOAPAction",Value="fdsa44" } };*/
        createMetrics();
        //     InitClient();
    }
    string ReplaceParams(string val)
    {
        for (int i = 0; i < queryParams.Count; i++)
            val = val.Replace("{" + queryParams[i] + "}", "{" + i + "}");
        return val;
    }
    private void InitClient()
    {
        queryParams = getAllQueryParam();
        if(queryParams.Count>0)
        {
            urlsShab = urls.Select(ii => ReplaceParams(ii)).ToArray();
        }

        var handler = new HttpClientHandler() {  AllowAutoRedirect = allowAutoRedirect};
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
        
       // client.DefaultRequestHeaders.Add("Content-Type", ResponseType);

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

    public override async Task<string> send(string JsonBody, ContextItem context)
    {
        return await  internSend(JsonBody,context);
    }
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
           // var stringContent = new StringContent("", UnicodeEncoding.UTF8, ResponseType);
    
            var result = await client.GetAsync(urls[index]);
            if (!result.IsSuccessStatusCode)
            {
                Logger.log("Error get http request {res} url {url}", Serilog.Events.LogEventLevel.Error, result.StatusCode.ToString(), urls[index]);

            }
            return true;
        }
        catch (Exception e63)
        {
            Logger.log("Error get", e63, Serilog.Events.LogEventLevel.Error);
            return false;
        }


    }

    List<string> getAllQueryParam()
    {
        string urlTemplate = url;

        // Regular expression to find expressions within curly braces
        string pattern = @"\{(.*?)\}";

        // Find matches in the URL template
        MatchCollection matches = Regex.Matches(urlTemplate, pattern);

        // Create a list to hold extracted expressions
        List<string> extractedExpressions = new List<string>();

        foreach (Match match in matches)
        {
            extractedExpressions.Add(match.Groups[1].Value); // Extract captured group
        }
        return extractedExpressions;
    }

    public override string ToString()
    {
        return base.ToString()+" url:"+this.url;
    }
    public async Task<string> internSend(string body, ContextItem context)
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
        if (headers != null)
        {
            foreach (var item in headers)
                stringContent.Headers.Add(item.Key, item.Value);
        }
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
            if (!urls[index].Contains("_dummy_") )
            {
                var urlT = urls[index];
                if (queryParams.Count > 0)
                    urlT = string.Format(urlsShab[index], queryParamsValues); 
                if (method== Method.POST)
                result = await client.PostAsync(urlT, stringContent);
                else
                if(method== Method.GET)
                    result=await client.GetAsync(urlT);
                else
                if (method == Method.PUT)
                    result = await client.PutAsync(urlT, stringContent);
                else
                    result = await client.DeleteAsync(urlT);

                if (!result.IsSuccessStatusCode)
                {
                    string answer = await result.Content.ReadAsStringAsync();
                    Logger.log("Error send http request {res} {answer}", Serilog.Events.LogEventLevel.Error, "any", result.StatusCode.ToString(), answer);
                    (context.context as HTTPReceiver.SyncroItem).isError = true;
                    (context.context as HTTPReceiver.SyncroItem).HTTPStatusCode = ((int)result.StatusCode);
                    (context.context as HTTPReceiver.SyncroItem).errorContent = answer;

                    (context.context as HTTPReceiver.SyncroItem).HTTPErrorJsonText = System.Text.Json.JsonSerializer.Serialize(result.ReasonPhrase);
                    if (string.IsNullOrEmpty(result.ReasonPhrase))
                    {
                        try
                        {
                            var obj = JsonSerializer.Deserialize<JsonElement>(answer);
                            if (!(obj.ValueKind == JsonValueKind.String))
                            {
                                var jsonNode = JsonNode.Parse(answer);
                                var err = jsonNode["error"];
                                if (err != null)
                                    (context.context as HTTPReceiver.SyncroItem).HTTPErrorJsonText = System.Text.Json.JsonSerializer.Serialize(err.ToString());
                            }
                        }
                        catch (Exception e67)
                        {

                        }
                        //result.EnsureSuccessStatusCode();//Add throw
                    }
                }
            } else
            {
                Log.Information("Send request to dummy address");
                return "";
            }

        }
        catch(Exception e63)
        {
            if (kolRetry / urls.Length >= timeoutsBetweenRetryInMilli.Length)
            {
                Logger.log("Error send", e63, Serilog.Events.LogEventLevel.Error);
                (context.context as HTTPReceiver.SyncroItem).isError = true;
                (context.context as HTTPReceiver.SyncroItem).HTTPStatusCode = 503;
                (context.context as HTTPReceiver.SyncroItem).errorContent =e63.Message;
                (context.context as HTTPReceiver.SyncroItem).HTTPErrorJsonText = System.Text.Json.JsonSerializer.Serialize("Service unavailable:"+e63.Message);
               // (context.context as HTTPReceiver.SyncroItem).
                return "";
     //           throw;
            }
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


            //if(result.Content.Headers..ContentType.MediaType;)
            //if (result.Headers["AAA"])
            if (result.Content.Headers.ContentType.MediaType.ToUpper() != this.ResponseType.ToUpper())
            {
                Logger.log("Error send http request {res} {answer}", Serilog.Events.LogEventLevel.Error, "any", result.StatusCode.ToString(), response);
                (context.context as HTTPReceiver.SyncroItem).isError = true;
                (context.context as HTTPReceiver.SyncroItem).HTTPStatusCode = 415;
                (context.context as HTTPReceiver.SyncroItem).errorContent = result.Content.Headers.ContentType.MediaType;
                (context.context as HTTPReceiver.SyncroItem).HTTPErrorJsonText = System.Text.Json.JsonSerializer.Serialize("Unexpected media type "+ result.Content.Headers.ContentType.MediaType);


            }
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
    [YamlIgnore]
    List<string> queryParams;
    [YamlIgnore]
    string[] queryParamsValues;
    [YamlIgnore]
    string[] urlsShab; 

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

    public override TypeContent typeContent => TypeContent.internal_list;//throw new NotImplementedException();

    public int[] timeoutsBetweenRetryInMilli = { 100};
    static Metrics.MetricCounters sendToHttp = new Metrics.MetricCounters("sendToHttp",  "Sended streams to http time exec", new string[] { "Step" }); 
    static Metrics.MetricCounters sendToHTTPErr = new Metrics.MetricCounters("sendToHttpErr", "Sended transactions to http with error", new string[] { "Step","Reason" });
    //static Metrics.MetricCount metricExecRex = new Metrics.MetricCount("RexTimeExecution", "Rex time execution");
    protected string getVal(AbstrParser.UniEl el)
    {
        if(el.childs.Count ==0)
            return $"\"{el.Value}\"";
        else
        {
            return "{"+String.Join(",",el.childs.Select(ii=>$"\"{ii.Name}\":\"{ii.Value}\""))+"}";
        }
    }


    public async override Task<string> sendInternal(AbstrParser.UniEl root, ContextItem context)
    {
        await base.sendInternal(root,context);

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


        string str = formBody(root);
        if (sendActivity != null)
        {
            sendActivity?.SetTag("context.url", owner.owner.SaveContext(str));
        }

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

            var ans = await internSend(str,context);
            sendActivity?.AddTag("answer", ans);
            sendActivity?.AddTag("send.url", this.url);


            //                Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Information,this, str, ans);
            //              createMetrics();
            if (sendToHttp != null && this.owner != null)
                sendToHttp.AddCount(this.owner.IDStep);
            return ans;
        }
        catch (Exception e77)
        {
            //                createMetrics();
            bool toManyConnection=false;
            if (e77.InnerException != null)
            {
                if (e77.InnerException.Message.Contains("was forcibly closed by the remote host"))
                    toManyConnection = true;
            }
            string Reason = e77.GetType().Name;
            if (toManyConnection || e77 is TaskCanceledException || e77 is OperationCanceledException)
                Reason = "ConnBusy";
            if (e77 is TimeoutException )
                Reason = "Timeout";
            if (sendToHTTPErr != null)
                sendToHTTPErr.AddCount($"{owner.IDStep}/{Reason}");
            Logger.log("Error to send {sender} url {url} ctn {context} send {req}", e77, Serilog.Events.LogEventLevel.Error, context.GetPrefix(owner.IDStep + "RecAns"), this.GetType().Name, this.url, context.GetPrefix("ErrHttpSender"), str.MaskSensitive());
            if (Reason == "ConnBusy")
                throw new ConnectionBusyException();
            throw;
        }
    }

    protected override string formBody(AbstrParser.UniEl root)
    {
        if(queryParams?.Count>0)
        {
            queryParamsValues = new string[queryParams.Count];
            for(int i=0;i<root.childs.Count;i++)// var chld in root.childs)
            {
                
                int index = queryParams.IndexOf(root.childs[i].Name);
                if (index >= 0)
                {
                    queryParamsValues[index] = root.childs[i].Value?.ToString() ?? "";
                    root.childs.RemoveAt(i);
                    i--;
                }
            }
        }
        if (root.childs.Count == 0)
            return "";
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
    public class ConnectionBusyException:Exception
    {

    }
}