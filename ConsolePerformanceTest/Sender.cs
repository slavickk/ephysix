﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace ConsolePerformanceTest
{
    public class Sender
    {
        public class ResponseFromRex
        {
            public HttpResponseMessage resp;
            public string TraceID;
        }
        const int maxRetry = 1;
        public static async Task<ResponseFromRex> SendToRexx(HttpClient httpClient, Uri url, string request)
        {
            Random rnd = new Random();
            int attempt = 0;
        restart:
            try
            {
                HttpContent content = new StringContent(request);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");  //  "application/json";
                return new ResponseFromRex() 
                {
                resp = await httpClient.PostAsync(url, content)
//                resp = await httpClient.PostAsJsonAsync(url, request), TraceID = ((System.Diagnostics.Activity.Current != null) ? System.Diagnostics.Activity.Current.Id : "unknown") 
                };
            }
            catch (Exception e56)
            {
                if (++attempt < maxRetry)
                {
                  /*  if (attempt > 1)
                        await Task.Delay(150*attempt+rnd.Next(150));*/
                    goto restart;
                }
                //Interlocked.Increment(ref OrexRefiller.allErrors);
                //                    Interlocked.Increment(ref OrexRefiller.allErrors);

                // new LogProtoError() { exeption = e56 }.log();
                return new ResponseFromRex()
                {
                    resp = null
                    //                resp = await httpClient.PostAsJsonAsync(url, request), TraceID = ((System.Diagnostics.Activity.Current != null) ? System.Diagnostics.Activity.Current.Id : "unknown") 
                };
            }

        }

    }
}
