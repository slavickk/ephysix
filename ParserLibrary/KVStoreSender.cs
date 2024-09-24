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
    [Annotation("Сохранение и извлечение данных по ключу из кеша")]
    public class KVStoreSender : Sender
    {


        public override string getTemplate(string key)
        {
            return "{\"key\":\"\",\"body\":\"\" }";
            //            return base.getTemplate(key);
        }
        public override string getExample()
        {
            return "";
            //            return "{\"Define\":[]}";
        }

        public string OnGetErrorMessage;

        public enum TypeOper { get, put }
        public TypeOper typeOper;// { get; set; }
        public long timeLiveInMilliseconds = 1000;
        public override TypeContent typeContent => TypeContent.internal_list;
        DistributedCacheEntryOptions cache_options;

        public override void Init(Pipeline owner)
        {
            cache_options = new()
            {
                AbsoluteExpirationRelativeToNow =
 TimeSpan.FromMilliseconds(timeLiveInMilliseconds)
            };


            base.Init(owner);
        }


        public async override Task<string> sendInternal(AbstrParser.UniEl root, ContextItem context)
        {
            Logger.log("KVStoreSender started", Serilog.Events.LogEventLevel.Information);
            //            var def = root.childs.First(ii => ii.Name == "Define");
            //            if (sqlVariables == null)
            try
            {
                    var key = root.childs.First(ii => ii.Name == "key").Value.ToString();
                if (this.typeOper == TypeOper.put)
                {
                    var body = root.childs.First(ii => ii.Name == "body");
                    await EmbeddedFunctions.cacheProvider.SetStringAsync(EmbeddedFunctions.cacheProviderPrefix+key, body.toJSON(), cache_options);
                    return "";
                }
                else
                {
                    var body=await EmbeddedFunctions.cacheProvider.GetStringAsync(EmbeddedFunctions.cacheProviderPrefix + key);
                    if(string.IsNullOrEmpty(body))
                    {
                        (context.context as HTTPReceiver.SyncroItem).HTTPStatusCode = 422;
                        (context.context as HTTPReceiver.SyncroItem).isError = true;
                        if (!string.IsNullOrEmpty(this.OnGetErrorMessage))
                            (context.context as HTTPReceiver.SyncroItem).SetErrorMessage(string.Format(this.OnGetErrorMessage,key));
                    }

                    return body;
                }
            }
            catch (Exception e77)
            {
                Logger.log("KVStoreSender:{err}", Serilog.Events.LogEventLevel.Error, e77);
                throw;
                return "";
            }
        }

        //            return await base.sendInternal(root);
    }



}

