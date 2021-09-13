using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleAppTestUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            int cycle = 1;
            for (int i = 0; i < cycle; i++)
            {
                string Body;
                PutObjectS("POST", @"http://localhost:5000/WeatherForecast", "{\"Count\":" + i + "}", out Body);
            }



        }
        public static byte[] PutObjectS(string Method, string postUrl, string payLoad, out string Body)
        {
            Body = "";
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = Method;
            request.ContentType = "application/json";
            if (payLoad != null)
            {
                //                request.Headers.Add("X-Consul-Namespace", "team-1");
                var bytes = System.Text.Encoding.ASCII.GetBytes(payLoad);

                request.ContentLength = (bytes.Length);
                Stream dataStream = request.GetRequestStream();
                //                var bytes = System.Text.Encoding.ASCII.GetBytes(payload);
                dataStream.Write(bytes, 0, bytes.Length);
                //                Serialize(dataStream, payload);
                dataStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            if (returnString == "OK")
            {
                List<byte> outBuff = new List<byte>();
                Stream stream = response.GetResponseStream();
                byte[] buffer = new byte[5000/*response.ContentLength*/];
                int bytesRead;

                {
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);

                        while (bytesRead > 0)
                        {
                            outBuff.AddRange(buffer[..bytesRead]);

                            bytesRead = stream.Read(buffer, 0, 256);
                        }

                        ASCIIEncoding coding = new ASCIIEncoding();
                        char[] chars = coding.GetChars(outBuff.ToArray());
                        Body = new string(chars);
                        return outBuff.ToArray();
                        //           binWriter.Write(buffer1);
                    }
                }

            }
            return new byte[] { };
        }

    }
}
