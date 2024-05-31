/******************************************************************
 * File: ContextItem.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UniElLib
{


    public class ContextItem
    {
        public DateTime startTime;
        public class StatItem
        {
 
            public string Name { get; set; }
            public long ticks { get; set; }=-1;
        }
        public List<UniElLib.AbstrParser.UniEl> list = new List<UniElLib.AbstrParser.UniEl>();
        public List<StatItem> stats = null;// new List<StatItem>();
        public object context;
        public Activity mainActivity;
        public static string ConstPrev = new string(Enumerable.Range(0, 3).Select(ii => (char)(65 + new Random().Next(25))).ToArray());
        public int increment;
        public string fileNameT;
        //    public Scenario currentScenario = null;

        public string GetPrefix(string context)
        {
            return $"{ConstPrev}_a{increment}_{context}";
        }


    }
}
