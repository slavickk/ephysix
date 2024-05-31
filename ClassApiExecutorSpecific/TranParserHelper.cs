/******************************************************************
 * File: TranParserHelper.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using CamundaInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace ClassApiExecutor
{
    public class TranParserHelper
    {
        public class Item
        {
            public class ValueItem
            {
                public string value;
                public Dictionary<string, string> descriptions = new Dictionary<string, string>();
                public override string ToString()
                {
                    return value/*+"("+string.Join(",",descriptions.Select(i1=>i1.Key+":"+i1.Value))+")"*/;
                }
            }

            public string Name { get; set; }
            public string Description { get; set; }
            public List<Item> childs { get; set; }= new List<Item>();
            public string TType { get; set; }
            public Dictionary<string, ValueItem> PossibleValues { get; set; } = new Dictionary<string, ValueItem>();
        }
        public static Item getDefine(string[] fields)
        {
            Item retValue = null;
            Dictionary<string, string> namespaces;
            XmlDocument xmlDoc;
            XmlNamespaceManager nsManager;

//            List<_ApiExecutor.ItemCommand> retValue = new List<_ApiExecutor.ItemCommand>();
            string filePath = @"C:\d\TranXSD\tran.xsd";
            var arrPath = new string[] { "tran.xsd", "types.xsd", "digitized-card.xsd" , "tokens-alias.xsd", "tran-common.xsd"
                , "common-types.xsd", "common.xsd", "eas.xsd","xscml.xsd","utils.xsd","faultdetail.xsd","groupsettings.xsd","commondef.xsd"
            ,"editmask.xsd","reports.xsd","adsdef-min.xsd","contracts-types.xsd","contracts-strategy.xsd","tariffs-admin.xsd","accounting-info.xsd"
            ,"exceptionalCard.xsd","contracts-payee-admin.xsd","contracts-notify.xsd","doer-types.xsd","issue-admin.xsd"
            ,"invoicing.xsd","cfgManagement-impExp.xsd","applications.xsd","subjects-admin.xsd","rc-admin.xsd","restricting-admin.xsd"
            ,"tranAddendum.xsd","fraudReport.xsd","contracts-standingorder-types.xsd","acquiring-admin.xsd"};
            var xmlSchemaSet = new XmlSchemaSet();
            foreach (var file in Directory.GetFiles(@"c:\d\TranXSD", "*.xsd"))
            {
                using var reader = XmlReader.Create(file);
                xmlSchemaSet.Add(XmlSchema.Read(reader, null));
            }
            xmlSchemaSet.Compile();
            var el = xmlSchemaSet.GlobalElements.Values;
            Dictionary<string, XmlSchemaElement> dict = new Dictionary<string, XmlSchemaElement>();
            int index = 229;
            int i1 = 0;
            foreach (XmlSchemaElement value in xmlSchemaSet.GlobalElements.Values)
            {
                if (i1 == index)
                {
                    using (StreamWriter sw = new StreamWriter(@"C:\D\Tran_Field.txt"))
                    {
                        int yy = 0;
                        string Name =/*"Tran/"+*/value.Name;
                        var retValue1 = NewMethod1(value, Name, new List<string>(),sw);
                    }
                }
                i1++;
//                dict.Add(value.Name, value);
                //Console.WriteLine($"XmlSchemaElement name = {value.Name}");
            }
           // var el=dict["Request"];
            xmlDoc = new XmlDocument();
            //    xmlDoc.Load(filePath);
            xmlDoc.Load(filePath);
            // xmlDoc.LoadXml(xmlContent);
            namespaces = new Dictionary<string, string>();


            //            namespaces.Add("env", "http://www.w3.org/2003/05/soap-envelope");
            namespaces.Add("env", "http://schemas.xmlsoap.org/soap/envelope/");



            namespaces.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaces.Add("m1", "http://schemas.compassplus.com/two/1.0/fimi.xsd");

            foreach (XmlAttribute attr in xmlDoc.DocumentElement.Attributes)
            {
                var attrName = attr.Name.Replace("xmlns:", "");
                if (!namespaces.ContainsKey(attrName))
                    namespaces.Add(attrName, attr.Value);
            }

            nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            foreach (var item in namespaces)
                nsManager.AddNamespace(item.Key, item.Value);
            foreach (XmlNode node in xmlDoc.SelectNodes("//xs:complexType[@name='Request']", nsManager))
            {
                retValue=NewMethod(node, node.Attributes["name"].Value);

                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (fields.Contains(attr.Name))
                    {

                    }

                }
                if (node.Attributes.GetNamedItem("name") != null)
                {
                    var Name = node.Attributes["name"].Value;
                    if (Name.Contains("Tran"))
                    {
                        int yy = 0;
                    }
                    if (Name.Substring(Name.Length - 2) == "Rq" || Name.Substring(Name.Length - 2) == "Rp")
                    {
                        _ApiExecutor.ItemCommand lastCommand = null;
                        //lastCommand = retValue.FirstOrDefault(x => x.Name == Name.Substring(0, Name.Length - 2));
                        if (lastCommand == null)
                        {
                            lastCommand = new _ApiExecutor.ItemCommand() { Name = Name.Substring(0, Name.Length - 2) };
//                            retValue.Add(lastCommand);

                        }
                        bool isRequest = false;
                        if (Name.Substring(Name.Length - 2) == "Rq")
                            isRequest = true;
                        foreach (XmlNode node1 in node.SelectNodes("xs:complexContent/xs:extension/xs:all/xs:element", nsManager))
                        {
                            if (Name.Substring(Name.Length - 2) == "Rp")
                            {
                                int yy = 0;
                            }
                            var NameMember = node1.Attributes["name"].Value;
                            _ApiExecutor.ItemCommand.Parameter lastInpParameter = null;
                            _ApiExecutor.ItemCommand.OutputItem lastOutParameter = null;
                            if (isRequest)
                            {
                                lastInpParameter = new _ApiExecutor.ItemCommand.Parameter()
                                {
                                    name = NameMember
                                };
                                lastCommand.parameters.Add(lastInpParameter);
                            }
                            else
                            {
                                lastOutParameter = new _ApiExecutor.ItemCommand.OutputItem() { path = NameMember };
                                lastCommand.outputItems.Add(lastOutParameter);
                            }
                            if (node1.Attributes.GetNamedItem("minOccurs") != null)
                            {
                                var minOccurs = node1.Attributes["minOccurs"].Value;
                                if (isRequest && int.Parse(minOccurs) > 0)
                                    lastInpParameter.isDemand = true;

                            }
                            if (node1.Attributes.GetNamedItem("type") != null)
                            {

                                var TypeMember = node1.Attributes["type"].Value;
                                var ttypes = TypeMember.Split(':');
                                if (ttypes[0] == "st")
                                {
                                    foreach (XmlNode node3 in node1.SelectNodes($"//xs:simpleType[@name='{ttypes[1]}']/xs:restriction/xs:enumeration", nsManager))
                                    {
                                        var val = node3.Attributes["value"].Value;
                                        if (isRequest)
                                            lastInpParameter.alternatives.Add(val);

                                    }
                                }
                            }
                            else
                            {
                                foreach (XmlNode node3 in node1.SelectNodes("xs:simpleType/xs:restriction/xs:enumeration", nsManager))
                                {
                                    var val = node3.Attributes["value"].Value;
                                    if (isRequest)
                                        lastInpParameter.alternatives.Add(val);
                                }
                                int yy = 0;
                            }
                            foreach (XmlNode node2 in node1.SelectNodes("xs:complexType/xs:sequence/xs:element[@name='Row']/xs:complexType/xs:all/xs:element", nsManager))
                            {
                                var NameMember1 = node2.Attributes["name"].Value;
                                if (!isRequest)
                                {
                                    _ApiExecutor.ItemCommand.OutputItem lastChildItem = new _ApiExecutor.ItemCommand.OutputItem() { parent = lastOutParameter, path = lastOutParameter.path + "/Row/" + NameMember1 };
                                    lastOutParameter?.children.Add(lastChildItem);
                                    lastCommand.outputItems.Add(lastChildItem);
                                }
                                else
                                {
                                    _ApiExecutor.ItemCommand.Parameter lastChildItem = new _ApiExecutor.ItemCommand.Parameter() { /*parent = lastOutParameter,*/ name = lastInpParameter.name + "/Row/" + NameMember1 };
                                }



                                if (node2.Attributes.GetNamedItem("minOccurs") != null)
                                {
                                    var minOccurs = node2.Attributes["minOccurs"].Value;
                                }
                                if (node2.Attributes.GetNamedItem("type") != null)
                                {

                                    var TypeMember = node2.Attributes["type"].Value;
                                    var ttypes = TypeMember.Split(':');
                                    if (ttypes[0] == "st")
                                    {
                                        foreach (XmlNode node3 in node1.SelectNodes($"//xs:simpleType[@name='{ttypes[1]}']/xs:restriction/xs:enumeration", nsManager))
                                        {
                                            var val = node3.Attributes["value"].Value;
                                            //                                        lastChildItem.
                                        }
                                    }
                                }
                                int rr = 0;
                            }
                        }
                    }
                }
            }
            return retValue;
        }

        private static Item NewMethod1(XmlSchemaElement value,string Name,List<string> stack,StreamWriter sw)
        {
            if (Name == "Payee")
            {
                int yy = 0;
            }
            if (Name == "Owner")
            {
                int yy = 0;
            }
            if (stack.Contains(Name))
            {
                // Console.WriteLine($"cicle detected!!!!{string.Join(" / ", stack)}");
                return null;
            }
            stack.Add(Name);

            Item retValue = new Item() { Name = Name };
            var add_attrs = ((value.ElementSchemaType as XmlSchemaComplexType)?.ContentModel?.Content as XmlSchemaComplexContentExtension)?.Attributes;
            var attrs = (value.ElementSchemaType as XmlSchemaComplexType).Attributes;
            AppendAttrs(stack, retValue, attrs,sw);
            if((value.ElementSchemaType as XmlSchemaComplexType).AttributeUses!= null)
            foreach(var att4 in  (value.ElementSchemaType as XmlSchemaComplexType).AttributeUses.Values)
                    if(att4 is XmlSchemaAttribute && !attrs.Contains(att4 as XmlSchemaAttribute))
                    AppendAttr(stack, retValue, sw, (att4 as XmlSchemaAttribute));

            /*if (add_attrs != null) 
            AppendAttrs(stack, retValue, add_attrs,sw);
            */


            var par = (value.ElementSchemaType as XmlSchemaComplexType).Particle;
            XmlSchemaObjectCollection childs = null;
            if (par is XmlSchemaChoice)
                childs = (par as XmlSchemaChoice).Items;
            if (par is XmlSchemaAll)
                childs = (par as XmlSchemaAll).Items;
            if (par is XmlSchemaSequence)
                childs = (par as XmlSchemaSequence).Items;
            if (((value.ElementSchemaType as XmlSchemaComplexType).ContentTypeParticle as XmlSchemaSequence) != null)
                childs = ((value.ElementSchemaType as XmlSchemaComplexType).ContentTypeParticle as XmlSchemaSequence).Items;
            if (childs != null)
            {
                foreach (var item in childs)
                {
                    if ((item as XmlSchemaElement)?.ElementSchemaType is XmlSchemaComplexType)
                    {

                        var el5 = NewMethod1(item as XmlSchemaElement, (item as XmlSchemaElement).Name, stack,sw);
                        if (el5 != null)
                        {
                            retValue.childs.Add(el5);
                            stack.RemoveAt(stack.Count - 1);
                        }
                    }
                    else
                    {
                        var simple = ((item as XmlSchemaElement)?.ElementSchemaType as XmlSchemaSimpleType);
                        //retValue.childs.Add(new Item() { Name = simple.Name, TType = (attr1.AttributeType as XmlSchemaType)?.Name });
                    }

                }
            }
            return retValue;
        }

        private static void AppendAttrs(List<string> stack, Item retValue, XmlSchemaObjectCollection attrs, StreamWriter sw)
        {
            foreach (var attr in attrs)
            {
                AppendAttr(stack, retValue, sw, attr);
            }
        }

        private static void AppendAttr(List<string> stack, Item retValue, StreamWriter sw, XmlSchemaObject attr)
        {
            if (attr is XmlSchemaAttribute)
            {
                var attr1 = attr as XmlSchemaAttribute;
                if (attr1.Name == "Kind" /*&& retValue.Name == "Token"*/)
                {
                    int yy = 0;
                }
                if ((attr1.AttributeType as XmlSchemaType)?.Name == "PayerType")
                {
                    int yy = 0;
                }
                if ((attr1.AttributeType as XmlSchemaType)?.Annotation != null)
                {
                    var lastChild = new Item() { Name = attr1.Name, TType = "Str" };
                    retValue.childs.Add(lastChild);
                    foreach (var it in (attr1.AttributeType as XmlSchemaType)?.Annotation.Items)
                    {
                        if (it is System.Xml.Schema.XmlSchemaAppInfo)
                        {
                            foreach (var it1 in ((System.Xml.Schema.XmlSchemaAppInfo)it).Markup)
                            {
                                if (it1.LocalName == "enumItems")
                                {
                                    foreach (XmlNode node in it1.ChildNodes)
                                    {
                                        var val = new Item.ValueItem() { value = node.Attributes["Val"].Value };
                                        lastChild.PossibleValues.Add(node.Attributes["Name"].Value, val);
                                        foreach (XmlNode node1 in node.ChildNodes)
                                        {
                                            val.descriptions.Add(node1.Attributes["language"].Value, node1.InnerText);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //                        retValue.childs.Add(new Item() { Name = attr1.Name, TType = "Str" });
                }
                else
                    if (attr1.AttributeType is XmlSchemaSimpleType)
                    retValue.childs.Add(new Item() { Name = attr1.Name, TType = (attr1.AttributeType as XmlSchemaSimpleType)?.TypeCode.ToString() });
                else
                    retValue.childs.Add(new Item() { Name = attr1.Name, TType = (attr1.AttributeType as XmlSchemaType)?.Name });
                if (retValue.childs.Last().PossibleValues.Count > 0)
                {
                    sw.WriteLine($"{string.Join("_", stack)}_{attr1.Name} Type:{retValue.childs.Last().TType} Enum:{string.Join(",", retValue.childs.Last().PossibleValues.Select(i3 => i3.Key + ":" + i3.Value))}");
                }
                else
                    sw.WriteLine($"{string.Join("_", stack)}_{attr1.Name} Type:{retValue.childs.Last().TType}");

            }
        }

        private static Item NewMethod(XmlNode node,string Name)
        {
            var retValue =new Item() { Name =Name  };
            foreach (XmlNode node1 in node.ChildNodes)
            {
                if (node1.LocalName == "sequence" || node1.LocalName == "all")
                {
                    foreach (XmlNode node2 in node1.ChildNodes)
                    {
                        if (node2.LocalName == "element")
                        {
                            if (node2.Attributes.GetNamedItem("type") != null)
                            {
                                retValue.TType = node2.Attributes["type"].Value;

                            }
                            else
                            {
                                if(node2.HasChildNodes)
                               retValue.childs.Add( NewMethod(node2.FirstChild, node2.Attributes["name"].Value));
                                else
                                {
                                    int yy = 0;
                                }
                            }
                        }
                    }
                }
            }
            return retValue;
        }
    }
}
