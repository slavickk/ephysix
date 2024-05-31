/******************************************************************
 * File: AsyncClient.cs
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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class AsyncClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        public AsyncClient(
            string auth,
            int timeout,
            ProductInfoHeaderValue userAgent,
            HttpClient httpClient
            )
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

            _httpClient = httpClient;
        }

        public async Task<Response> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            // Reading to a byte array isn't ideal, but changing this would require
            // more refactoring and probably introducing completely separate code
            // paths for async vs sync. Hopefully we can get rid of the sync code at
            // some point instead.
            var content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var contentType = response.Content.Headers.GetValues("Content-Type")?.FirstOrDefault();

            return new Response(uri, response.StatusCode, contentType, content);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _httpClient.Dispose();
            }

            _disposed = true;
        }
    }
}
