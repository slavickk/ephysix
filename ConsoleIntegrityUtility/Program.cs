/******************************************************************
 * File: Program.cs
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

using System;
//using System.Reflection;
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Reflection;
//using Plugins.TIC;

namespace ConsoleIntegrityUtility
{
    class Program
    {
        static int Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .AddEnvironmentVariables()
   .AddCommandLine(args)
   .Build();

            /*            JsonSender sender = new JsonSender() { url = "http://192.168.75.160:25080/api/Streams/LoadStream1" };
                        var answer=sender.internSend("{\"stream\":\"loginEnter\",\"originalTime\":\"2020-12-19T21:06:35.2387735+05:00\",\"login\":\"+79222310645\"}").Result;*/
            /*            var bytes=Convert.FromBase64String("M0RTLzNSSUluZGljYXRvcj0QM0RTL0V4cFRpbWVJbnRlcnZhbD02MBBFeHQvTmV0d29yaz0xMQ==");

                        string value = System.Text.Encoding.UTF8.GetString(bytes);*/
            /*            var pip2 = new Pipeline();
                        pip2.Save( @"aa3.yml");*/
            /*     var tt=typeof(LongLifeRepositorySender).IsSubclassOf(typeof(Sender));
                 var tt1 = typeof(LongLifeRepositorySender).IsAssignableTo(typeof(Sender));
                 // typeof(ComparerForValue).GenericTypeParameters
                 //           typeof(ComparerForValue).IsAssignableTo(typeof(ComparerV));*/
            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);

            Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
//Configuration[]
            Pipeline pip;
            try
            {
//                pip = Pipeline.load(args[0],Assembly.GetAssembly(typeof(ParserLibrary.TICReceiver)));
                pip = Pipeline.load(args[0], Assembly.GetAssembly(typeof(ParserLibrary.HTTPReceiver)));
                var suc =pip.SelfTest().GetAwaiter().GetResult().Result;

//                pip = pips[0];
            }  
            catch( Exception e77)
            {
                Console.WriteLine("Error:"+e77.Message);
                return -1;
            }
            pip.run().GetAwaiter().GetResult();
            return 0;
        }
    }
}
