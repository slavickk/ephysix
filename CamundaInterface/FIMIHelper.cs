using Namotion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace CamundaInterface
{
    public class FIMIHelper
    {
        public class ItemCommand
        {
            public string Name;
            public class Parameter
            {
                public string name;
                public bool isDemand = false;
                public List<string> alternatives = new List<string>();
            }
            public List<Parameter> parameters = new List<Parameter>();
            public class OutputItems
            {
                public string path;
                public OutputItems parent = null;

            }
            public List<OutputItems> outputItems = new List<OutputItems>();
        }
        public static List<ItemCommand> getDefine()
        {
            Dictionary<string, string> namespaces;
            XmlDocument xmlDoc;
            XmlNamespaceManager nsManager;

            List<ItemCommand> retValue = new List<ItemCommand>();
            string filePath = "Data/fimi_types.xsd";
            xmlDoc = new XmlDocument();
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

            /*            namespaces.Add("m0", "http://schemas.compassplus.com/two/1.0/fimi_types.xsd");
                        namespaces.Add("m1", "http://schemas.compassplus.com/two/1.0/fimi.xsd");
                        namespaces.Add("m", "http://schemas.compassplus.com/two/1.0/fimi.wsdl");*/
            nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            foreach (var item in namespaces)
                nsManager.AddNamespace(item.Key, item.Value);
            foreach (XmlNode node in xmlDoc.SelectNodes("//xs:complexType", nsManager))
            {
                if (node.Attributes.GetNamedItem("name") != null)
                {
                    var Name = node.Attributes["name"].Value;
                    if (Name.Substring(0, Name.Length - 2) == "AcctDebit")
                    {
                        int yy = 0;
                    }
                    if (Name.Substring(Name.Length - 2) == "Rq" || Name.Substring(Name.Length - 2) == "Rp")
                    {
                        ItemCommand lastCommand = null;
                        lastCommand = retValue.FirstOrDefault(x => x.Name == Name.Substring(0, Name.Length - 2));
                        if (lastCommand == null)
                        {
                            lastCommand = new ItemCommand() { Name = Name.Substring(0, Name.Length - 2) };
                            retValue.Add(lastCommand);

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
                            ItemCommand.Parameter lastInpParameter = null;
                            ItemCommand.OutputItems lastOutParameter = null;
                            if (isRequest)
                            {
                                lastInpParameter = new ItemCommand.Parameter()
                                {
                                    name = NameMember
                                };
                                lastCommand.parameters.Add(lastInpParameter);
                            }
                            else
                            {
                                lastOutParameter = new ItemCommand.OutputItems() { path = NameMember };
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
                                    ItemCommand.OutputItems lastChildItem = new ItemCommand.OutputItems() { parent = lastOutParameter, path = lastOutParameter.path + "/Row/" + NameMember1 };
                                    lastCommand.outputItems.Add(lastChildItem);
                                }
                                else
                                {
                                    ItemCommand.Parameter lastChildItem = new ItemCommand.Parameter() { /*parent = lastOutParameter,*/ name = lastInpParameter.name + "/Row/" + NameMember1 };
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
    }



}
