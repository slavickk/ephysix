using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ParserLibrary.HTTPSender;

namespace ParserLibrary.Tests
{
    public class MockHTTPServer : Kestrel.KestrelServerImplement
    {
//        HTTPReceiver owner;
        public MockHTTPServer(int port,int maxConnectionLimits=100)
        {
            this.Start(port, maxConnectionLimits);
            //this..GetMetrics();
        }



        public class Header
        {
            public string Key;
            public string? Value;
        }
        // List<Header> headers = new List<Header>();
        public class MockItem
        {
            string Key;
            public MockItem(string Key)
            {
                this.Key = Key;
            }
            public Func<string, bool> reqMethod= (ii)=> true;
            public Func<string, bool> reqContentPath = (ii) => true;
            public Func<string, bool> reqContentType = (ii) => true;
            public Func<IHeaderDictionary, bool> reqHeaders = (ii) => true;
            public Func<string, bool> reqContentBody = (ii) => true;
            //            Func<HttpRequest, string> respMethod;
            public Func<HttpRequest, int> respStatusCode;
            //         Func<HttpRequest, string> respContentPath;
            public Func<HttpRequest, string> respContentType;
            public Func<HttpRequest, string> respContentBody;
            public Func<HttpRequest, IHeaderDictionary> respHeaders;
            public async Task<bool> handleRequest(HttpContext httpContext,string requestBody)
            {
                if (reqMethod(httpContext.Request.Method) 
                    && reqContentPath(httpContext.Request.Path.Value)
                    && reqContentType(httpContext.Request.ContentType)
                    && reqHeaders(httpContext.Request.Headers)
                    && reqContentBody(requestBody))
                {
                    if(Key=="unknown")
                    {
                        int yy = 0;
                    }
                    if(respStatusCode!= null)
                        httpContext.Response.StatusCode = respStatusCode(httpContext.Request);
                    if(respHeaders!= null)
                        foreach(var header in respHeaders(httpContext.Request))
                            httpContext.Response.Headers.Add(header);    
                    if(respContentType!= null)
                        SetResponseType(httpContext, respContentType(httpContext.Request));
                    if(respContentBody!= null)
                        await SetResponseContent(httpContext, respContentBody(httpContext.Request));
                    return true;
                }
                return false;

            }


        }
        public List<MockItem> mockItems = new List<MockItem>();

        public override async Task ReceiveRequest(HttpContext httpContext)
        {
            var body = new StreamReader(httpContext.Request.Body);
            //The modelbinder has already read the stream and need to reset the stream index
            //                body..BaseStream.Seek(0, SeekOrigin.Begin);
            var requestBody = await body.ReadToEndAsync();
            foreach (var item1 in mockItems)
            {
                if (await item1.handleRequest(httpContext,requestBody))
                    return;

            }
        }
    }

}
