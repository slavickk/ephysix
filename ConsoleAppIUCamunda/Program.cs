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
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using CamundaInterface;
using Microsoft.Extensions.Configuration.Json;

namespace ConsoleAppIUCamunda
{
    class Program
    {
        async static Task<int> Main(string[] args)
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
            Log.Information("Start log");
            while (0 == 0)
            {
                try
                {
                    await CamundaExecutor.fetch(new string[] { "integrity_utility", "to_dict_sender", "url_crowler", "to_exec_proc", "FimiConnector" });
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    Log.Information("Restart fetch");
                }
            }
            Console.ReadKey();
            Log.Information("Close program");
            //Configuration[]
            return 0;
        }
    }
}
