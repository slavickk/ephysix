/******************************************************************
 * File: SyncClient.cs
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

#region

using MaxMind.GeoIP2.Exceptions;
using System;
using System.Net;
using System.Net.Http.Headers;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class SyncClient : ISyncClient
    {
        private readonly string _auth;
        private readonly int _timeout;
        private readonly string _userAgent;

        public SyncClient(
            string auth,
            int timeout,
            ProductInfoHeaderValue userAgent
            )
        {
            _auth = auth;
            _timeout = timeout;
            _userAgent = userAgent.ToString();
        }

        public Response Get(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = _timeout;
            request.UserAgent = _userAgent;
            request.Headers["Authorization"] = $"Basic {_auth}";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError || e.Response == null)
                {
                    throw new HttpException(
                        $"Error received while making request: {e.Message}",
                        0, uri, e);
                }
                response = (HttpWebResponse)e.Response;
            }

            using var responseStream = response.GetResponseStream();
            using var stream = new System.IO.MemoryStream();
            responseStream.CopyTo(stream);

            // The creation of an additional array with ToArray() isn't ideal,
            // but presumably most people who care about performance are using
            // the Async methods anyway. We can't use the underlying buffer as
            // that has null bytes at the end. Potentially we could refactor
            // the code to use spans.
            return new Response(uri, response.StatusCode, response.ContentType,
                stream.ToArray());
        }
    }
}
