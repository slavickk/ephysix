/******************************************************************
 * File: Logger.cs
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

using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class Logger
    {
        public static LoggingLevelSwitch levelSwitch;
        public static void log(string message, LogEventLevel level = LogEventLevel.Information, params object[] propertyValues)
        {
            Log.ForContext("ctn", "any").Write(level, message, propertyValues);
           
        }
        public static void log(DateTime prevTime, string message, string context, LogEventLevel level = LogEventLevel.Information, params object[] propertyValues)
        {
            Log.ForContext("ctn", context).ForContext("intr", (DateTime.Now - prevTime).TotalMilliseconds.ToString()).Write(level, message,propertyValues);
        }

        public static void log(string message, LogEventLevel level = LogEventLevel.Information, string context = "any", params object[] propertyValues)
        {
            Log.ForContext("ctn", context).Write(level, message, propertyValues);
        }

        public static void log(string message, LogEventLevel level= LogEventLevel.Information, string context="any")
        {
            Log.ForContext("ctn", context).Write(level,message);
        }
        public static void log(string message,Exception ex, LogEventLevel level = LogEventLevel.Information, string context = "any")
        {
            Log.ForContext("ctn", context).Write(level,ex,message);
        }
        public static void log(string message, Exception ex, LogEventLevel level = LogEventLevel.Information, string context = "any", params object[] propertyValues)
        {
            Log.ForContext("ctn", context).Write(level, ex, message,propertyValues);
        }
        public static void log(DateTime prevTime,string message, string context, LogEventLevel level = LogEventLevel.Information)
        {
            Log.ForContext("ctn", context).ForContext("intr", (DateTime.Now-prevTime).TotalMilliseconds.ToString()).Write(level,message);
        }

    }
}
