/******************************************************************
 * File: ParseHelper.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

    using Microsoft.AspNetCore.Components;
using ParserLibrary;
using System;
    using System.Collections.Specialized;
    using System.Web;

    namespace WebApplicationConfigUI1.Client
    {
        public static class ExtensionMethods
        {

        public static Pipeline pipeline;
            // get entire querystring name/value collection
            public static NameValueCollection QueryString(this NavigationManager navigationManager)
            {
                return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
            }

            // get single querystring value with specified key
            public static string QueryString(this NavigationManager navigationManager, string key)
            {
                return navigationManager.QueryString()[key];
            }
        }
    }
