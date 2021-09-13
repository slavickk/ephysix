﻿using System;
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
            return el?.Value.ToString() == value_for_compare;
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
        public static List<AbstrParser> availParser = new List<AbstrParser>() { new JsonParser(), new Base64Parser(), new IPAddrParser() };
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
            public UniEl copy(UniEl newAncestor)
            {
                var newEl= new UniEl(ancestor) { Name = this.Name, Value = Value };
                foreach (var nod in childs)
                    newEl.childs.Add(nod.copy(newEl));
                return newEl;
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
                    return value1?.ToString();
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
        const string dbPath = @"C:\Data\GeoLite2-Country_20210720\GeoLite2-Country.mmdb";
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
                Console.WriteLine("addr " + line + "not found");
                return false;
            }
            return true;
        }
    }
    public class XmlParser : AbstrParser
    {
        public override bool canRazbor(string line, UniEl ancestor, List<UniEl> list)
        {
            if (!(line.Trim()[0] == '<' && line.Trim()[^1] == '>'))
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
        private void ExtractXmlFields(XmlNode property, UniEl ancestor, List<UniEl> list)
        {
            var type = property.GetType();
            var name = property.Name;
            UniEl newEl = CreateNode(ancestor, list, name);
            var value = property.Value;
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
            if (line.Trim().Length == 0)
                return false;
            if (!(line.Trim()[0] == '{' && line.Trim()[^1] == '}') && !(line.Trim()[0] == '[' && line.Trim()[^1] == ']'))
                return false;
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
                            foreach (var el1 in value.EnumerateArray())
                            {
                                UniEl newEl = CreateNode(ancestor, list, name);
                                if (el1.ValueKind == JsonValueKind.Object)
                                    ExtractJsonFields(el1, newEl, list);
                                else
                                {
                                    // HZ
                                }

                            }
                        }
                        else
                        {
                            UniEl newEl = CreateNode(ancestor, list, name);

                            foreach (var pars in availParser)
                                if (pars.canRazbor(value.ToString(), newEl, list))
                                {
                                    if (includeBodyOnComplexField)
                                        newEl.Value = value;
                                    goto ext;
                                }
                            newEl.Value = value;
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
