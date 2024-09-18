// See https://aka.ms/new-console-template for more information
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;
using ConsoleConvertToXML;
using ParserLibrary;
using static ParserLibrary.OpenApiDef;
/*using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using ParserLibrary;
using RabbitMQ.Client;
using static ParserLibrary.OpenApiDef;*/
XmlNode GetNodeForName(XmlNode node,string name)
{
    if(node.Name == name)
        return node;
    foreach (XmlNode childNode in node.ChildNodes)
    {
        if(childNode.Name == name)
            return childNode;
        var nod1= GetNodeForName(childNode, name);
        if(nod1 != null) 
            return nod1;
    }
    return null;
}
Console.WriteLine("Hello, World!");
var descrAttrName = "dEscription";
var nname = "ASTEP";
string path = $"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.txt";
SenderPartDefinition definitions = new SenderPartDefinition();
XmlDocument doc = new XmlDocument();
using (StreamReader sr = new StreamReader(path))
{
    while (!sr.EndOfStream)
    {
        string line = sr.ReadLine();
        if(!string.IsNullOrEmpty(line))
        if (line.Substring(0, 1) != "•")
        {
            var arrs = line.Split("\t");
            Console.WriteLine("Line:" + string.Join("=", arrs));
            //Node
            var nodes = arrs[0].Split("/");
            nodes=nodes.Select(x => x.Trim()).Where(ii=>ii.Length>0).ToArray();
           /* for (int i = 0; i < nodes.Length; i++)
                nodes[i] = nodes[i].Trim();*/
            if (doc.DocumentElement == null)
            {
                if (nodes.Length != 1)
                    throw new Exception($"Wrong root element {arrs[0]}");
                if(nname.Substring(0,1)=="A")
                    doc.LoadXml($"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:s='http://www.w3.org/2001/XMLSchema' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>\r\n  <SOAP-ENV:Header>\r\n    <Security xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n      <UsernameToken>\r\n        <Username>I_Bank2</Username>\r\n        <Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">IBank456</Password>\r\n      </UsernameToken>\r\n    </Security>\r\n  </SOAP-ENV:Header>\r\n  <SOAP-ENV:Body>\r\n    <{nname.Substring(1)}Response xmlns=\"http://finstream.ru/csp/awl\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n<{nname.Substring(1)}Result/>\r\n    </{nname.Substring(1)}Response>\r\n  </SOAP-ENV:Body>\r\n</SOAP-ENV:Envelope>");
                else
                    doc.LoadXml($"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:s='http://www.w3.org/2001/XMLSchema' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>\r\n  <SOAP-ENV:Header>\r\n    <Security xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n      <UsernameToken>\r\n        <Username>I_Bank2</Username>\r\n        <Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">IBank456</Password>\r\n      </UsernameToken>\r\n    </Security>\r\n  </SOAP-ENV:Header>\r\n  <SOAP-ENV:Body>\r\n    <{nodes[0].Substring(1)} xmlns=\"http://finstream.ru/csp/awl\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n<{nodes[0]}/>\r\n    </{nodes[0].Substring(1)}>\r\n  </SOAP-ENV:Body>\r\n</SOAP-ENV:Envelope>");
            }
            else
            {
                XmlNode node = doc.DocumentElement;
                if (nodes.Length > 1)
                {
                    for (int i = 0; i < nodes.Length - 1; i++)
                    {
                        if (nodes[i] == "Operator" && nodes.Length==3/*"Localization"*/)
                        {
                            int yy = 0;
                        }
                        var nodePrev = node;
                        node = GetNodeForName(node, nodes[i]);
                        if(node==  null)
                        {
                            node = doc.CreateNode(XmlNodeType.Element, nodes[i], nodePrev.NamespaceURI);

                            nodePrev.AppendChild(node);

                        }
                    }
                    if (nodes.Last().Substring(0, 1) == "@")
                    {
                        //Attribute
                        var attr = doc.CreateAttribute(nodes.Last().Substring(1));
                        node.Attributes.Append(attr);
                    }
                    else
                    {
                        var nodeNew = doc.CreateNode(XmlNodeType.Element, nodes.Last(), node.NamespaceURI);

                        node.AppendChild(nodeNew);
                    }
                }
                else
                    node = GetNodeForName(node, nodes[0]);
                List<string> keywords =new List<string> { "required" , "Required", "Обязательный" };
                if (arrs.Length > 3)
                {
                        if (arrs[0].Contains("@Direction"))
                        {
                    //        •
                        }
                    definitions.definitionItems.Add(new SenderPartDefinition.DefinitionItem() { path = arrs[0].Trim().Replace(" ", ""), description = arrs[3] + ((arrs[3]== "•") ? (arrs[4]+"\n"):""), repeatable = arrs[2] == "*", required = keywords.Contains(arrs[1]) });


                    if (arrs.Length > 4)
                        definitions.definitionItems.Last().type = arrs[4];
                }
                else
                    definitions.definitionItems.Add(new SenderPartDefinition.DefinitionItem() { path = arrs[0].Trim().Replace(" ", ""), description = "" });
                /*
                {
                    var attr = doc.CreateAttribute(descrAttrName);
                    attr.Value = arrs[3];
                    node.Attributes.Append(attr);
                }*/


            }
        }
        else
        {
            var arrs = line.Split("\t");
                if(definitions.definitionItems.Last().path.Contains("@Direction"))
                    {
                    int yy = 0;
                }
            definitions.definitionItems.Last().description+=arrs[0];
            if (arrs.Length > 1)
                    definitions.definitionItems.Last().description +=arrs[1]+ "\n";

                if (arrs.Length > 2)
                definitions.definitionItems.Last().type = arrs[2];
            // definitions[key] += line;
        }
    }
}
doc.Save($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.xml");
List<OpenApiDef.EntryPoint.ExternalItem> allProviders = null;
using (StreamReader sr = new StreamReader("C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\AllProviders.json"))
{
    allProviders = JsonSerializer.Deserialize<List<OpenApiDef.EntryPoint.ExternalItem>>(sr.ReadToEnd());
}
/*if(nname.Substring(0,1)== "R")
    MyExtensions.compare2XML($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.xml",allProviders.First(ii=>ii.Name==nname.Substring(1)).examplePathInput );
else
    MyExtensions.compare2XML($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.xml", allProviders.First(ii => ii.Name == nname.Substring(1)).examplePathOutput);
*/
JsonSerializerOptions options = new JsonSerializerOptions()
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
,
    IgnoreNullValues = true
};

using (StreamWriter sw = new StreamWriter($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.json"))
    sw.Write(System.Text.Json.JsonSerializer.Serialize(definitions,options));
Console.WriteLine(doc.OuterXml);

