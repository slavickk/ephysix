using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet;
using CSScriptLib;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.Json;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using YamlDotNet.Core.Tokens;
using Microsoft.AspNetCore.Http.Metadata;
using CSScripting;
using System.Runtime.InteropServices;

namespace ParserLibrary
{
    public interface ISelfTested
    {
        Task<(bool,string,Exception)> isOK();
    }

    public interface SenderDataExchanger
    {
        string getContent();
        void setContent(string content);
    }


    public class ScriptCompaper : ComparerV
    {
        public ScriptCompaper()
        {

        }
        MethodDelegate checker = null;
        string body = @"using System;
using ParserLibrary;
bool Filter(AbstrParser.UniEl el)
{                                                
    return true;
}";
        public string ScriptBody
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
                checker = CSScript.RoslynEvaluator
                  .CreateDelegate(body);

            }
        }

        /* = @"using System;
using ParserLibrary;
bool Filter(AbstrParser.UniEl el)
{                                                
return true;
}";*/
        public bool Compare(AbstrParser.UniEl el)
        {
            DateTime time1 = DateTime.Now;
            var ret = (bool)checker(el);
            AbstrParser.regEvent("CS", time1);
            return ret;
        }
    }


    public class FilterComparer
    {
        /*        public virtual bool filter(List<Filter> filters, List<AbstrParser.UniEl> list)
                {
                    if (filters.Count == 1)
                        return filters[0].filter(list);
                }*/
    }


    public class ReplaySaver
    {
        public string path;
        ConcurrentQueue<string> queue = null;
        Thread t;
        void writeToReplay()
        {
            //            using (StreamWriter sw = new StreamWriter(@"Log.info"))
            {
                for (; ; )
                {
                    string el;
                    while (queue.TryDequeue(out el))
                    {
                        var fileName=Path.GetRandomFileName();
                        using(StreamWriter sw = new StreamWriter(Path.Combine(path,fileName)))
                        {
                            sw.Write(el);

                        }
                        el = null;
                    }

                    Thread.Sleep(100);
                }
            }
        }
        public virtual void save(string input)
        {
            if (queue == null)
            {
                queue = new ConcurrentQueue<string>();
                t = new Thread(writeToReplay);
                t.Start();
            }
            queue.Enqueue(input);
        }

    }
}
