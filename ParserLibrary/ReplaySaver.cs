/******************************************************************
 * File: ReplaySaver.cs
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

using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UniElLib;
using YamlDotNet.Serialization;

namespace ParserLibrary;

public class ReplaySaver
{
    [YamlIgnore]
    public bool enable
    {
        get => !string.IsNullOrEmpty(path);
    }
    public string path;

    public class SaveItem
    {
        public string fileName; 
        public string value;
    }

    ConcurrentQueue<SaveItem> queue = null;
    Thread t;
    void writeToReplay()
    {
        //            using (StreamWriter sw = new StreamWriter(@"Log.info"))
        {
            for (; ; )
            {
                SaveItem el;
                while (queue.TryDequeue(out el))
                {
//                    var fileName=Path.GetRandomFileName();
                    using(StreamWriter sw = new StreamWriter(Path.Combine(path,el.fileName/*fileName*/)))
                    {
                        sw.Write(el.value);

                    }
                    el = null;
                }

                Thread.Sleep(100);
            }
        }
    }

    /*string fileNamePatt = "";
    public void setContext()
    {
        fileNamePatt = Path.GetRandomFileName();
    }*/
    public virtual string save(string input, string context = "")
    {
        if (enable)
        {
            if (queue == null)
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                } 
                queue = new ConcurrentQueue<SaveItem>();
                t = new Thread(writeToReplay);
                t.Start();

            }
            var fileName = context ;
            SaveItem item = new SaveItem() { fileName = fileName, value = input.MaskSensitive() };
            queue.Enqueue(item);
            return item.fileName;
        }
        else
            return "";
    }

    public void Init()
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}