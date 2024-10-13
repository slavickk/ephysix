using DynamicExpresso;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static UniElLib.EmbeddedFunctions;

namespace UniElLib
{
    public static class EmbeddedFunctions
    {
        
        public static IDistributedCache cacheProvider;
        public static string  cacheProviderPrefix="";

        public static Dictionary<string, Func<List<object>, string>> embeddedFunctions = new Dictionary<string, Func<List<object>, string>>();
        public static Dictionary<string, string> embeddedFunctionsMask = new Dictionary<string, string>();
        public class FuncDescriptionAttribute : System.Attribute
        {
            public string description;
            public string mask;
            public FuncDescriptionAttribute(string Description,string mask="ssssss")
            {
                description = Description;
                this.mask = mask;
            }
        }

        [FuncDescription( "текущее время в ISO ")]
        static string curr_time(List<object> pars)
        {
            if (pars.Count == 0)
                 return DateTime.Now.ToString("o");
            else
                return DateTime.Now.ToString(pars[0].ToString());

        }
        [FuncDescription("взять параметр по пути ", "so")]
        static string path(List<object> pars)
        {
            string path = pars[0].ToString().Replace("@","-");
            if (pars[0].ToString().Contains("-AgentFee"))
            {
                int yy = 0;
            }
            AbstrParser.UniEl root = pars[1] as AbstrParser.UniEl;
            foreach( var el in root.getAllDescentants(path.Split('/'),0/* root.rootIndex*/, null))
            { 
                return el?.Value?.ToString();
            }
            return "";
        }
        [FuncDescription("форматировать строку  ", "so")]
        static string format(List<object> pars)
        {
            return string.Format( pars[0].ToString(),pars.Skip(1).ToArray());
        }

        [FuncDescription("транслитерировать строку  ", "so")]
        static string translit(List<object> pars)
        {
            return Transliteration.Front( pars[0].ToString());
        }
        public static Func<AbstrParser.UniEl, object> Parse(string expression)
        {
                 return new Interpreter().SetVariable("this", new EmbeddedClass()).ParseAsDelegate <Func<AbstrParser.UniEl, object>> (expression,new  string[] { "node"});
    /*        return interpreter.Eval<double>("Math.Pow(x, y) + 5",
                    new Parameter("x", typeof(double), 10),
                    new Parameter("y", typeof(double), 2));
            return interpreter.Eval(expression);*/
        }


        [FuncDescription("конкатенировать параметры ","")]
        static string concat(List<object> pars)
        {

            return string.Join("",pars.Select(ii=>ii.ToString()));
        }
        [FuncDescription("текущее время в ISO ")]
        static string gen_id(List<object> pars)
        {
            string key = "IDPrefix_" + pars[0];
            var currId = EmbeddedFunctions.cacheProvider.GetString(EmbeddedFunctions.cacheProviderPrefix + key);
            if (string.IsNullOrEmpty(currId))
                currId = "1";
            currId = (Convert.ToInt64(currId) + 1).ToString();
            TimeSpan period;
            if (!TimeSpan.TryParse(pars[pars.Count - 1].ToString(), out period))
            {
                throw new Exception("Can't parse " + pars[pars.Count - 1] + " to timespan");
            }
            DistributedCacheEntryOptions cache_options = new()
            {
                AbsoluteExpirationRelativeToNow =
      period
            };
            TimeSpan diff = TimeSpan.Zero;
            if (period.Days >= 365)
            {
                diff = TimeSpan.FromDays(-DateTime.Now.DayOfYear);
                diff = diff.Subtract(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));

            }
            else
                if (period.Days >= 7)
            {
                diff = TimeSpan.FromDays(-(int)DateTime.Now.DayOfWeek);
                diff = diff.Subtract(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            }
            else
                               if (period.Days >= 1)
            {
                diff = TimeSpan.FromHours(-(int)DateTime.Now.Hour);

                diff = diff.Subtract(new TimeSpan(0, DateTime.Now.Minute, DateTime.Now.Second));
            }
            period = period.Add(diff);
            EmbeddedFunctions.cacheProvider.SetString(EmbeddedFunctions.cacheProviderPrefix + key, currId, cache_options);
            return currId;
        }
        [FuncDescription("Генерация уникального идентификатора ")]
        static string gen_id_uniq(List<object> pars)
        {
            var val = gen_id(pars);
            var ticks = DateTime.Now.Ticks.ToString();

            return val + ticks.Substring(ticks.Length - 2);
        }

        public static bool isEmbeddedFunc(string val)
        {
            var arr=val.ToCharArray();
            return arr.Length>1 && arr[0] == '$' && arr[arr.Length - 1] == '$';
        }

        static char[] ffind = new char[] { '(', ')' };
        public static List<string> SplitSpec(this string spec)
        {
            List<string> result = new List<string>();
            int indexBeg = -1;
            int indexBegS = indexBeg;
            while (indexBeg < spec.Length)
            {
                int indexEnd = spec.IndexOf(';', indexBeg+1);
                if (indexEnd == -1)
                {
                    result.Add(spec.Substring(indexBegS + 1));
                    return result;
                }

                int indexBeg1 = spec.IndexOfAny(ffind, indexBeg + 1);
                if (indexBeg1 < 0 || indexBeg1 > indexEnd)
                {

                    result.Add(spec.Substring(indexBegS + 1, indexEnd - indexBegS - 1));
                    indexBeg =indexBegS= indexEnd;

                }
                else
                {
                    int countB = 0, countE = 0;
                    do
                    {
                        if (spec.Substring(indexBeg1, 1) == "(")
                            countB++;
                        else
                            countE++;
                        if (countB != countE)
                            indexBeg1 = spec.IndexOfAny(ffind, indexBeg1 + 1);
                    } while (countB != countE && indexBeg1 >= 0);
                    if (indexBeg1 < 0)
                        throw new Exception("Unparseable args " + spec);
                    indexBeg = indexBeg1;
                }
            }
            return result;
        }
        public static  object exec(string func_body,List<AbstrParser.UniEl> parameters = null,string prev_func="",int indexPar=-1)
        {
            if(!isEmbeddedFunc(func_body)) 
                return String.Empty;
            List<object> pars = new List<object>(); 
            func_body = func_body.Substring(1, func_body.Length - 2);
            int indexBeg = func_body.IndexOf('(');
            string funcName = "";
            if (indexBeg > -1)
            {
                funcName = func_body.Substring(0, indexBeg);
                int indexEnd = func_body.LastIndexOf(')');
                if (indexEnd == -1)
                    throw new Exception("in func body " + func_body + " expected )");
                var args = func_body.Substring(indexBeg + 1, indexEnd - indexBeg-1 );
                int i1 = 0;
                foreach (var arg in args.SplitSpec())
                {
                    if (isEmbeddedFunc(arg))
                        pars.Add(exec(arg, parameters, funcName,i1));
                    else
                    {
                        pars.Add(arg);
                    }
                    i1++;
                }
            } else
                funcName = func_body;
            int indParam;
            if (int.TryParse(funcName, out indParam) && indParam <= parameters?.Count)
            {
                var mask = embeddedFunctionsMask[prev_func];
                char symb = 's';
                if(mask.Length>= indexPar)
                    symb = mask[indexPar];
                if (symb == 's')
                    return parameters[indParam - 1]?.Value?.ToString() ?? "";
                else
                    return parameters[indParam - 1];
            }
            return embeddedFunctions[funcName](pars);
        }
        public static void Init()
        {
            embeddedFunctions.Clear();
            embeddedFunctionsMask.Clear();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if(type.Name=="EmbeddedFunctions")
                {
                    int yy = 0;
                }
                foreach (var meth in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
                {
                    foreach (var attr in meth.CustomAttributes.Where(ii => ii.AttributeType == typeof(FuncDescriptionAttribute)))
                    {
                        //  bool isLongExec = (bool)attr.ConstructorArguments[4].Value;
                        string mask = attr.ConstructorArguments[1].Value.ToString();    
                        Func<List<object>, string> action = null;
                            action = (Func<List<object>, string>)meth.CreateDelegate(typeof(Func<List<object>, string>));
                       /* else
                            actionAsync = (Func<List<object>, Task<object>>)meth.CreateDelegate(typeof(Func<List<object>, Task<object>>));*/

                        var description = attr.ConstructorArguments[0].Value.ToString();
                        if (!embeddedFunctions.TryGetValue(meth.Name, out var val))
                        {
                            embeddedFunctions.Add(meth.Name, action);
                            embeddedFunctionsMask.Add(meth.Name, mask);
                        }
                        

                    }
                }

            }

            //callableFunction=callableFunction.OrderBy(ii => ii.order).ToList();
        }

    }
    public class EmbeddedClass
    {
        public string guid(int len=16)
        {
            Random rnd = new Random();
            string retValue=rnd.Next().ToString("X");
            for(int i=1; i<len/16; i++) 
                retValue+=rnd.Next(len).ToString("X");

            return retValue;
        }
        public  string curr_time(string format= "o")
        {
            return DateTime.Now.ToString(format);
           
        }
        [FuncDescription("взять параметр по пути ", "so")]
        public string path(string path,AbstrParser.UniEl node)
        {
            AbstrParser.UniEl root = node;
            path = path.ToString().Replace("@", "-");
            foreach (var el in root.getAllDescentants(path.Split('/'), 0/* root.rootIndex*/, null))
            {
                return el?.Value?.ToString();
            }
            return "";
        }
        [FuncDescription("форматировать строку  ", "so")]
        public string format(string formatString ,params object[]  pars)
        {
            return string.Format(formatString, pars);
        }

        [FuncDescription("транслитерировать строку  ", "so")]
        public string translit(string source)
        {
            return Transliteration.Front(source);
        }
      

        [FuncDescription("конкатенировать параметры ", "")]
        public string concat(params object[] pars)
        {

            return string.Join("", pars.Select(ii => ii.ToString()));
        }
        [FuncDescription("текущее время в ISO ")]
        public string gen_id(string keyPref,string fmt)
        {
            string key = "IDPrefix_"+ keyPref;
            var currId = EmbeddedFunctions.cacheProvider.GetString(EmbeddedFunctions.cacheProviderPrefix + key);
            if (string.IsNullOrEmpty(currId) /*|| !int.TryParse(currId,out int result)*/)
                currId = "0";
            currId = (Convert.ToInt64(currId) + 1).ToString();
            TimeSpan period;
            if (!TimeSpan.TryParse(fmt, out period))
            {
 //               currId = "0";
                throw new Exception("Can't parse " + fmt + " to timespan");
            }
            DistributedCacheEntryOptions cache_options = new()
            {
                AbsoluteExpirationRelativeToNow =
      period
            };
            TimeSpan diff = TimeSpan.Zero;
            if (period.Days >= 365)
            {
                diff = TimeSpan.FromDays(-DateTime.Now.DayOfYear);
                diff = diff.Subtract(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));

            }
            else
                if (period.Days >= 7)
            {
                diff = TimeSpan.FromDays(-(int)DateTime.Now.DayOfWeek);
                diff = diff.Subtract(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            }
            else
                               if (period.Days >= 1)
            {
                diff = TimeSpan.FromHours(-(int)DateTime.Now.Hour);

                diff = diff.Subtract(new TimeSpan(0, DateTime.Now.Minute, DateTime.Now.Second));
            }
            period = period.Add(diff);
            EmbeddedFunctions.cacheProvider.SetString(EmbeddedFunctions.cacheProviderPrefix + key, currId, cache_options);
            return currId;
        }

        public string gen_id_uniq(string keyPref, string fmt)
        {
            var val = gen_id(keyPref, fmt);
            var ticks=DateTime.Now.Ticks.ToString();

            return val+ticks.Substring(ticks.Length-2);
        }


    }
}
