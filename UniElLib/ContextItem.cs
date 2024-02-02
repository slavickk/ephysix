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
            public long ticks { get; set; }
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
