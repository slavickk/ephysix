/******************************************************************
 * File: PluginsInterface.cs
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
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace ParserLibrary
{
    public class PluginsInterface
    {
        public static List<Assembly> Plugins = new List<Assembly>();    



        public static List<Type> getAllTypes()
        {
            List<Type> types = new List<Type>();
            types.AddRange(Assembly.GetAssembly(typeof(Pipeline)).GetTypes());
            foreach(var ass in Pipeline.pluginAssemblies)
                types.AddRange(ass.GetTypes());

            types.AddRange(Assembly.GetEntryAssembly().GetTypes());
            
            // Referencing Plugins here would introduce a circular dependency.
            // types.AddRange(Pipeline.getAllRegTypes(Assembly.GetAssembly(typeof(DummyProtocol1Receiver))));
            /* foreach(var ass in Plugins)
             {
                 types.AddRange(ass.GetTypes());
             }*/

            return types;
        }

        public static List<Assembly> loadPlugins(string[] paths)
        {
            Plugins.Clear();
            foreach(var path in paths) 
            {
                Plugins.Add(LoadPlugin(path));
            }
            return Plugins;
        }

        public static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(ParserLibrary.Receiver).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            
            return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginLocation));
        }

    }
}
