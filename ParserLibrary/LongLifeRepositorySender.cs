using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class LongLifeRepositorySender: Sender, ISelfTested
    {
        public class Item
        {
            public int count = 0;
            public string path;
            public List<string> values;
        }
        List<Item> list = new List<Item>();
        bool changed = false;

        public override TypeContent typeContent => TypeContent.internal_list;// throw new NotImplementedException();

        public async override Task<string> sendInternal(AbstrParser.UniEl root, Step.ContextItem context)
        {

            List<AbstrParser.UniEl> stored_list =root.toList();

            Add(stored_list);
            return "";
//            base.send(root);
        }

        void Add(List<AbstrParser.UniEl> stored_list)
        {
            DateTime time1 = DateTime.Now;
            foreach(var item in stored_list.Where(ii=>ii.Value != null))
            {
                var path = item.path;
                var item1 = list.FirstOrDefault(ii => ii.path == path);
                if(item1== null)
                {
                    item1 = new Item() { path = path, values = new List<string>() };
                    list.Add(item1);
                }
                item1.count++;
                string ss = item.Value.ToString();
                if (item1.values.Count(ii => ii == ss) == 0)
                {
                    
                    item1.values.Add(ss);
                    changed = true;
                }
            }
            AbstrParser.regEvent("SR", time1);

        }

        public Task<(bool, string, Exception)> isOK()
        {
            Logger.log("Consider implementing LongLifeRepositorySender.isOK(). Returning true for now.", Serilog.Events.LogEventLevel.Warning);
            return Task.FromResult((true, string.Empty, (Exception)null));
        }
    }
}
