/******************************************************************
 * File: PosgresExecutorSender.cs
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Npgsql;
using PluginBase;
using UniElLib;

namespace ParserLibrary
{
    [Annotation("Возможность управлять кодом HTTP ответа из пайплайна")]
    public class HTTPErrorSender : Sender
    {
        public override TypeContent typeContent => TypeContent.internal_list;

        public override string getTemplate(string key)
        {
            return "{  \"ErrorCode\": \"\",\r\n  \"data\": { \"operationId\": \"\" },\r\n  \"error\": {\r\n    \"code\": \"\",\r\n    \"message\": \"\"\r\n  }\r\n}";
            //            return base.getTemplate(key);
        }
        public override string getExample()
        {
            return "";
            //            return "{\"Define\":[]}";
        }

   
        public override void Init(Pipeline owner)
        {
 

            base.Init(owner);
        }
        public async override Task<string> sendInternal(AbstrParser.UniEl root, ContextItem context)
        {
            Logger.log("HTTPErrorSender started", Serilog.Events.LogEventLevel.Information);
            //            var def = root.childs.First(ii => ii.Name == "Define");
            //            if (sqlVariables == null)
            try
            {
                var errCodNode = root.childs.FirstOrDefault(ii => ii.Name == "ErrorCode");
                if (errCodNode != null)
                {
                    (context.context as HTTPReceiver.SyncroItem).isError = true;
                    (context.context as HTTPReceiver.SyncroItem).HTTPStatusCode = Convert.ToInt32(errCodNode.Value.ToString());
                    root.childs.Remove(errCodNode);
                }
                //context.this.StatusCode = Convert.ToInt32(errCodNode.Value.ToString());
                if((context.context as HTTPReceiver.SyncroItem).isError)
                    (context.context as HTTPReceiver.SyncroItem).SetErrorMessage(root.toJSON()/*errCodNodeText.Value.ToString()*/);

               // this.StatusMessage = errCodNodeText.Value.ToString();
            }
            catch (Exception e77)
            {
                Logger.log("HTTPErrorSender:{err}", Serilog.Events.LogEventLevel.Error, e77);
                throw;
            }
            return "";
        }

        //            return await base.sendInternal(root);
    }

    public class HTTPStatusException:Exception
    {
        public int StatusCode;
        public object StatusReasonJson;
    }

}

