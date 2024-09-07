using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace ConsoleConvertToXML
{
    public static class MyExtensions
    {
        private static string GetQName(XElement xe)
        {
            string prefix = xe.GetPrefixOfNamespace(xe.Name.Namespace);
            if (xe.Name.Namespace == XNamespace.None || prefix == null)
                return xe.Name.LocalName.ToString();
            else
                return prefix + ":" + xe.Name.LocalName.ToString();
        }

        private static string GetQName(XAttribute xa)
        {
            string prefix =
                xa.Parent.GetPrefixOfNamespace(xa.Name.Namespace);
            if (xa.Name.Namespace == XNamespace.None || prefix == null)
                return xa.Name.ToString();
            else
                return prefix + ":" + xa.Name.LocalName;
        }

        private static string NameWithPredicate(XElement el)
        {
            if (el.Parent != null && el.Parent.Elements(el.Name).Count() != 1)
                return GetQName(el) + "[" +
                    (el.ElementsBeforeSelf(el.Name).Count() + 1) + "]";
            else
                return GetQName(el);
        }

        public static string StrCat<T>(this IEnumerable<T> source,
            string separator)
        {
            return source.Aggregate(new StringBuilder(),
                       (sb, i) => sb
                           .Append(i.ToString())
                           .Append(separator),
                       s => s.ToString());
        }

        public static string GetXPath(this XObject xobj)
        {
            if (xobj.Parent == null)
            {
                XDocument doc = xobj as XDocument;
                if (doc != null)
                    return ".";
                XElement el = xobj as XElement;
                if (el != null)
                    return "/" + NameWithPredicate(el);
                // The XPath data model doesn't include white space text nodes
                // that are children of a document, so this method returns null.
                XText xt = xobj as XText;
                if (xt != null)
                    return null;
                XComment com = xobj as XComment;
                if (com != null)
                    return
                        "/" +
                        (
                            com
                            .Document
                            .Nodes()
                            .OfType<XComment>()
                            .Count() != 1 ?
                            "comment()[" +
                            (com
                            .NodesBeforeSelf()
                            .OfType<XComment>()
                            .Count() + 1) +
                            "]" :
                            "comment()"
                        );
                XProcessingInstruction pi = xobj as XProcessingInstruction;
                if (pi != null)
                    return
                        "/" +
                        (
                            pi.Document.Nodes()
                            .OfType<XProcessingInstruction>()
                            .Count() != 1 ?
                            "processing-instruction()[" +
                            (pi
                            .NodesBeforeSelf()
                            .OfType<XProcessingInstruction>()
                            .Count() + 1) +
                            "]" :
                            "processing-instruction()"
                        );
                return null;
            }
            else
            {
                XElement el = xobj as XElement;
                if (el != null)
                {
                    return
                        "/" +
                        el
                        .Ancestors()
                        .InDocumentOrder()
                        .Select(e => NameWithPredicate(e))
                        .StrCat("/") +
                        NameWithPredicate(el);
                }
                XAttribute at = xobj as XAttribute;
                if (at != null)
                    return
                        "/" +
                        at
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(e => NameWithPredicate(e))
                        .StrCat("/") +
                        "@" + GetQName(at);
                XComment com = xobj as XComment;
                if (com != null)
                    return
                        "/" +
                        com
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(e => NameWithPredicate(e))
                        .StrCat("/") +
                        (
                            com
                            .Parent
                            .Nodes()
                            .OfType<XComment>()
                            .Count() != 1 ?
                            "comment()[" +
                            (com
                            .NodesBeforeSelf()
                            .OfType<XComment>()
                            .Count() + 1) + "]" :
                            "comment()"
                        );
                XCData cd = xobj as XCData;
                if (cd != null)
                    return
                        "/" +
                        cd
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(e => NameWithPredicate(e))
                        .StrCat("/") +
                        (
                            cd
                            .Parent
                            .Nodes()
                            .OfType<XText>()
                            .Count() != 1 ?
                            "text()[" +
                            (cd
                            .NodesBeforeSelf()
                            .OfType<XText>()
                            .Count() + 1) + "]" :
                            "text()"
                        );
                XText tx = xobj as XText;
                if (tx != null)
                    return
                        "/" +
                        tx
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(e => NameWithPredicate(e))
                        .StrCat("/") +
                        (
                            tx
                            .Parent
                            .Nodes()
                            .OfType<XText>()
                            .Count() != 1 ?
                            "text()[" +
                            (tx
                            .NodesBeforeSelf()
                            .OfType<XText>()
                            .Count() + 1) + "]" :
                            "text()"
                        );
                XProcessingInstruction pi = xobj as XProcessingInstruction;
                if (pi != null)
                    return
                        "/" +
                        pi
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(e => NameWithPredicate(e))
                        .StrCat("/") +
                        (
                            pi
                            .Parent
                            .Nodes()
                            .OfType<XProcessingInstruction>()
                            .Count() != 1 ?
                            "processing-instruction()[" +
                            (pi
                            .NodesBeforeSelf()
                            .OfType<XProcessingInstruction>()
                            .Count() + 1) + "]" :
                            "processing-instruction()"
                        );
                return null;
            }
        }

        static Dictionary<string, string> toDictOld(string filePath)
        {
            Dictionary<string, string> retValue = new Dictionary<string, string>();
            var doc = XDocument.Load(filePath);
            foreach (XObject obj in doc.DescendantNodes())
            {
                var path = obj.GetXPath();
                retValue.Add(path, path);
                XElement el = obj as XElement;
                if (el != null)
                    foreach (XAttribute at in el.Attributes())
                    {
                        var path1 = at.GetXPath();
                        retValue.Add(path1, path1);
                    }
            }
            return retValue;
        }

        const bool isLocal = true;
        static string calcPath(XmlNode node, XmlNode parentNode)
        {
            var nodeName = (isLocal&& node.GetType() != typeof(XmlAttribute) )? node.LocalName:node.Name;
             if (node.GetType() == typeof(XmlAttribute))
                nodeName = "@" + ((XmlAttribute)node).Name;
            var retValue = (node.GetType() != typeof(XmlAttribute) && node.ParentNode.Name == "#document") ? nodeName : ("/" + nodeName);
            if (nodeName == "#comment")
                retValue = "";
            if (node.GetType() == typeof(XmlAttribute))
            {
                retValue = calcPath(parentNode, parentNode.ParentNode) + retValue;

            }
            else
            {
                if (node.ParentNode.Name != "#document")
                    retValue = calcPath(node.ParentNode, node.ParentNode.ParentNode) + retValue;
            }
            return retValue;
        }

        static List<string> getAllPaths(XmlNode node, XmlNode parentNode, List<string> list)
        {

            var path = calcPath(node, parentNode);
            if (!list.Contains(path))
                list.Add(path);
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.GetType() != typeof(XmlText))

                    getAllPaths(node2, node, list);
            }
            if (node.Attributes != null)
            {
                foreach (XmlNode node2 in node.Attributes/*.ChildNodes*/)
                {
                    getAllPaths(node2, node, list);
                }
            }
            return list;
        }
        static List<string> toDict(string filePath)
        {
            List<string> retValue = new List<string>();
            var doc = new XmlDocument();
            doc.Load(filePath);
            return getAllPaths(doc.DocumentElement, null, retValue);
        }

        public static void compare2XML(string file1,string file2)
        {
            var dict1 = toDict(file1);
            var dict2 = toDict(file2);
            foreach (var key in dict2)
            {
                if (!dict1.Contains(key) && (isLocal || !dict1.Contains(key.Substring(key.IndexOf(":")))))
                {
                    Console.WriteLine(key);
                }
            }
        }
    }
}
