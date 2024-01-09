using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAMP;
using UniElLib;

namespace Plugins.Parsers
{
    public class UampParser : AbstrParser
    {
        public override bool canRazbor(string context, string line, UniEl ancestor, List<UniEl> list, bool cantTryParse = false)
        {
            if(!UAMPMessage.isUAMPMessage(line))
                return false;
            var uamp=new UAMPMessage(line);
            foreach(var el in uamp.Value)
            {
                ExtractUampValue(el.Key,ancestor, list, el.Value);

            }
            return true;
           // throw new NotImplementedException();
        }

        private static void ExtractUampValue(string name,UniEl ancestor, List<UniEl> list, UAMPValue? el)
        {
           // var name1 = el.Key;
            UniEl newEl1 = CreateNode(ancestor, list, name);
            switch(el.Type)
            {
                case UAMPType.Scalar:
                    newEl1.Value = (el as UAMPScalar).Value;
                    break;
                case UAMPType.Array:
                    foreach (var el1 in (el as UAMPArray).Value)
                        ExtractUampValue(name,newEl1, list, el1);
                    break;
                case UAMPType.Struct:
                    foreach (var el1 in (el as UAMPStruct).Value)
                        ExtractUampValue(name, newEl1, list, el1);
                    break;
                case UAMPType.UAMPPackage:
                    var el5 = (el as UAMPPackage).Value;
                    for(var i = 0;i<el5.Count; i++)
                        foreach(var el6 in el5[i].Value)
                            ExtractUampValue(el6.Key, newEl1, list, el6.Value);
                    break;
                case UAMPType.UAMPMessage:
                    foreach (var el8 in (el as UAMPMessage).Value)
                    {
                        string pattern = "\\u0010";
                        int pos = el8.Key.IndexOf(pattern);
                        ExtractUampValue(el8.Key.Substring(pos+pattern.Length), newEl1, list, el8.Value);
                        if (pos > 0)
                            newEl1.Value = el8.Key.Substring(0, pos);
                    }
                    break;
                default:
                    new Exception($"Unknown UAMPType {el.Type}");
                    break;

            }
        }
    }
}
