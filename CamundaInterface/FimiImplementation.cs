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
using Microsoft.OpenApi.Services;

namespace CamundaInterface
{
        public class FimiXmlTransport:APIExecutor._ApiExecutor
        {
            string addr;
            string password = "qwerty";
            string session;
        public string NextChallenge;
        public FimiXmlTransport(string addr = @"http://10.74.28.30:30401", string password = "qwerty")
            {
                this.addr = addr;
                this.password = password;
            }
            static HttpClient client = new HttpClient();



            public async Task<XmlFimi> send(XmlFimi fimi, string currentTopic = "")
            {
                if (!string.IsNullOrEmpty(currentTopic) && !string.IsNullOrEmpty(NextChallenge) && !string.IsNullOrEmpty(session))
                {
                    var pwd = XmlFimi.GetChallengePassword(NextChallenge, password);

                    fimi.setPath($"FIMI/{currentTopic}Rq/Rq/@Password", pwd);
                    fimi.setPath($"FIMI/{currentTopic}Rq/Rq/@Session", session);

                }
                string request1 = fimi.outerXml;
                StringContent httpContent = new StringContent(request1, System.Text.Encoding.UTF8, "application/json");

                var ans = await client.PostAsync(addr, httpContent);
                //.PostAsJsonAsync($"{camundaPath}external-task/fetchAndLock", new ItemFetchAndLock() { maxTasks = 1, usePriority = true, workerId = workerId, topics = topics.Select(ii => new ItemFetchAndLock.Topic() { lockDuration = 100000, topicName = ii }).ToList() });
                if (ans.IsSuccessStatusCode)
                {
                    var ret = new XmlFimi(await ans.Content.ReadAsStringAsync());
                    var nod = ret.xmlDoc.SelectSingleNode("//@NextChallenge");
                    if (nod != null)
                    {
                        NextChallenge = nod.InnerText;
                    }
                    if (currentTopic == "InitSession")
                    {
                        session = ret.getPath("FIMI/InitSessionRp/Rp/Id");
                    }
                    if (currentTopic == "Logoff")
                    {
                        session = NextChallenge = "";
                    }
                    return ret;

                }
                else
                {
                    //var errorContent = await ans.Content.ReadAsStringAsync();
                    XmlSerializer ser = new XmlSerializer(typeof(Envelope));
                    lastError = ((Envelope)ser.Deserialize(ans.Content.ReadAsStream())).Body.Fault;

                    return null;
                }

            }

       
        public async  Task beginSessionAsync()
        {
            var currentKey = "InitSession";
            XmlFimi fimi = new XmlFimi();
            fimi.setPath("FIMI/InitSessionRq/Rq/NeedDicts", "0");
            fimi.setPath("FIMI/InitSessionRq/Rq/AllVendors", "0");
            fimi.setPath("FIMI/InitSessionRq/Rq/AvoidSession", "0");
            var ans = await send(fimi, currentKey);
            //                var pwd =XmlFimi.GetChallengePassword(tr.NextChallenge/* ans.getPath("FIMI/InitSessionRp/Rp/@NextChallenge")*/);
            XmlFimi fimiLogon = new XmlFimi();
            /*              fimiLogon.setPath("FIMI/LogonRq/Rq/@Password", pwd);
                          fimiLogon.setPath("FIMI/LogonRq/Rq/@Session", ans.getPath("FIMI/InitSessionRp/Rp/Id"));*/
            var ans11 = await send(fimiLogon, "Logon");
        }

        public async Task<APIExecutor._ApiFilter> ExecAsync(APIExecutor.ExecContextItem[] commands)
        {
/*            string content;
            using (StreamReader sr = new StreamReader(@"c:\d\Answer.xml"))
            {
                content = sr.ReadToEnd();
            }*/
            //            return new XmlFimi(content);
            XmlFimi retValue = null;
            foreach (var com in commands)
            {
                XmlFimi fimiCommand = new XmlFimi();

                var currentKey = com.Command;
                /*    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Password", pwd);
                    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Session", ans.getPath("FIMI/InitSessionRp/Rp/Id"));*/
                foreach (var par in com.Params)
                {
                    fimiCommand.setPath($"FIMI/{currentKey}Rq/Rq/{par.Key}", par.Value);
                }
                retValue = await send(fimiCommand, currentKey);
            }
            return retValue;
        }

        public async Task endSessionAsync()
        {
            XmlFimi fimiLogoff = new XmlFimi();
            var ans11 = await send(fimiLogoff, "Logoff");
        }

        public EnvelopeBodyFault getError()
        {
            return lastError;
        }

        public EnvelopeBodyFault lastError;
        }

        public class XmlFimi : APIExecutor._ApiFilter
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
                if (path.Substring(0, 1) != "$")
                {
                    var patt = string.Join("/", new string[] { "env:Envelope", "env:Body", "*", "Response" }.Union(path.Split('/').Select(ii => "m0:" + ii)));
                    foreach (XmlNode node in xmlDoc.SelectNodes(patt, nsManager))
                        retValue.Add(node.InnerText);
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
            public XmlFimi()
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
            public XmlFimi(string xmlContent)
            {
                Init(xmlContent);
            }

            private void Init(string xmlContent)
            {
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);
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
            }

            public static string XmlPath(string path)
            {
                var lex = path.Replace("FIMI/", "").Replace("/Rq/", "/m1:Request/").Replace("/Rp/", "/Response/").Split("/");
                lex[0] = "m1:" + lex[0];
                if (lex[lex.Length - 1][0] != '@')
                    lex[lex.Length - 1] = "m0:" + lex[lex.Length - 1];
                return string.Join("/", new string[] { "env:Envelope", "env:Body" }.Union(lex));
            }
            public string getPath(string path)
            {
                return xmlDoc.SelectSingleNode(XmlPath(path), nsManager).InnerText;
            }
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

            public void setPath(string path, string Value)
            {
                var tokens = XmlPath(path).Split("/");
                var comm_obj = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(3)), nsManager);
                if (comm_obj == null)
                {
                    var tt = tokens[2].Split(':');
                    var nod = xmlDoc.CreateNode(XmlNodeType.Element, tt[0], tt[1], namespaces[tt[0]]);
                    var rootNode = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(2)), nsManager);
                    var child = rootNode.ChildNodes.Item(0).ChildNodes.Item(0);
                    nod.AppendChild(child);
                    rootNode.ReplaceChild(nod, rootNode.ChildNodes.Item(0));
                }
                var obj = xmlDoc.SelectSingleNode(string.Join('/', tokens.Take(tokens.Length - 1)), nsManager);
                if (tokens.Last()[0] == '@')
                {
                    var attrName = tokens.Last().Substring(1);
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
                    var replaced = xmlDoc.SelectSingleNode(string.Join('/', tokens), nsManager);
                    if (replaced == null)
                    {
                        var tt = tokens.Last().Split(':');
                        var nod = xmlDoc.CreateNode(XmlNodeType.Element, tt[0], tt[1], namespaces[tt[0]]);
                        obj.AppendChild(nod);
                        nod.InnerText = Value;
                    }
                    else
                        replaced.InnerText = Value;


                }

            }

        public string[] filter(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            return extractMulti(path).ToArray();
        }
    }
        /*
        public class Envelope
        {
            XmlDocument xmlDoc;
            public void InitXml()
            {
                xmlDoc = new XmlDocument();
                XmlSchema schema = new XmlSchema();
                schema.Namespaces.Add("env", "http://www.w3.org/2003/05/soap-envelope");
                schema.Namespaces.Add("m0", "http://schemas.compassplus.com/two/1.0/fimi_types.xsd");
                schema.Namespaces.Add("m1", "http://schemas.compassplus.com/two/1.0/fimi.xsd");
                //            schema.Namespaces.Add("xmlns", "http://www.sample.com/file");
                xmlDoc.Schemas.Add(schema);
                //xmlDoc.LoadXml(xmlContent);
                XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace("env", "http://www.w3.org/2003/05/soap-envelope");
                nsManager.AddNamespace("m0", "http://schemas.compassplus.com/two/1.0/fimi_types.xsd");
                nsManager.AddNamespace("m1", "http://schemas.compassplus.com/two/1.0/fimi.xsd");

            }
            string path = "FIMI/GetReport/Rq/BlockSize";

            public static string XmlPath(string path) 
                { 
                    var lex= path.Replace("FIMI/","").Replace("/Rq/","/m1:Request/").Replace("/Rp/", "/m1:Responce/").Split("/");
                    lex[0] = "m1:" + lex[0];
                    if(lex[lex.Length - 1][0] != '@')
                        lex[lex.Length - 1] = "m0:" + lex[lex.Length - 1];
                    return string.Join("/",new string[] {"env:Envelope","env:Body" }.Union( lex));
            }
            public static string getPath( XmlDocument doc,string path, XmlNamespaceManager nsManager)
            {
                return doc.SelectSingleNode(XmlPath(path),nsManager).InnerText;
            }

            public static void setPath(XmlDocument doc, string path,string Value, XmlNamespaceManager nsManager)
            {
                var tokens= XmlPath(path).Split("/");
                var comm_obj= doc.SelectSingleNode(string.Join('/', tokens.Take(3)), nsManager);
                if(comm_obj == null)
                {
                    var tt = tokens.Last().Split(':');
                    var nod = doc.CreateNode(XmlNodeType.Element, tt[0], tt[1], "http://schemas.compassplus.com/two/1.0/fimi_types.xsd");
                    doc.SelectSingleNode(string.Join('/', tokens.Take(2)), nsManager).AppendChild(nod);
                }
                var obj= doc.SelectSingleNode(string.Join('/',tokens.Take(tokens.Length-1)), nsManager);
                if (tokens.Last()[0] == '@')
                {
                    var attrName = tokens.Last().Substring(1);
                    var attr = obj.Attributes.GetNamedItem(attrName);
                    if (attr == null)
                    {
                        var attr2 = doc.CreateAttribute(attrName);
                        attr2.InnerText = Value;
                        obj.Attributes.Append(attr2);
                    }
                    else
                        attr.InnerText = Value;
                } else
                {
                    var replaced = doc.SelectSingleNode(string.Join('/', tokens), nsManager);
                    if (replaced == null)
                    {
                        var tt = tokens.Last().Split(':');
                        var nod = doc.CreateNode(XmlNodeType.Element, tt[0], tt[1], "http://schemas.compassplus.com/two/1.0/fimi_types.xsd");
                        obj.AppendChild(nod);
                        nod.InnerText = Value;
                    } else
                        replaced.InnerText = Value;


                }    

            }


            public string ToXml()
            {
                var paths = path.Split('/');
                var prefs=paths.Last().Split(':');
                var el = xmlDoc.CreateNode(XmlNodeType.Element, prefs[1], prefs[0]);
                if (paths.Length>1)
                {
                    var new_path=string.Join("/", paths.Take(paths.Length-1));
                    var nod=xmlDoc.SelectSingleNode(new_path);
                    nod.AppendChild(el);
                } else
                {
                    xmlDoc.AppendChild(el);

                }
                 xmlDoc.Save(@"c:\d\aa.xml");
                return "";
               // xmlDoc.
            }
        }*/
        /*******************************************/

        // Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private EnvelopeBody bodyField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private EnvelopeBodyFault faultField;

            /// <remarks/>
            public EnvelopeBodyFault Fault
            {
                get
                {
                    return this.faultField;
                }
                set
                {
                    this.faultField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBodyFault
        {

            private EnvelopeBodyFaultCode codeField;

            private EnvelopeBodyFaultReason reasonField;

            private EnvelopeBodyFaultDetail detailField;

            /// <remarks/>
            public EnvelopeBodyFaultCode Code
            {
                get
                {
                    return this.codeField;
                }
                set
                {
                    this.codeField = value;
                }
            }

            /// <remarks/>
            public EnvelopeBodyFaultReason Reason
            {
                get
                {
                    return this.reasonField;
                }
                set
                {
                    this.reasonField = value;
                }
            }

            /// <remarks/>
            public EnvelopeBodyFaultDetail Detail
            {
                get
                {
                    return this.detailField;
                }
                set
                {
                    this.detailField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBodyFaultCode
        {

            private byte valueField;

            /// <remarks/>
            public byte Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBodyFaultReason
        {

            private string textField;

            /// <remarks/>
            public string Text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBodyFaultDetail
        {

            private string nextChallengeField;

            private uint tranIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.compassplus.com/two/1.0/fimi.xsd")]
            public string NextChallenge
            {
                get
                {
                    return this.nextChallengeField;
                }
                set
                {
                    this.nextChallengeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.compassplus.com/two/1.0/telebank.xsd")]
            public uint TranId
            {
                get
                {
                    return this.tranIdField;
                }
                set
                {
                    this.tranIdField = value;
                }
            }
        }


        /*******************************************/



}
