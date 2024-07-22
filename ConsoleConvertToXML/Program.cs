// See https://aka.ms/new-console-template for more information
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;
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
var nname = "APUPAY";
string path = $"C:\\БГС\\{nname}.txt";
Dictionary<string,string> definitions = new Dictionary<string, string>();
XmlDocument doc = new XmlDocument();
using (StreamReader sr = new StreamReader(path))
{
    while (!sr.EndOfStream)
    {
        string line = sr.ReadLine();
        if (line.Substring(0, 1) != "•")
        {
            var arrs = line.Split("\t");
            Console.WriteLine("Line:" + string.Join("=", arrs));
            //Node
            var nodes = arrs[0].Split("/");
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = nodes[i].Trim();
            if (doc.DocumentElement == null)
            {
                if (nodes.Length != 1)
                    throw new Exception($"Wrong root element {arrs[0]}");
                doc.LoadXml($"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:s='http://www.w3.org/2001/XMLSchema' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>\r\n  <SOAP-ENV:Header>\r\n    <Security xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\r\n      <UsernameToken>\r\n        <Username>I_Bank2</Username>\r\n        <Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">IBank456</Password>\r\n      </UsernameToken>\r\n    </Security>\r\n  </SOAP-ENV:Header>\r\n  <SOAP-ENV:Body>\r\n    <{nodes[0].Substring(1)} xmlns=\"http://finstream.ru/csp/awl\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n<{nodes[0]}/>\r\n    </{nodes[0].Substring(1)}>\r\n  </SOAP-ENV:Body>\r\n</SOAP-ENV:Envelope>");
            }
            else
            {
                XmlNode node = doc.DocumentElement;
                if (nodes.Length > 1)
                {
                    for (int i = 0; i < nodes.Length - 1; i++)
                    {
                        if (nodes[i] == "AWRequest")
                        {
                            int yy = 0;
                        }
                        node = GetNodeForName(node, nodes[i]);
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
                if(arrs.Length > 3)
                definitions.Add(arrs[0].Trim().Replace(" ",""), arrs[3]);
                else
                    definitions.Add(arrs[0].Trim().Replace(" ", ""), "");
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
            var key = definitions.Last().Key;
            definitions[key] += line;
        }
    }
}
doc.Save($"C:\\БГС\\{nname}.xml");
JsonSerializerOptions options = new JsonSerializerOptions()
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
,
    IgnoreNullValues = true
};

using (StreamWriter sw = new StreamWriter($"C:\\БГС\\{nname}.json"))
    sw.Write(JsonSerializer.Serialize(definitions,options));
Console.WriteLine(doc.OuterXml);

