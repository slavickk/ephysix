﻿/******************************************************************
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

// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Threading.Tasks;
using ParserLibrary.TIP;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();
TIPReceiver tipReciever = new()
    {WorkDir = @"C:\TestTip"/* "/home/ilya/Documents/projects/integration-utility/TIPConsoleTest/Test"*/, DelayTime = 10};
string json_directory = Path.Combine(tipReciever.WorkDir, "json");
Directory.CreateDirectory(json_directory);
tipReciever.stringReceived = async (s, o) =>
{
    string filename = o as string;
    await File.AppendAllTextAsync(Path.Combine(json_directory, filename), s);
};
Task start = tipReciever.start();
Console.ReadKey();
Log.Information("Close programm");