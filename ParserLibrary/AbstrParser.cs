using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using System.Xml;
using System.Text.RegularExpressions;
using MaxMind.GeoIP2;
using MaxMind.Db;
using System.Net;
using MaxMind.GeoIP2.Exceptions;
using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace ParserLibrary
{

    [Serializable]
    public class ComparerForValue : ComparerV
    {
        public bool isNegative = false;

        [JsonInclude]
        public string value_for_compare { get; set; }
        public ComparerForValue(string val)
        {
            value_for_compare = val;
        }
        public ComparerForValue()
        {
        }

        public bool Compare(AbstrParser.UniEl el)
        {
            return isNegative? (el?.Value.ToString() != value_for_compare) : (el?.Value.ToString() == value_for_compare);
        }
    }

    public class ComparerForValueList : ComparerV
    {
        public bool isNegative = false;

        [JsonInclude]
        public string[] values_for_compare { get; set; }
        public ComparerForValueList(string[] val)
        {
            values_for_compare = val;
        }
        public ComparerForValueList()
        {
        }

        public bool Compare(AbstrParser.UniEl el)
        {
            return isNegative ? (!values_for_compare.Contains(el?.Value.ToString())) : (values_for_compare.Contains(el?.Value.ToString()));
        }
    }


    [Serializable]
    public class ComparerAlwaysTrue : ComparerV
    {
        public ComparerAlwaysTrue()
        {
        }

        public bool Compare(AbstrParser.UniEl el)
        {
            return true;
        }
    }

    public interface Drawer
    {
        void Update(AbstrParser.UniEl newEl);

    }
    public interface DrawerFactory
    {
        Drawer Create(AbstrParser.UniEl newEl, AbstrParser.UniEl ancestor);
    }
    public abstract class AbstrParser
    {
        public static AbstrParser.UniEl ParseString(string templ)
        {
            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            var rootElement = AbstrParser.CreateNode(null, list, "Root");
            try
            {
                //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
                foreach (var pars in AbstrParser.availParser)
                    if (pars.canRazbor(templ, rootElement, list))
                    {

                    }
            }
            catch
            {

            }
            return rootElement;
        }
        public static string RetVal()
        {
            return "111";
        }

        public class ItemChrono
        {
            public string patt;
            public List<double> intervals = new List<double>();
            public override string ToString()
            {
                var all = intervals.Sum();
                var avg = intervals.Average();

                return patt+":count:"+intervals.Count+";all:"+all+";avg:"+avg+";min:"+intervals.Min() + ";max:" + intervals.Max();
            }
        }
        public static List<ItemChrono> chronos = new List<ItemChrono>();
        public static void regEvent(string patt,DateTime time)
        {
            var milli = (DateTime.Now - time).TotalMilliseconds;
            var item = chronos.FirstOrDefault(ii => ii.patt == patt);
            if(item == null)
            {
                item = new ItemChrono() { patt = patt };
                chronos.Add(item);
            }
            item.intervals.Add(milli);
        }

        public static bool includeBodyOnComplexField = false;
        public static string[] PathBuilder(string[] paths, string commonPath = "")
        {
            int indCommon = -1;
            if (commonPath == "")
            {
                int iDelim = 0;
                for (int i = 0; i < paths.Min(ii => ii.Length); i++)
                {
                    for (int j = 1; j < paths.Length; j++)
                    {
                        if (paths[j][i] != paths[0][i])
                        {
                            indCommon = iDelim+1;
                            i = Int32.MaxValue - 5;
                            break;
                        }
                        if (paths[j][i] == '/')
                            iDelim = i;
                    }
                }
                if (indCommon < 0)
                {
                    indCommon = paths.Min(ii => ii.Length);
                }
                commonPath = paths[0].Substring(0, indCommon);
            }
            else
                indCommon = commonPath.Length;
            return paths.Select(ii => ii.Substring(indCommon)).ToArray();
        }



       // public static Drawer treeView1;
        public static List<AbstrParser> availParser = new List<AbstrParser>() { new JsonParser(), new XmlParser(), new Base64Parser(), new IPAddrParser() };
        public static DrawerFactory drawerFactory;
        public abstract bool canRazbor(string line, UniEl ancestor, List<UniEl> list);
        public virtual bool canRazbor(byte[] bytes, UniEl ancestor, List<UniEl> list)
        {
            return false;
        }
        public static  UniEl CreateNode(UniEl ancestor, List<UniEl> list, string name)
        {
            UniEl newEl = new UniEl() { Name = name, ancestor = ancestor };
            if(drawerFactory != null)
                newEl.treeNode = drawerFactory.Create(newEl, ancestor);
//             new TreeNode(newEl.Name);
  /*          if (ancestor == null)
            {
                treeView1.Nodes[0].Nodes.Add(newEl.treeNode);
                treeView1.Nodes[0].Nodes[^1].Tag = newEl;
            }
            else
            {
                ancestor.treeNode.Nodes.Add(newEl.treeNode);
                ancestor.treeNode.Nodes[^1].Tag = newEl;

            }*/
            list.Add(newEl);
            return newEl;
        }

        public class UniEl
        {
            public UniEl()
            {

            }
            public UniEl(UniEl ancestor)
            {
                ancestor.childs.Add(this);
                this.ancestor = ancestor;

            }
            public List<UniEl> toList()
            {
                List<UniEl> retValue = new List<UniEl>();
                retValue.Add(this);
                foreach (var item in childs)
                    retValue.AddRange(item.toList());
                return retValue;
            }
            public string toJSON()
            {
                string tt = "";
                return to_json_internal(ref tt);
                
//                throw new Exception("not implemented");
            }
            bool firstElnArray(List<UniEl> arr ,int i)
            {
                return (i < this.childs.Count - 2 && arr[i].Name == arr[i + 1].Name) && (i == 0 || arr[i].Name != arr[i - 1].Name);
            }
            bool lastElInArray(List<UniEl> arr, int i)
            {
                return (i >0 &&  arr[i].Name == arr[i - 1].Name) && (i == arr.Count-1 || arr[i].Name != arr[i+ 1].Name);
            }

            string mask(string value)
            {
                return value.Replace("\"", "\\\"");
            }
            public string to_json_internal(ref string val,bool isArr1=false)
            {
/*                JsonElement el = new JsonElement() ;
                el.*/
                if (val != "" && !isArr1)
                    val += "\"" + this.Name + "\":";
                if(this.Name == "PrimaryBitMap")
                {
                    int yy = 0;
                }
                if (this.childs.Count > 0)
                {
                    val += "{";
                    string prevName = "";
                    bool isArr = false;
                    for (int i = 0; i < this.childs.Count; i++)
                    {
                        if (firstElnArray(this.childs ,i))
//                        if(isArr==false && i< this.childs.Count-2 && this.childs[i].Name== this.childs[i+1].Name)
                        {
                            isArr = true;
                            val += "\"" + this.childs[i].Name + "\":[";
                        }
                        if (prevName == this.childs[i].Name)
                        {
                            isArr = true;
                        }

                        this.childs[i].to_json_internal(ref val,isArr);
                        if (lastElInArray(this.childs, i))
                        {
                            isArr = false;
                            val += "]";
 /*                           if (i != this.childs.Count - 1 && !(isArr && this.childs[i].Name != this.childs[i + 1].Name))
                                val += ",";*/
                        }
                        if (i != this.childs.Count - 1 && !(isArr  && this.childs[i].Name != this.childs[i + 1].Name))
                            val += ",";
                        prevName = this.childs[i].Name;
                    }
                    val += "}";
                } else
                {
                    if (this.Value != null)
                    {
                        if (this.Value.GetType() == typeof(string))
                            val += "\"" + mask(this.Value.ToString()) + "\"";
                        else
                        {
                            if(this.Value != null && this.Value.GetType() == typeof(bool))
                            {
                //                int yy = 0;
                                if ((bool)this.Value)
                                    val += "true";
                                else
                                    val += "false";
                            } else
                                val += mask(this.Value.ToString());
                        }
                    }
                    else
                        val += "\"\"";

                }
                return val;
            }

            public UniEl copy(UniEl newAncestor)
            {
                var newEl= new UniEl(newAncestor) { Name = this.Name, Value = Value };
                foreach (var nod in childs)
                    nod.copy(newEl);
//                    newEl.childs.Add(nod.copy(newEl));
                if (childs.Count == 0)
                    newEl.value1 = Value;

                return newEl;
            }

            public IEnumerable<UniEl> getAllDescentants(string[] path,int index)
            {
                if (this.Name == path[index])
                {
                    if (index == path.Length - 1)
                        yield return this;
                    else
                    {
                        foreach (var el in this.childs)
                        {
//                            el.getAllDescentants(path, index + 1);
                            foreach (var el2 in el.getAllDescentants(path, index + 1))
                                yield return el2;

                            /*                            yield return el;
                                                        foreach (var el1 in el.childs)
                                                        {
                            //                                yield return el1;
                                                            return  el1.getAllDescentants(path,index+1));
                                                        }*/
                        }
                    }
                }
            }

            public IEnumerable<UniEl> getAllDescentants()
            {
                foreach (var el in this.childs)
                {
                    yield return el;
                    foreach (var el1 in el.childs)
                    {
                        yield return el1;
                        foreach (var el2 in el1.getAllDescentants())
                            yield return el2;
                    }
                }
            }
            private UniEl ancestor1;
            public UniEl ancestor
            {
                set
                {
                    ancestor1 = value;
                    if (ancestor1 != null)
                        rootIndex = ancestor1.rootIndex + 1;
                    else
                        rootIndex = 0;
                    if (ancestor1 != null && !ancestor1.childs.Contains(this))
                        ancestor1.childs.Add(this);
                }
                get
                {
                    return ancestor1;
                }
            }
            public List<UniEl> childs = new List<UniEl>();
            public Drawer treeNode= null;
            public int rootIndex;
            public string path
            {
                get
                {
                    if (ancestor != null)
                        return ancestor.path + "/" + Name;
                    return Name;
                }
            }
            public string Name;
            object value1;
            public object Value
            {
                set
                {
                    if(this.Name == "PrimaryBitMap")
                    {
                        int yy = 0;
                    }
                    value1 = value;
                    if (treeNode != null)
                    {
                        treeNode.Update(this);
/*                        if (treeNode.Nodes.Count == 0)
                        {
                            treeNode.Nodes.Add(value.ToString());
                            treeNode.Nodes[^1].Tag = this;
                        }
                        else
                        {
                            treeNode.Nodes[0] = new TreeNode(value.ToString());
                            treeNode.Nodes[0].Tag = this;
                        }
*/
                    }
                }
                get
                {
//                    return value1?.ToString();
                    return value1;
                }
            }

            public override string ToString()
            {
                return path;
            }
        }


    }


    public class Base64Parser : AbstrParser
    {
        public bool IsBase64(string base64String, out byte[] bytes)
        {
            bytes = null;
            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n")
               || !Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
                return false;

            try
            {
                bytes = Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception)
            {
                // Handle the exception
            }

            return false;
        }

        public override bool canRazbor(string line, UniEl ancestor, List<UniEl> list)
        {
            //            line = @"eyJCcmFuZCI6InhpYW9taSIsIkhhc05mY0hjZSI6ZmFsc2UsIkhhc0ZwU2Nhbm5lciI6dHJ1ZSwiSWQiOiIwNjZhY2MwMDE4ZDA1NWRiIiwiTWFudWZhY3RvcmVyIjoiWGlhb21pIiwiTW9kZWwiOiJSZWRtaSBOb3RlIDciLCJPc1ZlcnNpb24iOjI4LCJQcm9kdWN0IjoiQW5kcm9pZCIsIlJhbU1iIjoyNTYsIlNjcmVlbkRwaSI6IlhYSERQSSIsIlNjcmVlbkhlaWdodFB4IjoyMTMxLCJTY3JlZW5XaWR0aFB4IjoxMDgwLCJTY3JlZW5TaXplSW5jaGVzIjoyLjYzNzgwMDd9";
            byte[] bytes;
            if (IsBase64(line, out bytes))
            {
                string value = System.Text.Encoding.UTF8.GetString(bytes);
                foreach (var pars in availParser)
                    if (pars.canRazbor(value, ancestor, list))
                        return true;

                int yy = 0;
            }
            return false;
        }
    }

    public class IPAddrParser : AbstrParser
    {
        const string dbPath = @"GeoData/GeoLite2-Country.mmdb";
//        DatabaseReader reader = null;
        DatabaseReader reader = new DatabaseReader(dbPath, FileAccessMode.Memory);


        public class IPAddressRange
        {
            readonly AddressFamily addressFamily;
            readonly byte[] lowerBytes;
            readonly byte[] upperBytes;

            public IPAddressRange(string lowerInclusive, string upperInclusive)
            {
                // Assert that lower.AddressFamily == upper.AddressFamily

                this.addressFamily = IPAddress.Parse(lowerInclusive).AddressFamily;
                this.lowerBytes = IPAddress.Parse(lowerInclusive).GetAddressBytes();
                this.upperBytes = IPAddress.Parse(upperInclusive).GetAddressBytes();
            }

            public IPAddressRange(IPAddress lowerInclusive, IPAddress upperInclusive)
            {
                // Assert that lower.AddressFamily == upper.AddressFamily

                this.addressFamily = lowerInclusive.AddressFamily;
                this.lowerBytes = lowerInclusive.GetAddressBytes();
                this.upperBytes = upperInclusive.GetAddressBytes();
            }

            public bool IsInRange(IPAddress address)
            {
                if (address.AddressFamily != addressFamily)
                {
                    return false;
                }

                byte[] addressBytes = address.GetAddressBytes();

                bool lowerBoundary = true, upperBoundary = true;

                for (int i = 0; i < this.lowerBytes.Length &&
                    (lowerBoundary || upperBoundary); i++)
                {
                    if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                        (upperBoundary && addressBytes[i] > upperBytes[i]))
                    {
                        return false;
                    }

                    lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                    upperBoundary &= (addressBytes[i] == upperBytes[i]);
                }

                return true;
            }
        }
        public class GrayAddrItem
        {
            byte[] from;
            byte[] to;
            bool le(byte[] first, byte[] second)
            {
                return first[0] <= second[0] && first[1] <= second[1] && first[2] <= second[2] && first[3] <= second[3];
            }
            bool ge(byte[] first, byte[] second)
            {
                return first[0] >= second[0] && first[1] >= second[1] && first[2] >= second[2] && first[3] >= second[3];
            }

            public bool check(byte[] addr)
            {

                return ge(from, addr) && le(to, addr);
                //                return addr.Address >= from.Address && addr.Address <= to.Address;
            }
            public GrayAddrItem(string fromVal, string toVal)
            {
                from = IPAddress.Parse(fromVal).GetAddressBytes();
                to = IPAddress.Parse(toVal).GetAddressBytes();
            }
        }
        IPAddressRange[] grayAddrs = new IPAddressRange[] { new IPAddressRange("10.0.0.0","10.255.255.255"),new IPAddressRange("172.16.0.0","172.31.255.255")
            ,new IPAddressRange("192.168.0.0","192.168.255.255")
            ,new IPAddressRange("100.64.0.0","100.127.255.255")};

        /*         10.0.0.0 до 10.255.255.255 с маской 255.0.0.0 или /8
        От 172.16.0.0 до 172.31.255.255 с маской 255.240.0.0 или /12
        От 192.168.0.0 до 192.168.255.255 с маской 255.255.0.0 или /16
        От 100.64.0.0 до 100.127.255.255 с маской подсети 255.192.0.0
          */
        public override bool canRazbor(string line, UniEl ancestor, List<UniEl> list)
        {
       //     return false;
            Match match = Regex.Match(line, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            if (!match.Success || match.Length != line.Length)
                return false;

            try
            {

                if (line == "85.140.22.46")
                {
                    int yy = 0;

                }
                var ip = IPAddress.Parse(line);
                var fam = ip.AddressFamily;

                if (line == "127.0.0.1" || grayAddrs.Count(ii => ii.IsInRange(ip)) > 0)
                {
                    var name1 = "Address";
                    UniEl newEl1 = CreateNode(ancestor, list, name1);
                    newEl1.Value = line;
                    name1 = "TypeAddress";
                    newEl1 = CreateNode(ancestor, list, name1);
                    newEl1.Value = "local";
                    return true;
                }
                var cc = reader.Country(ip);
                var name = "Address";
                UniEl newEl = CreateNode(ancestor, list, name);
                newEl.Value = line;

                name = "TypeAddress";
                newEl = CreateNode(ancestor, list, name);
                newEl.Value = "global";

                name = "IsoCountryCode";
                newEl = CreateNode(ancestor, list, name);
                newEl.Value = cc.Country.IsoCode;

                name = "IsoCountryName";
                newEl = CreateNode(ancestor, list, name);
                newEl.Value = cc.Country.Name;

                name = "ContinentCode";
                newEl = CreateNode(ancestor, list, name);
                newEl.Value = cc.Continent.Code;

                name = "ContinentName";
                newEl = CreateNode(ancestor, list, name);
                newEl.Value = cc.Continent.Name;

                //                    reader.City(ip);
            }
            catch (AddressNotFoundException)
            {
                Logger.log("addr {line} not found", Serilog.Events.LogEventLevel.Error,"any",line);
                return false;
            }
            return true;
        }
    }
    public class XmlParser : AbstrParser
    {
        public override bool canRazbor(string line, UniEl ancestor, List<UniEl> list)
        {
            if (line.Contains("<SOAP-ENV"))
            {
                int yy = 0;
            }
            var line1 = line.Trim();
            if (line1.Length < 3)
                return false;
            if (!(line1[0] == '<' && line1[^1] == '>'))
                return false;
            if (line.ToUpper().Contains("<!DOCTYPE HTML"))
                return false;
            if (line.ToUpper().Contains("<HTML>"))
                return false;
            XmlDocument docXml;
            try
            {
                docXml = new XmlDocument();
                docXml.LoadXml(line);
            }
            catch
            {
                return false;
            }
            ExtractXmlFields(docXml.DocumentElement, ancestor, list);

            return true;

        }

        delegate bool mustRazbor(UniEl el);
        private void ExtractXmlFields(XmlNode property, UniEl ancestor, List<UniEl> list,bool isAttr=false)
        {
            var type = property.GetType();
            var name = property.Name;
            if(isAttr)
            {
                name = "-" + name;
            }
            if(name=="mcp:Action")
            {
                int yy = 0;
            }
            UniEl newEl = CreateNode(ancestor, list, name);
            var value = property.Value;
            if (type.Name == "XmlElement")
            {


                foreach (XmlNode node in property.Attributes)
                {
                    ExtractXmlFields(node, newEl, list,true);
                }
            }

            foreach (XmlNode node in property.ChildNodes)
            {
                ExtractXmlFields(node, newEl, list);
            }
            //            else
            {
                {
                    if (property.ChildNodes.Count == 0)
                    {
                        foreach (var pars in availParser)
                            if (pars.canRazbor(property.InnerText, newEl, list))
                            {
                                if(includeBodyOnComplexField)
                                     newEl.Value = property.InnerText;

                                return;
                            }

                        newEl.Value = property.InnerText;
                    }
                }
            }

        }

    }

    public class JsonParser : AbstrParser
    {
        static string rootEls = "";
        public override bool canRazbor(string line, UniEl ancestor, List<UniEl> list)
        {
            var line1 = line.Trim();

            if (line1.Length == 0)
                return false;
            if (!(line1[0] == '{' && line1[^1] == '}') && !(line1[0] == '[' && line1[^1] == ']'))
                return false;
            if(line1[0] == '[' && line1[^1] == ']')
            {
                /*
                if (line1.Length > 2 && !(line1[1] == '{' && line1[^2] == '}'))
                    return false;*/
            }
            JsonDocument doc;
            DateTime time1 = DateTime.Now;
            try
            {
                doc = JsonDocument.Parse(line);
            }
            catch(Exception e77)
            {
                AbstrParser.regEvent("FR", time1);

                return false;
            }
            var el = doc.RootElement;
            ExtractJsonFields(el, ancestor, list);
            AbstrParser.regEvent("PR", time1);

            var milli = (DateTime.Now - time1).TotalMilliseconds;
            return true;

        }
        object GetObject(JsonElement el)
        {
            switch (el.ValueKind)
            {
                case  JsonValueKind.Number:
                    return el.GetDouble();
                    break; // Указывает на окончание блока
                case JsonValueKind.False:
                    return false; // Выполнится, если s равно 2
                    break;
                case JsonValueKind.True:
                    return true; // Выполнится, если s равно 3
                    break;
                default:
                    return el.ToString();
            }
        }
        private void ExtractJsonFields(JsonElement el, UniEl ancestor, List<UniEl> list)
        {
            if(el.ValueKind == JsonValueKind.Array)
            {
                foreach (var el1 in el.EnumerateArray())
                {
                    var newAnc = CreateNode(ancestor,list,"ItemList");
                    ExtractJsonFields(el1, newAnc, list);
                }
            } else
            {
                if (el.ValueKind == JsonValueKind.String || el.ValueKind == JsonValueKind.Number || el.ValueKind == JsonValueKind.True)
                {
//                    UniEl newEl = CreateNode(ancestor, list, "");
                    ancestor.Value = GetObject(el);
                    return;
                }
                    
                foreach (var property in el.EnumerateObject())
                {
                    var type = property.GetType();
                    var name = property.Name;
                    if (ancestor == null)
                    {
                        rootEls += (name + ";");
                        if (ancestor == null && (name == "http" || name == "everything!"))
                        {
                            int yy = 0;
                        }
                    }
                    var value = property.Value;
                    if (value.ValueKind == JsonValueKind.Object)
                    {
                        UniEl newEl = CreateNode(ancestor, list, name);
                        ExtractJsonFields(value, newEl, list);
                    }
                    else
                    {
                        if (value.ValueKind == JsonValueKind.Array)
                        {
                            if (property.Name == "PrimaryBitMap")
                            {
                                int yy = 0;
                            }
                            foreach (var el1 in value.EnumerateArray())
                            {
                                if (el1.ValueKind != JsonValueKind.Undefined)
                                {
                                    UniEl newEl = CreateNode(ancestor, list, name);
                                    if (el1.ValueKind == JsonValueKind.Object)
                                        ExtractJsonFields(el1, newEl, list);
                                    else
                                    {
                                        // HZ
                                        newEl.Value = GetObject(el1);

                                    }
                                }
                            }
                        }
                        else
                        {
                            UniEl newEl = CreateNode(ancestor, list, name);
                            if(value.ToString().Contains("<SOAP"))
                            {
                                int yy = 0;
                            }
                            foreach (var pars in availParser)
                                if (pars.canRazbor(value.ToString(), newEl, list))
                                {
                                    if (includeBodyOnComplexField)
                                        newEl.Value = GetObject(value);
                                    goto ext;
                                }
                           newEl.Value = GetObject(value);
                        ext:;
                            /*                        if (xmlPaths.Contains(newEl.path))
                                                    {
                                                        int yy = 0;
                                                        XmlDocument docXml = new XmlDocument();
                                                        docXml.LoadXml(value.ToString());
                                                        ExtractXmlFields(docXml.DocumentElement, newEl, list);
                                                    }*/
                        }
                    }
                }

            }
        }

    }





}
