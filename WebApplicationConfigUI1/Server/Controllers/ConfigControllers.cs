/******************************************************************
 * File: ConfigControllers.cs
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using WebApplicationConfigUI1.Shared;
using ParserLibrary;
using System.Linq;
using System.Text.Json;
using System;

namespace WebApplicationConfigUI1.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Files")]
        public IEnumerable<YamlFiles> Get()
        {
            List<YamlFiles> files = new List<YamlFiles>();
            System.IO.DirectoryInfo di = new DirectoryInfo(@"c:\d\out");

//            foreach (FileInfo file in di.GetFiles())
                foreach (var file in Directory.GetFiles(@"C:\D\Out\", "*.yml"))
            {
                FileInfo fileInfo = new FileInfo(file);

                files.Add(new YamlFiles() { Date =fileInfo.CreationTime, Name = fileInfo.Name, Detail = "dfff" , full_path=fileInfo.FullName});
            }
            return files;
        }

        [HttpGet("Pipeline")]
        public Pipeline GetPipeline(string path)
        {
            return Pipeline.load(path);

        }
        [HttpGet("GetFileBody")]
        public string GetYaml(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
//            return Pipeline.load(path);

        }


        [HttpGet("GetTreeView")]
        public string GetTreeView(string path)
        {
            try
            {
                path = @"C:\D\Out\PC\0vzpdd43.vcs";
                var ans = TreeViewItemInternal.ParseInput(path, new string[] { "st1", "Rec" });
                var ser = JsonSerializer.Serialize<TreeViewItemInternal>(ans);
                return ser;
            } 
            catch(Exception e55)
            {
               return null; 
            }

        }


 

    }
}

