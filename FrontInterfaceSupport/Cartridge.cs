/******************************************************************
 * File: Cartridge.cs
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

using Microsoft.Extensions.Configuration;
using MXGraphHelperLibrary;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontInterfaceSupport
{
    public  class Cartridge
    {
        // Func<IConfiguration, MXGraphDoc, string, string, Task<MXGraphHelperLibrary.MXGraphDoc.Box>> aa = StreamHelper.createStreamBox;
        public static Dictionary<string, Func<IConfiguration, MXGraphDoc, string, MXGraphDoc.Box, Task<MXGraphHelperLibrary.MXGraphDoc.Box>>> handlers = new Dictionary<string, Func<IConfiguration, MXGraphDoc, string, MXGraphDoc.Box, Task<MXGraphHelperLibrary.MXGraphDoc.Box>>> 
        {
            { "stream", StreamHelper.createStreamBox },
            { "dictionary", DictionaryHelper.createDictionaryBox },
            { "service", ServiceHelper.createServiceBox },
            { "transformer", TransformerHelper.createTransformerBox },
            { "filter", FilterHelper.createFilterBox },
            { "table", DBTable.createDBTableBox },
            { "csv", CSVTable.createCSVBox },
            {"receiver",ReceiverHelper.createReceiverBox }
        };

        public static string[] allSupportedTypes()
        {
            return handlers.Keys.ToArray();
        }
            public static async Task<string> createOrModifyCartridge(IConfiguration conf,string cartridgeType,string jsonMXGrapth, string cartridgeDefJson, string cartridgeId)
        {
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();

            if (handlers.ContainsKey(cartridgeType))
            {
                var oldBox=retDoc.boxes.FirstOrDefault(ii=>ii.id==cartridgeId);
                if(oldBox!=null)
                    retDoc.boxes.Remove(oldBox);
                var box = await handlers[cartridgeType](conf, retDoc, cartridgeDefJson, oldBox);
            }

            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;

        }

    }
}
