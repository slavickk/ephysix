/******************************************************************
 * File: RTPImplementation.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.ConstrainedExecution;

namespace CamundaInterface
{
        public class RTPXmlTransport:_ApiExecutor
        {
            string addr;
            string password = "qwerty";
            string session;
        public string NextChallenge;
        public RTPXmlTransport()
        {

        }
        public RTPXmlTransport(string addr = @"http://10.74.28.40:25404", string password = "qwerty")
            {
                this.addr = addr;
                this.password = password;
            }
            static HttpClient client = new HttpClient();



            public async Task<RTPFimi> send(RTPFimi fimi, string currentTopic = "")
            {
            /*    if (!string.IsNullOrEmpty(currentTopic) && !string.IsNullOrEmpty(NextChallenge) && !string.IsNullOrEmpty(session))
                {
                    var pwd = RTPFimi.GetChallengePassword(NextChallenge, password);

                    fimi.setPath($"FIMI/{currentTopic}Rq/Rq/@Password", pwd);
                    fimi.setPath($"FIMI/{currentTopic}Rq/Rq/@Session", session);

                }*/
                string request1 = fimi.outerXml;
                StringContent httpContent = new StringContent(request1, System.Text.Encoding.UTF8, "application/xml");

                var ans = await client.PostAsync(addr, httpContent);
                //.PostAsJsonAsync($"{camundaPath}external-task/fetchAndLock", new ItemFetchAndLock() { maxTasks = 1, usePriority = true, workerId = workerId, topics = topics.Select(ii => new ItemFetchAndLock.Topic() { lockDuration = 100000, topicName = ii }).ToList() });
                if (ans.IsSuccessStatusCode)
                {
                    var ret = new RTPFimi(await ans.Content.ReadAsStringAsync());
                    var nod = ret.xmlDoc.SelectSingleNode("//@Result");
                    if (nod != null)
                    {
                        var res = nod.InnerText;
                        if(res != "Approved")
                    {
                        lastError = new _ApiExecutor.ErrorItem() { content = request1, error = ret.xmlDoc.SelectSingleNode("//@DeclineReason").InnerText };
                        return null;    
                    }
                }
                    /*if (currentTopic == "InitSession")
                    {
                        session = ret.getPath("FIMI/InitSessionRp/Rp/Id");
                    }
                    if (currentTopic == "Logoff")
                    {
                        session = NextChallenge = "";
                    }*/
                    return ret;

                }
                else
                {
                    //var errorContent = await ans.Content.ReadAsStringAsync();
                    XmlSerializer ser = new XmlSerializer(typeof(Envelope));
                    lastError = new _ApiExecutor.ErrorItem() { content = request1, error = ((Envelope)ser.Deserialize(ans.Content.ReadAsStream())).Body.Fault.Reason.Text };

                    return null;
                }

            }

       
        public async  Task beginSessionAsync()
        {
          /*  var currentKey = "InitSession";
            RTPFimi fimi = new RTPFimi();
            fimi.setPath("FIMI/InitSessionRq/Rq/NeedDicts", "0");
            fimi.setPath("FIMI/InitSessionRq/Rq/AllVendors", "0");
            fimi.setPath("FIMI/InitSessionRq/Rq/AvoidSession", "0");
            var ans = await send(fimi, currentKey);
            //                var pwd =XmlFimi.GetChallengePassword(tr.NextChallenge);
            RTPFimi fimiLogon = new RTPFimi();
            var ans11 = await send(fimiLogon, "Logon");*/
        }

        public async Task<_ApiFilter> ExecAsync(ExecContextItem[] commands)
        {
/*            string content;
            using (StreamReader sr = new StreamReader(@"c:\d\Answer.xml"))
            {
                content = sr.ReadToEnd();
            }*/
            //            return new XmlFimi(content);
            RTPFimi retValue = null;
            foreach (var com in commands)
            {
                RTPFimi rtpCommand = new RTPFimi(com.CommandItem.environment);

                var currentKey = com.CommandItem;
                /*    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Password", pwd);
                    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Session", ans.getPath("FIMI/InitSessionRp/Rp/Id"));*/
                foreach(var par1 in com.CommandItem.parameters)
//                foreach (var par in com.Params)
                {
                    /*                    if(par.Value.GetType() == typeof(JsonDocument))
                                        {
                                            JsonDocument jsonDocument = (JsonDocument)par.Value;
                                            if(jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
                                            {
                                                var users = jsonDocument.RootElement.EnumerateArray();

                                                while (users.MoveNext())
                                                {
                                                    var user = users.Current;
                                                    var props = user.EnumerateObject();
                                                    rtpCommand.setPath($"FIMI/{currentKey}Rq/Rq/{par.Key}/Row", null);
                                                    while (props.MoveNext())
                                                    {
                                                        var prop = props.Current;
                                                        if(prop.Value.ValueKind != JsonValueKind.Null)
                                                        rtpCommand.setPath($"FIMI/{currentKey}Rq/Rq/{par.Key}/Row/{prop.Name}",prop.Value.ToString());
                                                    }
                                                }
                                            } 
                    //                        if(jsonDocument.RootElement. != null)
                    //                        fimiCommand.setPath($"FIMI/{currentKey}Rq/Rq/{par.Key}", par.Value.ToString());

                                        }
                                        else*/
                    var par=com.Params.FirstOrDefault(ii => ii.Key == par1.name);
                    if(par != null)
                        rtpCommand.setPath(par.FullAddr, par.Value.ToString());
                    else
                        rtpCommand.setPath(par1.fullPath, string.Empty);

                }
                retValue = await send(rtpCommand, currentKey.Name);
            }
            return retValue;
        }

        public async Task endSessionAsync()
        {
            /*RTPFimi fimiLogoff = new RTPFimi();
            var ans11 = await send(fimiLogoff, "Logoff");*/
        }

        /*public EnvelopeBodyFault getError()
        {
            return lastError;
        }*/

        void analiseXmlEl(XmlNode el, List<_ApiExecutor.ItemCommand.Parameter> parametres)
        {
            if(!el.HasChildNodes && el.InnerText=="?")
            {
                var name = "";
                name = FormName(el, name);
                parametres.Add(new _ApiExecutor.ItemCommand.Parameter() {name=el.ParentNode.Name, fullPath = name });
            }
            foreach (XmlNode node in el.ChildNodes)
                analiseXmlEl(node, parametres);
            
            if(el.Attributes != null)
            foreach(XmlAttribute attr in el.Attributes)
            {
                if(attr.Value =="?")
                {
                    var name ="/@"+attr.Name;
                    name=FormName(el, name);
                    parametres.Add(new _ApiExecutor.ItemCommand.Parameter() {name=attr.Name, fullPath = name });
                }
            }

        }

        public static string FormName(XmlNode el, string name)
        {
            if (el.GetType() != typeof(XmlText) && el.GetType() != typeof(XmlDocument) && el.GetType() != typeof(XmlAttribute) && string.IsNullOrEmpty(el.Prefix))
                el.Prefix = "default";

                var nameEl = el.Name;
            if (el.GetType() == typeof(XmlAttribute))
                nameEl = "@" + nameEl;
            if (el.GetType() != typeof(XmlText) && el.GetType() !=typeof(XmlDocument))                
                name = ((el.ParentNode.ParentNode!= null)?"/":"") + nameEl + name;
            if (el.ParentNode != null)
                name = FormName(el.ParentNode, name) ;

            return name;
        }

        public  List<_ApiExecutor.ItemCommand> getDefine()
        {
            Dictionary<string, string> namespaces;
            XmlDocument xmlDoc;
            XmlNamespaceManager nsManager;

            List<_ApiExecutor.ItemCommand> retValue = new List<_ApiExecutor.ItemCommand>();
//            string pattern = "Data/RPT/*.xml";
            foreach (var filePath in Directory.GetFiles("Data/RTP", "*.xml"))
            {
                var lastCommand = new _ApiExecutor.ItemCommand() { Name =Path.GetFileNameWithoutExtension(filePath) };
                retValue.Add(lastCommand);
                xmlDoc = new XmlDocument();
                string xmlContent;
                using (StreamReader sr= new StreamReader(filePath))
                {
                    xmlContent = sr.ReadToEnd();
                }
                xmlDoc.LoadXml(RTPFimi.TransformXml(xmlContent));
                var nod=xmlDoc.DocumentElement.SelectSingleNode("//*[@outpath]");
                if( nod != null)
                {
                    lastCommand.outputPath = nod.Attributes["outpath"].Value;
                    nod.Attributes.Remove(nod as XmlAttribute);
                }

                lastCommand.environment = xmlDoc.OuterXml;
                analiseXmlEl( xmlDoc.DocumentElement, lastCommand.parameters );

            }
            return retValue;
        }

 
    _ApiExecutor.ErrorItem _ApiExecutor.getError()
        {
            return lastError;
//            throw new NotImplementedException();
        }

        public _ApiExecutor.ErrorItem lastError;
        }

        public class RTPFimi : _ApiFilter
    {
            public XmlDocument xmlDoc;
            public XmlNamespaceManager nsManager;
            Dictionary<string, string> namespaces;
            public string extract(string path, string currentTopic)
            {
                var patt = string.Join("/", new string[] { "env:Envelope", "env:Body", "m1:" + currentTopic + "Rp", "Response" }.Union(path.Split('/').Select(ii => "m0:" + ii)));
                var nod = xmlDoc.SelectSingleNode(patt, nsManager);
                return nod.InnerText;

            }
            public List<string> extractMulti(string path)
            {
                List<string> retValue = new List<string>();
//                if (path.Substring(0, 1) != "$")
                {
//                    var patt = string.Join("/", new string[] { "env:Envelope", "env:Body", "*", "Response" }.Union(path.Split('/').Select(ii => "m0:" + ii)));
                    foreach (XmlNode node in xmlDoc.SelectNodes(path, nsManager))
                        retValue.Add(node.InnerText);
                }
                return retValue;

            }
        public List<(string name,string value)> extractMultiWithNames(string path)
        {
            List<(string,string)> retValue = new List<(string, string)>();
            //                if (path.Substring(0, 1) != "$")
            {
                //                    var patt = string.Join("/", new string[] { "env:Envelope", "env:Body", "*", "Response" }.Union(path.Split('/').Select(ii => "m0:" + ii)));
                foreach (XmlNode node in xmlDoc.SelectNodes(path, nsManager))
                {
                    var name = "";
//                    RTPXmlTransport.FormName(node, name);
                    retValue.Add((RTPXmlTransport.FormName(node, name), node.InnerText));
                }
            }
            return retValue;

        }

        static byte[] hashPWD(string password, string text)
            {
                var keyArray = UTF8Encoding.UTF8.GetBytes(password);
                return Encrypt3Des(text, keyArray);
            }


            private static byte[] EncryptDes(string text, byte[] keyArray)
            {
                var toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);
                DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();
                //Return the encrypted data into unreadable string format

                return resultArray;
                //            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            private static byte[] Encrypt3Des(string text, byte[] keyArray)
            {
                var toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();
                //Return the encrypted data into unreadable string format
                string hex = string.Join(" ", resultArray.Select(x => x.ToString("X2")));
                return resultArray;
                //            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            public static string GetChallengePassword(string NextChallenge, string password = "qwerty")
            {
                var enc = hashPWD(password.PadRight(16, ' ').ToUpper(), password.PadRight(8, ' ').ToUpper());
                var enc1 = EncryptDes(NextChallenge, enc.Take(8).ToArray());
                return string.Join("", enc1.Take(8).Select(x => x.ToString("X2")));
            }
            public RTPFimi()
            {
                string xmlContent = @"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/' xmlns:m1='http://schemas.compassplus.com/two/1.0/fimi.xsd' xmlns:m0='http://schemas.compassplus.com/two/1.0/fimi_types.xsd'>
  <env:Body>
    <m1:InitSessionRq>
      <m1:Request Ver='9.1' Product='FIMI' RetAddress='192.166.22.33' Clerk='FIMIXML'>
      </m1:Request>
    </m1:InitSessionRq>
  </env:Body>
</env:Envelope>";
                Init(xmlContent);
            }
            public RTPFimi(string xmlContent)
            {
                Init(xmlContent);
            }
        public static string TransformXml(string xmlContent)
        {
            return xmlContent;
            xmlContent = xmlContent.Replace("<Tran xmlns=", "<a1:Tran xmlns:a1=");
            xmlContent = xmlContent.Replace("<Tran>", "<a1:Tran xmlns:a1=\"http://schemas.tranzaxis.com/tran.wsdl\">");
            //xmlns="http://schemas.tranzaxis.com/tran.wsdl"
            xmlContent = xmlContent.Replace("</Tran", "</a1:Tran");
            return xmlContent;
        }
        private void Init(string xmlContent)
        {
            xmlContent = TransformXml(xmlContent);
//            xmlContent = xmlContent.Replace("</Tran", "</a1:Tran");
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            namespaces = new Dictionary<string, string>();


            //            namespaces.Add("env", "http://www.w3.org/2003/05/soap-envelope");
            /*                namespaces.Add("env", "http://schemas.xmlsoap.org/soap/envelope/");



                            namespaces.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
                            namespaces.Add("m1", "http://schemas.compassplus.com/two/1.0/fimi.xsd");*/

            var el = xmlDoc.DocumentElement;
            AddNameSpaces(el);

            /*            namespaces.Add("m0", "http://schemas.compassplus.com/two/1.0/fimi_types.xsd");
                        namespaces.Add("m1", "http://schemas.compassplus.com/two/1.0/fimi.xsd");
                        namespaces.Add("m", "http://schemas.compassplus.com/two/1.0/fimi.wsdl");*/
            nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            foreach (var item in namespaces)
//                if (!string.IsNullOrEmpty(item.Key))
                    nsManager.AddNamespace(item.Key, item.Value);
  /*              else
                    nsManager.AddNamespace("default", item.Value);*/


            /*else
                xmlDoc..NamespaceURI = item.Value;*/
        }

        private void AddNameSpaces(XmlElement? el)
        {
            if (el?.Attributes?.Count > 0)
            {
                foreach (XmlAttribute attr in el.Attributes)
                {
                    int index = attr.Name.IndexOf("xmlns");
                    if (index >= 0)
                    {
                        string attrName = "default";
                        if(attr.Name.Contains("xmlns:"))
                            attrName = attr.Name.Substring(index+6);

                        if (!namespaces.ContainsKey(attrName))
                            namespaces.Add(attrName, attr.Value);
                    }
                }
            }
            foreach (var el1 in el.ChildNodes)
                if(el1 is XmlElement)
                    AddNameSpaces(el1 as XmlElement);
        }

      /*  public static string XmlPath(string path)
            {
                var lex = path.Replace("FIMI/", "").Replace("/Rq/", "/m1:Request/").Replace("/Rp/", "/Response/").Split("/").ToList();
                lex[0] = "m1:" + lex[0];
                int index = lex.IndexOf("m1:Request");
                if(index== -1)
                    index = lex.IndexOf("Response");
            for (int i=index+1;i <lex.Count; i++)
                if (lex[i][0] != '@')
                    lex[i] = "m0:" + lex[i];
                return string.Join("/", new string[] { "env:Envelope", "env:Body" }.Union(lex));
            }
            public string getPath(string path)
            {
                return xmlDoc.SelectSingleNode(XmlPath(path), nsManager).InnerText;
            }*/
            public string outerXml
            {
                get
                {
                    return xmlDoc.OuterXml;
                }
            }
            public void saveToStream(Stream stream)
            {
                xmlDoc.Save(stream);
            }

            public void setPath(string path, string? Value)
            {
            var comm_obj1 = xmlDoc.SelectSingleNode(path, nsManager);
            if(comm_obj1 is XmlAttribute)
            {
                if (string.IsNullOrEmpty(Value))
                    comm_obj1.ParentNode.Attributes.Remove((comm_obj1 as XmlAttribute));
                else
                     (comm_obj1 as XmlAttribute).Value= Value;
            } else
            {
                if (string.IsNullOrEmpty(Value))
                    comm_obj1.ParentNode.RemoveChild(comm_obj1 );
                else
                    comm_obj1.InnerText = Value;

            }

/*            var tokens = XmlPath(path).Split("/");
            int lastIndex = tokens.Length - 1;
            var comm_obj = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(3)), nsManager);
                if (comm_obj == null)
                {
                    var tt = tokens[2].Split(':');
                    var nod = xmlDoc.CreateNode(XmlNodeType.Element, tt[0], tt[1], namespaces[tt[0]]);
                    var rootNode = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(2)), nsManager);
                    var child = rootNode.ChildNodes.Item(0).ChildNodes.Item(0);
                    nod.AppendChild(child);
                    rootNode.ReplaceChild(nod, rootNode.ChildNodes.Item(0));
                    lastIndex = 4;
                }
                if (tokens.Last()[0] == '@')
                {
                    var attrName = tokens.Last().Substring(1);
                    var obj = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(tokens.Length - 1)), nsManager);
                    var attr = obj.Attributes.GetNamedItem(attrName);
                    if (attr == null)
                    {
                        var attr2 = xmlDoc.CreateAttribute(attrName);
                        attr2.InnerText = Value;
                        obj.Attributes.Append(attr2);
                    }
                    else
                        attr.InnerText = Value;
                }
                else
            {
                for(int index = lastIndex; index <tokens.Length;index++)
                    SetNodeRq(Value, tokens, index);

            }
*/
        }

     /*   private void SetNodeRq(string? Value, string[] tokens, int index)
        {
            var nodes = xmlDoc.SelectNodes(string.Join('/', tokens.Take(index)), nsManager);
            var obj = nodes[nodes.Count - 1];
            var replaced = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(index + 1)), nsManager);
            if (replaced == null)
            {
                var tt = tokens[index].Split(':');
                var nod = xmlDoc.CreateNode(XmlNodeType.Element, tt[0], tt[1], namespaces[tt[0]]);
                obj.AppendChild(nod);
                if (index == tokens.Length - 1 && Value != null)
                    nod.InnerText = Value;
            }
            else
            {
                if (index == tokens.Length - 1 && Value != null)
                    replaced.InnerText = Value;
            }
        }
     */
        public string[] filter(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            return extractMulti(path).ToArray();
        }

        public (string name, string value)[] filterWithNames(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            return extractMultiWithNames(path).ToArray();
        }
    }



}
