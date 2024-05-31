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
using System.Threading.Tasks;

namespace ParserLibrary;

public class TestReceiver:Receiver
{
    public string path;
    public string pattern="";



    protected async override Task startInternal()
    {

        foreach (var file_name in ((pattern == "") ? new string[] { path } : Directory.GetFiles(path, pattern)))
        {
            using (StreamReader sr = new StreamReader(file_name))
            {
                var body = sr.ReadToEnd();
                await signal(body,null);

            }
        }

    }
}