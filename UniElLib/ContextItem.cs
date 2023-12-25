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
        public List<UniElLib.AbstrParser.UniEl> list = new List<UniElLib.AbstrParser.UniEl>();
        public object context;
        public Activity mainActivity;
        public int increment;
        public string fileNameT;
        //    public Scenario currentScenario = null;

        public string GetPrefix(string context)
        {
            return $"a{increment}_{context}";
        }


    }
}
