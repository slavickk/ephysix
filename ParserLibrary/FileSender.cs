/******************************************************************
 * File: DummySender.cs
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
using System.Threading.Tasks;
using UniElLib;

namespace ParserLibrary;

/// <summary>
/// File sender that saves each message to a file in the specified directory.
/// </summary>
public class FileSender : Sender
{
    public override TypeContent typeContent => TypeContent.json;
    
    public string SaveDirectory { get; set; }
    
    private int _fileNumber = 0;

    public override void Init(Pipeline owner)
    {
        base.Init(owner);
        
        if (string.IsNullOrEmpty(this.SaveDirectory))
            throw new ArgumentException("SaveDirectory is not set");
        
        System.IO.Directory.CreateDirectory(this.SaveDirectory);
    }

    public override Task<string> send(string jsonBody, ContextItem context)
    {
        System.IO.File.WriteAllText(System.IO.Path.Combine(this.SaveDirectory, $"{++_fileNumber}.json"), jsonBody);
        return Task.FromResult("OK"); // some dummy result
    }
}
