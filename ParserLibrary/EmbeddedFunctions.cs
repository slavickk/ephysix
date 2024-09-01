using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class EmbeddedFunctions
    {
        public static Dictionary<string, Func<List<string>, string>> embeddedFunctions = new Dictionary<string, Func<List<string>, string>>();
        public class FuncDescriptionAttribute : System.Attribute
        {
            public string description;
            public FuncDescriptionAttribute(string Description)
            {
                description = Description;
            }
        }

        [FuncDescription( "текущее время в ISO ")]
        static string curr_time(List<string> pars)
        {
            return DateTime.Now.ToString("o");
        }

        public static bool isEmbeddedFunc(string val)
        {
            var arr=val.ToCharArray();
            return arr[0] == '$' && arr[arr.Length - 1] == '$';
        }
        public static string exec(string func_body)
        {
            if(!isEmbeddedFunc(func_body)) 
                return String.Empty;
            List<string> pars = new List<string>(); 
            func_body = func_body.Substring(1, func_body.Length - 2);
            int indexBeg = func_body.IndexOf('(');
            string funcName = "";
            if (indexBeg > -1)
            {
                funcName = func_body.Substring(0, indexBeg);
                int indexEnd = func_body.LastIndexOf(')');
                if (indexEnd == -1)
                    throw new Exception("in func body " + func_body + " expected )");
                var args = func_body.Substring(indexBeg + 1, indexEnd - indexBeg - 2);
                foreach (var arg in args.Split(';'))
                    if (isEmbeddedFunc(arg))
                        pars.Add(exec(arg));
                    else
                        pars.Add(arg);
            } else
                funcName = func_body;
            return embeddedFunctions[funcName](pars);
        }
        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                foreach (var meth in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    foreach (var attr in meth.CustomAttributes.Where(ii => ii.AttributeType == typeof(FuncDescriptionAttribute)))
                    {
                      //  bool isLongExec = (bool)attr.ConstructorArguments[4].Value;

                        Func<List<string>, string> action = null;
                            action = (Func<List<string>, string>)meth.CreateDelegate(typeof(Func<List<string>, string>));
                       /* else
                            actionAsync = (Func<List<object>, Task<object>>)meth.CreateDelegate(typeof(Func<List<object>, Task<object>>));*/

                        var description = attr.ConstructorArguments[0].Value.ToString();
                        embeddedFunctions.Add(meth.Name, action);
                        

                    }
                }

            }

            //callableFunction=callableFunction.OrderBy(ii => ii.order).ToList();
        }

    }
}
