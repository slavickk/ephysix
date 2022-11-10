using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ParserLibrary
{


    public class MetricBuilder
    {
        [YamlIgnore]
        Metrics.MetricCounters metricCounter;

        public void Init()
        {
            metricCounter = new Metrics.MetricCounters(Name, Description, labels.Select(ii => ii.Name).ToArray());
        }
        public async Task Fill(AbstrParser.UniEl rootElement)
        {
            int maxCount = -1;
            int maxIndex = -1;
            {
                int i = 0;
                foreach (var el in labels)
                {
                    el.current_values = el.Value.getAllObject(rootElement).ToArray();
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
            
        }
        public string Name;
        public string Description;
        public class Label
        {
            public string Name;
            public OutputValue Value;
            public object[] current_values;
        }
        public List<Label> labels= new List<Label>();
    }
}
