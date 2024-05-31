/******************************************************************
 * File: MetricBuilder.cs
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
using UniElLib;
using YamlDotNet.Serialization;

namespace ParserLibrary
{


    public class MetricBuilder
    {
        [YamlIgnore]
        Metrics.MetricCounters metricCounter;
        [YamlIgnore]
        static Metrics.MetricHistogram metricHist;

        public void Init()
        {
            metricCounter = new Metrics.MetricCounters(Name, Description, labels.Select(ii => ii.Name).ToArray());
            if(metricHist== null)
                metricHist = new Metrics.MetricHistogram("calc_metrics", "calculate metric time");
        }
        public async Task Fill(AbstrParser.UniEl rootElement)
        {
            DateTime time1= DateTime.Now;   
            int maxCount = -1;
            int maxIndex = -1;
            {
                int i = 0;
                foreach (var el in labels)
                {
                    el.current_values = el.Value.getAllObject(rootElement,null).ToArray();
                    if (el.current_values.Length == 0)
                        return;
                    if (el.current_values.Length > maxCount)
                    {
                        maxCount = el.current_values.Length;
                        maxIndex = i;
                    }
                    i++;
                }
            }
            string[] values = new string[labels.Count];    
            for (int i = 0; i < maxCount; i++)
            {
                for (int j=0; j < labels.Count;j++)
                {
                    var el = labels[j];
                    int index = i;
                    if( el.current_values.Length <= i )
                        index=el.current_values.Length-1;
                    values[j] = el.current_values[index].ToString();
                }
                metricCounter.AddCount(String.Join('/', values));
            }
            metricHist.Add(time1);
        }

        [YamlIgnore]
        public string PrometheusString
        {
            get
            {
                return metricCounter?.getBody();
            }
        }
        public string Name;
        public string Description;
        public class Label
        {
            public string Name;
            public OutputValue Value;
            [YamlIgnore]

            public object[] current_values;
            public override string ToString()
            {
                return Name;
            }
        }
        public List<Label> labels= new List<Label>();
    }
}
