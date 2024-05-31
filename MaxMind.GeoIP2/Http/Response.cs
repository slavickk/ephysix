/******************************************************************
 * File: Response.cs
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

using System;
using System.Net;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class Response
    {
        internal HttpStatusCode StatusCode { get; }
        internal Uri RequestUri { get; }
        internal byte[] Content { get; }
        internal string? ContentType { get; }

        public Response(Uri requestUri, HttpStatusCode statusCode, string? contentType, byte[] content)
        {
            RequestUri = requestUri;
            StatusCode = statusCode;
            ContentType = contentType;
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Content = content;
#pragma warning restore IDE0003
        }
    }
}