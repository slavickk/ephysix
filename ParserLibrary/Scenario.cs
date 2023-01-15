using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{


    /// <summary>
    ///  Scenario item for Pipeline
    /// </summary>
    public  class Scenario
    {
        public string Description { get; set; }
        public List<Item> mocs { get; set; }=new List<Item>();
        public class Item
        { 
            public string IDStep { get; set; }
            public string MocFileReceiver { get; set; }
            public string MocFileSender { get; set; }
        }
    }
}
