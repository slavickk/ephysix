/******************************************************************
 * File: TestReceiver.cs
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

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary;
using PluginBase;
using Serilog.Events;

namespace Plugins;

/// <summary>
/// A drop-in replacement for the TestReceiver that uses the new IReceiver interface.
/// </summary>
public class ITestReceiver : IReceiver
{
    public string path;
    public string pattern = "";
    bool IReceiver.MocMode { get; set; }=false;
    string IReceiver.MocBody { get; set; }
    string IReceiver.MocFile { get; set; }
    public IReceiverHost host { get; set; }

    public bool cantTryParse { get; set; }

    public bool debugMode { get; set; }
    
    // cancellation token
    private CancellationTokenSource _cts = new();

    public async Task start()
    {
        foreach (var file_name in ((pattern == "") ? new string[] { path } : Directory.GetFiles(path, pattern)))
        {
            if (_cts.Token.IsCancellationRequested)
                break;
            
            using (StreamReader sr = new StreamReader(file_name))
            {
                var body = sr.ReadToEnd();
                await host.signal(body, null);
            }
        }
    }
    
    public Task stop()
    {
        Logger.log("TestReceiver: Cancelling the receiving loop", LogEventLevel.Debug);
        _cts.Cancel();
        return Task.CompletedTask;
    }


    public Task sendResponse(string response, object context)
    {
        throw new System.NotImplementedException("The TestReceiver is not supposed to send responses");
    }
}