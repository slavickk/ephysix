/******************************************************************
 * File: AbstrParser.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

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
using System.Xml.Linq;
using DotLiquid;
using System.Collections.Concurrent;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using YamlDotNet.Core;
using System.Text.Encodings.Web;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace UniElLib
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
            return isNegative? (el?.Value?.ToString() != value_for_compare) : (el?.Value?.ToString() == value_for_compare);
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
//        protected bool cantTryParse=false;
        public static AbstrParser.UniEl getLocalRoot(AbstrParser.UniEl item1, string[] patts)
        {
            var item = item1;
            AbstrParser.UniEl itemRet = null;
            for (int i = Math.Min(item1.rootIndex, patts.Length - 1); i >= 0; i--)
            {
                while (item.rootIndex > i)
                    item = item.ancestor;
                if (!AbstrParser.isEqual(item.Name, patts[i]))
                    itemRet = item.ancestor;
                //                        return item;
            }
            if (itemRet == null)
                return item1;//changed 10.09.22
            return itemRet;
        }


        public static AbstrParser.UniEl ParseString(string templ,bool cantTryParse=false)
        {
            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            var rootElement = AbstrParser.CreateNode(null, list, "Root");
            ParseString1(templ, cantTryParse, list, rootElement);
            return rootElement;
        }

        public static AbstrParser.UniEl ParseString1(string body, bool cantTryParse, List<UniEl> list, UniEl rootElement)
        {
            try
            {
                //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
                foreach (var pars in AbstrParser.availParser)
                    if (pars.canRazbor("", body, rootElement, list, cantTryParse))
                    {
                        return rootElement;
                    }
            }
            catch
            {

            }
            return rootElement;
        }

        public static AbstrParser.UniEl ParseStringTest(string templ, List<AbstrParser.UniEl> list,AbstrParser.UniEl rootElement , bool cantTryParse = false)
        {



            AbstrParser.getApropriateParser("Rec",templ,rootElement,list,cantTryParse);
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
        {/*
            var milli = (DateTime.Now - time).TotalMilliseconds;
            var item = chronos.FirstOrDefault(ii => ii.patt == patt);
            if(item == null)
            {
                item = new ItemChrono() { patt = patt };
                chronos.Add(item);
            }
            item.intervals.Add(milli);*/
        }

        public static bool includeBodyOnComplexField = false;
        public static string[] PathBuilder(string[] paths, string commonPath = "")
        {
            if (paths.Length == 0)
                return Array.Empty<string>();
            
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
        public static ConcurrentDictionary<string,AbstrParser> preferrableParsers= new ConcurrentDictionary<string, AbstrParser>();
        public static List<AbstrParser> availParser = new List<AbstrParser>() { new JsonParser(), new XmlParser(), new Base64Parser(), new IPAddrParser() };
        public static DrawerFactory drawerFactory;



        public static bool getApropriateParser(string context, object val, UniEl ancestor, List<UniEl> list, bool cantTryParse = false)
        {
            // if body.length ==0
/*            if (string.IsNullOrEmpty(context))
                return true;*/
            int dummy;
            if (val.GetType() == typeof(JsonElement))
                return false;
            if (1==0 && val.GetType() != typeof(string) /*|| int.TryParse(val as string,out dummy)*/)
            {
                return false;
            }
//            var valString = val.ToString(); 
            if (!string.IsNullOrEmpty(context)) 
            {
                if(preferrableParsers.TryGetValue(context, out var parser))
                {
                    //
                    //   myList?.Count ?? 0
                    AddAvalParser(ancestor, parser);
                    /*                    int cc = ancestor.implementedParsers?.Count ?? 0;
                                        if(cc>0)*/
                    return parser.canRazbor(context, val as string, ancestor, list, cantTryParse);
                }
            }
            foreach (var pars in AbstrParser.availParser)
                if (pars.canRazbor(context, val as string, ancestor, list, cantTryParse))
                {
                    AddAvalParser(ancestor, pars);
                    if (!string.IsNullOrEmpty(context))
                        preferrableParsers.TryAdd(context, pars);
                    return true;
                }
                return false;

        }

        private static void AddAvalParser(UniEl ancestor, AbstrParser parser)
        {
            if (ancestor.implementedParsers == null)
                ancestor.implementedParsers = new List<AbstrParser>();
            ancestor.implementedParsers.Add(parser);
        }

        /// <summary>
        /// Converts the given node to its original form.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract string toOriginal(UniEl node);

        /// <summary>
        /// Determines whether the given line can be parsed by this parser and creates corresponding UniEl nodes in the ancestor node.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <param name="ancestor">The ancestor UniEl node to create corresponding nodes in.</param>
        /// <param name="list">The list of UniEl nodes to add new nodes to.</param>
        /// <param name="cantTryParse">A boolean value indicating whether to try parsing complex fields.</param>
        /// <returns>A boolean value indicating whether the line was successfully parsed and corresponding nodes were created.</returns>
        /// 

        public abstract bool canRazbor(string context,string line, UniEl ancestor, List<UniEl> list, bool cantTryParse = false);
        public virtual bool canRazbor(byte[] bytes, UniEl ancestor, List<UniEl> list)
        {
            return false;
        }
        public static  UniEl CreateNode(UniEl ancestor, List<UniEl> list, string name)
        {
            if (name == "Step_requestotp_2_Sign")
            {
                int yy = 0;
            }
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
        public static bool isEqual(string Name, string pattern)
        {
            bool isFound = false;
            if (!(pattern == "*") && pattern.Contains("*"))
            {
                
                isFound=Wildcard.Match(Name, pattern);
                if(isFound)
                {
                    int yy = 0;
                }
              //  int yy = 0;
            }

            return pattern == "*" || Name == pattern || isFound;
        }

        public class UniEl
        {
            public UniEl()
            {

            }
            public UniEl(UniEl ancestor)
            {
                if(ancestor.Name== "Step_requestotp_0_Sign")
                {
                    int yy = 0;
                }
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
            #region Conversion to XML
            
            public static bool ignoreNamespace = false;

            string getXMLText()
            {
                if(this.implementedParsers!=null && !alreadyInPack)
                {
                    this.PackToParsers(this);
                    return this.value1.ToString();

                }
                var n = this.childs.FirstOrDefault(ii => ii.Name == "#text");
                if (n != null)
                {
                  /*  if (n.childs.Count > 0 && n.childs[0].Name == "DMNResult")
                    {
                        int y = 0;
                    }
*/
                        if (n.value1 != null && (n.childs.Count==0 ||n.implementedParsers!= null))
                    {

                        return n.value1.ToString();
                    }
                    else
                    {
                        if (n.childs.Count > 0)
                        {
                            return n.childs[0].toJSON();
                        }
                        else
                        {
                            int y = 0;
                        }
                    }
                        }
                return "";
            }
            public class NamespaceItem
            {
                public string Name;
                public string Namespace;
            }

            public void to_xml_internal(XmlDocument xmlDoc,XmlNode node,List<NamespaceItem> namespaces)
            {
                if (Name == "-xmlns" || Name=="#text")
                    return;
                if(Name== "-BrowserInfo")
                {
                    int yy = 0;
                }
               // "root/SOAP-ENV:Envelope/SOAP-ENV:Body/Tran/Request/Specific/Tds/TranDetails/-BrowserInfo/#text"

                if (Name.Contains("UsernameToken"))
                {
                    int yy = 0;
                }
                string Namespace="";
                if(this.childs.Count>0 && this.implementedParsers == null)
                {
                    bool found = false;
                    //                    var n = this.childs.FirstOrDefault(ii => ii.Name == "-xmlns" || ii.Name.Contains("-xmlns:"));

                    //                   if (n != null)
                    string baseNamespace = "";
                    foreach(var n in this.childs.Where(ii => ii.Name == "-xmlns" || ii.Name.Contains("-xmlns:")))
                    {
                        found = true;
                        int index = n.Name.IndexOf(":");
                        Namespace = n.getXMLText();
                        if (index == -1)
                            baseNamespace = Namespace;
                        namespaces.Add(new NamespaceItem() { Name = ((index == -1) ? "" : n.Name.Substring( index+1)+":"), Namespace = Namespace });

                    } 
                    if(!found)
                    {
                        Namespace = GetNamespace(namespaces, Namespace);
                    } else
                    {
                        if(baseNamespace != "")
                            Namespace = baseNamespace;
                    }
                } else
                    Namespace = GetNamespace(namespaces, Namespace);


                if ( this.Name.Substring(0, 1) == "-")
                {
                        XmlAttribute attr = xmlDoc.CreateAttribute(Name.Substring(1));
                        attr.Value = getXMLText();
                        node.Attributes.Append(attr);
                } else
                {
                    if (!this.Name.Contains("comment"))
                    {
                        var new_node = xmlDoc.CreateNode(XmlNodeType.Element, this.Name, string.IsNullOrEmpty(Namespace)?node?.NamespaceURI:Namespace);
                        new_node.InnerText = getXMLText();
                        if (node != null)
                            node.AppendChild(new_node);
                        else
                            xmlDoc.AppendChild(new_node);
                        foreach (var item in childs)
                            item.to_xml_internal(xmlDoc, new_node, namespaces);
                    }
                }
            }
            
            private string GetNamespace(List<NamespaceItem> namespaces, string Namespace)
            {
                if (!string.IsNullOrEmpty(Name) && Name.Substring(0, 1) != "-")
                {
                    var item = namespaces.FirstOrDefault(ii => !String.IsNullOrEmpty(ii.Name) && Name.Contains(ii.Name)  );
                    if (item != null)
                        Namespace = item.Namespace;
                }

                return Namespace;
            }

            public string toXML()
            {
                List<NamespaceItem> namespaces = new List<NamespaceItem>();
                XmlDocument xmlDoc = new XmlDocument();
                to_xml_internal(xmlDoc, null,namespaces);
               
                return xmlDoc.OuterXml;

            }
       
            #endregion

            #region Conversion to JSON

            public string toJSON_old(bool maskSensitive=false,bool noPack=false)
            {
                string tt = "";
                return to_json_internal(ref tt,maskSensitive,false,noPack);
                
//                throw new Exception("not implemented");
            }
            public string toJSON(bool maskSensitive = false, bool noPack = false)
            {
                Dictionary<string, object> doc;
                //if (this.ancestor == null)
                    doc = ChildsToJson(maskSensitive, noPack);
                /*else
                {
                    doc = new Dictionary<string, object>();
                    var res = to_json_internal_new(maskSensitive, noPack);
                    doc.Add(res.key, res.val);// ..Add()
                }*/
                if (noPack)
                {
                    var jsonSerializerOptions = new JsonSerializerOptions
                    {
                        //  WriteIndented = true, // not necessary, but more readable
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    return JsonSerializer.Serialize(doc, jsonSerializerOptions);// doc.to_json_internal_new(ref tt, maskSensitive, false, noPack);
                } else
                    return JsonSerializer.Serialize(doc);// doc.to_json_internal_new(ref tt, maskSensitive, false, noPack);
                //                throw new Exception("not implemented");
            }

            #endregion
            
            bool firstElnArray(List<UniEl> arr ,int i)
            {
                /*if (arr[i].alwaysArray)
                {
                    if (arr.Count(ii => ii.Name == arr[i].Name) == 1)
                    {
                        int yy = 0;
                    }
                }*/
                return (arr[i].alwaysArray&& (i == 0 || arr[i].Name != arr[i - 1].Name)) || ((i < this.childs.Count - 1 && arr[i].Name == arr[i + 1].Name) && (i == 0 || arr[i].Name != arr[i - 1].Name));
            }
            bool lastElInArray(List<UniEl> arr, int i)
            {
                return  (( (i >0 &&  arr[i].Name == arr[i - 1].Name) || arr[i].alwaysArray )&& (i == arr.Count-1 || arr[i].Name != arr[i+ 1].Name));
            }

            string mask(string value)
            {
                return value.Replace("\"", "\\\"");
            }
            public bool packToJsonString = false;

            public static bool ignoreInternalPacket = false;
            public (string key,object val) to_json_internal_new( bool maskSensitive, bool noPack = false)
            {
                /*                JsonElement el = new JsonElement() ;
                                el.*/

                if (this.childs.Count > 0 && !packToJsonString && (this.implementedParsers == null || ignoreInternalPacket || /*(noPack && this.ancestor == null) ||*/ this.ancestor == null))
                {
                    return (this.Name, ChildsToJson(maskSensitive, noPack));
                }
                else
                {
                    if (this.implementedParsers != null && this.ancestor != null && !ignoreInternalPacket)
                    {
                        this.PackToParsers(this);
                        if (this.ancestor == null)
                        {
                            return (this.Name, this.Value);// val = this.Value.ToString();
                        }
                        else
                        {
                            return (this.Name, (maskSensitive ? this.Value.ToString().MaskSensitive() : mask(this.Value.ToString())));
//                            val += "\"" + (maskSensitive ? this.Value.ToString().MaskSensitive() : mask(this.Value.ToString())) + "\"";
                        }

                    }
                    else
                    if (packToJsonString)
                    {
                        return (this.Name, toJSON(maskSensitive,noPack));
                    }
                    else
                    {
                        if (this.Value != null)
                        {
                            if (this.Value.GetType() == typeof(string))
                                return (this.Name, (maskSensitive ? this.Value.ToString().MaskSensitive() : (this.Value.ToString())));
                            else
                                return (this.Name, this.Value);
                        }
                        else
                            return (this.Name,"");
                    }
                }
//                return val;
            }

            public string to_json_internal(ref string val,bool maskSensitive,bool isArr1=false,bool noPack=false)
            {
/*                JsonElement el = new JsonElement() ;
                el.*/
                if (val != "" && !isArr1)
                    val += "\"" + this.Name + "\":";
                if(this.Name == "PrimaryBitMap")
                {
                    int yy = 0;
                }
                if (this.childs.Count > 0 && !packToJsonString && (this.implementedParsers == null || noPack))
                {
                    val = ChildsToJson(val,maskSensitive);
                }
                else
                {
                    if (this.implementedParsers != null)
                    {
                        this.PackToParsers(this);
                        if (this.ancestor == null)
                        {
                            val = this.Value.ToString();
                        }
                        else
                        {
                            val += "\"" + (maskSensitive ? this.Value.ToString().MaskSensitive() : mask(this.Value.ToString())) + "\"";
                        }

                    } else
                    if (packToJsonString)
                    {
                        val+="\""+mask(ChildsToJson("",maskSensitive)) + "\"";
                    }
                    else
                    {
                        if (this.Value != null)
                        {
                            if (this.Value.GetType() == typeof(string))
                                val += "\"" +(maskSensitive?this.Value.ToString().MaskSensitive(): mask(this.Value.ToString())) + "\"";
                            else
                            {
                                if (this.Value != null && this.Value.GetType() == typeof(bool))
                                {
                                    //                int yy = 0;
                                    if ((bool)this.Value)
                                        val += "true";
                                    else
                                        val += "false";
                                }
                                else
                                    val += mask(this.Value.ToString());
                            }
                        }
                        else
                            val += "\"\"";
                    }
                }
                return val;
            }

            private Dictionary<string,object> ChildsToJson(bool maskSensitive,bool noPack)
            {
                Dictionary<string, object> retValue = new Dictionary<string, object>();
                string prevName = "";
                bool isArr = false;
                List<object> arr = null;
                /*if(path== "Step_0/Rec/Request/tran:Link/-Kind/#text")
                {
                    int yy = 0;
                }*/
                for (int i = 0; i < this.childs.Count; i++)
                {
                    if (i == 3)
                    {
                        int yy = 0;
                    }
                    isArr = false;
                    if (firstElnArray(this.childs, i))
                    //                        if(isArr==false && i< this.childs.Count-2 && this.childs[i].Name== this.childs[i+1].Name)
                    {
                        isArr = true;
                        if (arr == null)
                            arr = new List<object>();
                        else
                            arr.Clear();
                    }
                    if (prevName == this.childs[i].Name && i>0  )
                    {

                        isArr = true;
                    }

                    var res = this.childs[i].to_json_internal_new(maskSensitive, noPack);
                    if (!isArr)
                    {
                        retValue.TryAdd(res.key, res.val);
        //                retValue.Add(res.key, res.val);
                    }
                    else
                        arr.Add(res.val);
                    if (lastElInArray(this.childs, i))
                    {
                        isArr = false;
                        retValue.TryAdd(res.key, arr);
        //                retValue.Add(res.key, arr);
                        /*                           if (i != this.childs.Count - 1 && !(isArr && this.childs[i].Name != this.childs[i + 1].Name))
                                                       val += ",";*/
                    }
                    prevName = this.childs[i].Name;
                }
                return retValue;
            }
            private string ChildsToJson(string val,bool maskSensitive)
            {
                val += "{";
                string prevName = "";
                bool isArr = false;
                for (int i = 0; i < this.childs.Count; i++)
                {
                    /*       if(i==10)
                           {
                               int yy = 0;
                           }*/
                    if (firstElnArray(this.childs, i))
                    //                        if(isArr==false && i< this.childs.Count-2 && this.childs[i].Name== this.childs[i+1].Name)
                    {
                        isArr = true;
                        val += "\"" + this.childs[i].Name + "\":[";
                    }
                    if (prevName == this.childs[i].Name)
                    {
                        isArr = true;
                    }

                    this.childs[i].to_json_internal(ref val,maskSensitive, isArr);
                    if (lastElInArray(this.childs, i))
                    {
                        isArr = false;
                        val += "]";
                        /*                           if (i != this.childs.Count - 1 && !(isArr && this.childs[i].Name != this.childs[i + 1].Name))
                                                       val += ",";*/
                    }
                    if (i != this.childs.Count - 1 && !(isArr && this.childs[i].Name != this.childs[i + 1].Name))
                        val += ",";
                    prevName = this.childs[i].Name;
                }
                val += "}";
                return val;
            }

            public UniEl copy(UniEl newAncestor,bool alwaysCopyChild=false)
            {
                if(path == "Step_0/Rec/SOAP-ENV:Envelope/SOAP-ENV:Body/Tran/Request/Specific/Tds/TranDetails/-BrowserInfo/#text")
                {
                    int yy = 0;
                }
                var newEl= new UniEl(newAncestor) { Name = this.Name, Value = Value };
                newEl.implementedParsers = this.implementedParsers; 
                /*if(ancestor.Name.Contains("BrowserInfo"))
                {
                    int yy = 0;
                }
                if(childs.Count >0 && value1 != null)
                {
                    int yy = 0;
                }*/
                //if (this.implementedParsers == null)
                {
                    foreach (var nod in childs)
                        nod.copy(newEl);
                }
                //                    newEl.childs.Add(nod.copy(newEl));
                if (/*childs.Count == 0 ||*/ this.implementedParsers != null)
                {
                    PackToParsers(newEl);
                    if (alwaysCopyChild)
                    {
                        foreach (var nod in childs)
                            nod.copy(newEl);
                    }

                }

                return newEl;
            }
            bool alreadyInPack = false;
            protected UniEl PackToParsers(UniEl newEl)
            {
                alreadyInPack = true;
                for (int i = 0; i < this.implementedParsers.Count; i++)
                    this.value1 = this.implementedParsers[i].toOriginal(this);
                newEl.value1 = Value;
                alreadyInPack = false;
                return newEl;
            }

            public List<AbstrParser> implementedParsers = null;

            public IEnumerable<UniEl> getAllDescentants(string[] path,int index, ContextItem context)
            {
                if(index>=10)
                {
                    int yy = 0;
                }
                if (index < path.Length)
                {
                    if (isEqual(this.Name, path[index]))
                    {
                        if (index == path.Length - 1)
                            yield return this;
                        else
                        {
                            if (this.childs.Count == 0 && index < path.Length - 1 && this.value1!= null && context != null)
                            {
                                if(AbstrParser.getApropriateParser(this.path, this.value1, this, context.list, true))
/*                                foreach (var pars in AbstrParser.availParser)
                                    if (pars.canRazbor(this.value1.ToString(), this, context.list, true))*/
                                    {

                              //          isExpandedNode = true;
                                        //break;
                                    }
                            }

                            foreach (var el in this.childs)
                            {
                                //                            el.getAllDescentants(path, index + 1);
                                foreach (var el2 in el.getAllDescentants(path, index + 1,context))
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
       //     public bool isExpandedNode = false;
            public List<UniEl> childs = new List<UniEl>();
            public List<int> aaa = new List<int>();
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
            public bool alwaysArray = false;
            object value1;
            
            // TODO: consider struct for Value to avoid boxing/unboxing, but profile the program first
            // TODO: consider pre-calculating multiple representations (string, numbers, dates/times) when setting
            public object Value
            {
                set
                {
                    if(this.Name == "PrimaryBitMap")
                    {
                        int yy = 0;
                    }
                    value1 = value;
                    if (drawerFactory != null && treeNode != null)
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

        public override bool canRazbor(string context,string line, UniEl ancestor, List<UniEl> list, bool cantTryParse=false)
        {
            //            line = @"eyJCcmFuZCI6InhpYW9taSIsIkhhc05mY0hjZSI6ZmFsc2UsIkhhc0ZwU2Nhbm5lciI6dHJ1ZSwiSWQiOiIwNjZhY2MwMDE4ZDA1NWRiIiwiTWFudWZhY3RvcmVyIjoiWGlhb21pIiwiTW9kZWwiOiJSZWRtaSBOb3RlIDciLCJPc1ZlcnNpb24iOjI4LCJQcm9kdWN0IjoiQW5kcm9pZCIsIlJhbU1iIjoyNTYsIlNjcmVlbkRwaSI6IlhYSERQSSIsIlNjcmVlbkhlaWdodFB4IjoyMTMxLCJTY3JlZW5XaWR0aFB4IjoxMDgwLCJTY3JlZW5TaXplSW5jaGVzIjoyLjYzNzgwMDd9";
            byte[] bytes;
            if (IsBase64(line, out bytes))
            {
                string value = System.Text.Encoding.UTF8.GetString(bytes);
                if(AbstrParser.getApropriateParser("BASE64_" + context, value, ancestor, list, cantTryParse))
                    return true;
/*                foreach (var pars in availParser)
                    if (pars.canRazbor("BASE64_"+context, value, ancestor, list,cantTryParse))
                        return true;*/

                int yy = 0;
            }
            return false;
        }

        public override string toOriginal(UniEl ancestor)
        {
            string value = ancestor.Value.ToString();
            var bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return  Convert.ToBase64String(bytes);
        }
    }
    public class IPAddrParser : AbstrParser
    {
        public static bool IgnoreDB = true;
        const string dbPath = @"GeoData/GeoLite2-Country.mmdb";
//        DatabaseReader reader = null;
        DatabaseReader reader = null;
        public IPAddrParser()
        {
            if(!IgnoreDB)
                reader = new DatabaseReader(dbPath, FileAccessMode.Memory);
        }

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
        public override bool canRazbor(string context,string line, UniEl ancestor, List<UniEl> list, bool cantTryParse = false)
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
                var name = "TypeAddress";
                var newEl = CreateNode(ancestor, list, name);
                newEl.Value = "global";
                if (reader != null)
                {
                    var cc = reader.Country(ip);
                    name = "Address";
                    newEl = CreateNode(ancestor, list, name);
                    newEl.Value = line;


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
                }

                //                    reader.City(ip);
            }
            catch (AddressNotFoundException)
            {
                // TODO: consider using some widely adopted logging library instead of reimplementing it
                // Logger.log("addr {line} not found", Serilog.Events.LogEventLevel.Error,"any",line);
                return false;
            }
            return true;
        }

        public override string toOriginal(UniEl node)
        {
            return node.childs[0].Value.ToString();
//            throw new NotImplementedException();
        }
    }
    public class XmlParser : AbstrParser
    {

        public static bool isCorrectedNamespace = false;
        public static ConcurrentDictionary<string,string> namespaces = new ConcurrentDictionary<string,string>();

        public override bool canRazbor(string context, string line, UniEl ancestor, List<UniEl> list, bool cantTryParse = false)
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
            ExtractXmlFields(docXml.DocumentElement, ancestor, list,false,cantTryParse);

           // var xml=list[2].toXML();

            return true;

        }

        delegate bool mustRazbor(UniEl el);
        private void ExtractXmlFields(XmlNode property, UniEl ancestor, List<UniEl> list,bool isAttr=false, bool cantTryParse = false)
        {
            var type = property.GetType();
            var name = property.Name;
            if (AbstrParser.UniEl.ignoreNamespace)
                name = property.LocalName;
            var nameSpace = property.NamespaceURI;
            if(!string.IsNullOrEmpty(nameSpace))
            {
               // string myPrefix=n
                string prefix;
                if(namespaces.TryGetValue(nameSpace,out prefix))
                {
                    if(isCorrectedNamespace)
                    {
                        property.Prefix= prefix;
                        if (!AbstrParser.UniEl.ignoreNamespace)
                            name = property.Name;
                    }

                }
                else
                {

                    if (!isCorrectedNamespace)
                        namespaces.TryAdd(nameSpace, property.Prefix);
                }
            }
            if(isAttr)
            {
                name = "-" + name;
            }
            if (string.IsNullOrEmpty(name))
            {
                int yy = 0;
            }
            if (name == "tran:Link")
            {
                int yy = 0;
            }
            if(name=="t:Extension")
            {
                int yy = 0;
            }
            UniEl newEl = CreateNode(ancestor, list, name);
            var value = property.Value;
            if (type.Name == "XmlElement")
            {


                foreach (XmlNode node in property.Attributes)
                {
                    ExtractXmlFields(node, newEl, list,true,cantTryParse);
                }
            }

            foreach (XmlNode node in property.ChildNodes)
            {
                ExtractXmlFields(node, newEl, list,false,cantTryParse);
            }
            //            else
            {
                {
                    if (property.ChildNodes.Count == 0)
                    {
                        if (!cantTryParse)
                        {
                            if(AbstrParser.getApropriateParser("Prop_"+ancestor.path+"/"+property.Name, property.InnerText, newEl, list))
/*                            foreach (var pars in availParser)
                                if (pars.canRazbor(property.InnerText, newEl, list))*/
                                {
                                    if (includeBodyOnComplexField)
                                        newEl.Value = property.InnerText;

                                    return;
                                }
                        }
                        newEl.Value = property.InnerText;
                    }
                }
            }

        }

        public override string toOriginal(UniEl node)
        {
            return node.toXML();
//            throw new NotImplementedException();
        }
    }

    public class JsonParser : AbstrParser
    {
        static string rootEls = "";
        public override bool canRazbor(string context, string line, UniEl ancestor, List<UniEl> list,bool cantTryParse=false)
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
            ExtractJsonFields(el, ancestor, list,cantTryParse);
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
        /// <summary>
        /// Extracts fields from a JSON element and creates corresponding UniEl nodes in the ancestor node.
        /// </summary>
        /// <param name="el">The JSON element to extract fields from.</param>
        /// <param name="ancestor">The ancestor UniEl node to create corresponding nodes in.</param>
        /// <param name="list">The list of UniEl nodes to add new nodes to.</param>
        /// <param name="cantTryParse">A boolean value indicating whether to try parsing complex fields.</param>
        private void ExtractJsonFields(JsonElement el, UniEl ancestor, List<UniEl> list,bool cantTryParse)
        {
            if(el.ValueKind == JsonValueKind.Array)
            {
                foreach (var el1 in el.EnumerateArray())
                {
                    var newAnc = CreateNode(ancestor,list,"ItemList");
                    ExtractJsonFields(el1, newAnc, list,cantTryParse);
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
                    if(name== "BrowserInfo")
                    {

                    }
                    if (ancestor == null)
                    {
                        rootEls += (name + ";");
                        if (ancestor == null && (name == "http" || name == "everything!"))
                        {
                            int yy = 0;
                        }
                    }
                    var value = property.Value;
                    /*if (value.ToString() == "ID1")
                    {
                        int y = 0;
                    }*/

                    if (value.ValueKind == JsonValueKind.Object)
                    {
                        UniEl newEl = CreateNode(ancestor, list, name);
                        ExtractJsonFields(value, newEl, list,cantTryParse);
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
                                        ExtractJsonFields(el1, newEl, list, cantTryParse);
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
                            if (!cantTryParse)
                            {
                                if(AbstrParser.getApropriateParser("Prop_"+ancestor.path+"/"+property.Name, value, newEl, list))
                                /*foreach (var pars in availParser)
                                    if (pars.canRazbor(value.ToString(), newEl, list))*/
                                    {
                                        if (includeBodyOnComplexField)
                                            newEl.Value = GetObject(value);
                                        goto ext;
                                    }
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

        public override string toOriginal(UniEl node)
        {
            return node.toJSON(false,true);
//            throw new NotImplementedException();
        }
    }





}
