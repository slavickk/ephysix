/******************************************************************
 * File: Form1.cs
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Xml;
using CSScriptLib;
using MaxMind.GeoIP2;
using UniElLib;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using System.Net;
using System.Net.Sockets;
using PluginBase;
using ParserLibrary;
using System.Reflection;

namespace TestJsonRazbor
{
    public partial class Form1 : Form
    {
        public class TreeDrawer : Drawer
        {
            public TreeNode node;
            public TreeDrawer(TreeView treeView1, AbstrParser.UniEl newEl, AbstrParser.UniEl ancestor)
            {
                newEl.treeNode = this;// new TreeNode(newEl.Name);
                node = new TreeNode(newEl.Name);
                if (ancestor == null)
                {
                    treeView1.Nodes[0].Nodes.Add(node);
                    treeView1.Nodes[0].Nodes[^1].Tag = newEl;
                }
                else
                {
                    (ancestor.treeNode as TreeDrawer).node.Nodes.Add(node);
                    (ancestor.treeNode as TreeDrawer).node.Nodes[^1].Tag = newEl;

                }

            }
            public void Update(AbstrParser.UniEl newEl)
            {
                if (node.Nodes.Count == 0)
                {
                    node.Nodes.Add(newEl.Value.ToString());
                    node.Nodes[^1].Tag = this;
                }
                else
                {
                    node.Nodes[0] = new TreeNode(newEl.Value.ToString());
                    node.Nodes[0].Tag = this;
                }

            }
        }

        public class TreeDrawerFactory : DrawerFactory
        {
            TreeView tree;
            public TreeDrawerFactory(TreeView tree1)
            {
                AbstrParser.drawerFactory = this;
                tree = tree1;

            }
            public Drawer Create(AbstrParser.UniEl newEl, AbstrParser.UniEl ancestor)
            {
                return new TreeDrawer(tree, newEl, ancestor);
            }
        }
        public static byte[] PutObjectS(string Method, string postUrl, string payLoad, out string Body)
        {
            Body = "";
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = Method;
            request.ContentType = "application/json";
            if (payLoad != null)
            {
                //                request.Headers.Add("X-Consul-Namespace", "team-1");
                var bytes = System.Text.Encoding.ASCII.GetBytes(payLoad);

                request.ContentLength = (bytes.Length);
                Stream dataStream = request.GetRequestStream();
                //                var bytes = System.Text.Encoding.ASCII.GetBytes(payload);
                dataStream.Write(bytes, 0, bytes.Length);
                //                Serialize(dataStream, payload);
                dataStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            if (returnString == "OK")
            {
                List<byte> outBuff = new List<byte>();
                Stream stream = response.GetResponseStream();
                byte[] buffer = new byte[5000/*response.ContentLength*/];
                int bytesRead;

                {
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);

                        while (bytesRead > 0)
                        {
                            outBuff.AddRange(buffer[..bytesRead]);

                            bytesRead = stream.Read(buffer, 0, 256);
                        }

                        ASCIIEncoding coding = new ASCIIEncoding();
                        char[] chars = coding.GetChars(outBuff.ToArray());
                        Body = new string(chars);
                        return outBuff.ToArray();
                        //           binWriter.Write(buffer1);
                    }
                }

            }
            return new byte[] { };
        }


        public Form1()
        {
            InitializeComponent();
        }


        List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
        TreeDrawerFactory drawFactory;
        string delim = "---------------------------RRRRR----------------------------------";
        void test()
        {
            List<byte> allBytesReceived = new List<byte>();
            using (var stream = new FileStream(@"c:/d/file.encr", FileMode.Open))
            {
                while (stream.Position < stream.Length)
                {
                    byte[] arr = new byte[4];
                    stream.Read(arr, 0, arr.Length);
                    int len = BitConverter.ToInt32(arr, 0);
                    byte[] arr1 = new byte[len];
                    stream.Read(arr1, 0, arr1.Length);
                    IPEndPoint point = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 75, 159 }), 16000);
//                    192.168.75.159:16000
                    TcpClient client = new TcpClient();
                    client.Connect(point);
                    var sr=client.GetStream();
                    sr.Write(arr1, 0, arr1.Length);
                    while(0==0)
                    {
                        int yy = sr.ReadByte();
                        if(yy !=-1)
                        {
                            allBytesReceived.Add((byte)yy);
                        } else
                        {

                        }
                    }
                }
            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            //GenerateStatement.camundaAddr = Resolver.ResolveConsulAddr("Camunda");

            Step.Test();
//            Pipeline
//            var pip=Pipeline.load(@"C:\Users\User\Documents\model.yml");
            Pipeline pip = new Pipeline();
            pip.steps.First().sender = new HTTPSender();
           // pip.Save(@"C:\Users\User\Documents\aa3.yml", Assembly.GetAssembly(typeof(TICReceiver)));
 
/*            var pip2 = Pipeline.load();// (@"C:\D\aa1.yml");
            var pip1 = new Pipeline();
            pip1.Save();*/

            /*     byte bytes = 1;
                 var a = bytes & 0b10000000;
                 var bytes1 = bytes | 0b10000000;
                 var b = bytes1 & 0b10000000;
                 //  test();
                 System.IO.DirectoryInfo di = new DirectoryInfo(@"c:\d\out");

                 foreach (FileInfo file in di.GetFiles())
                 {
                     file.Delete();
                 }
                 int cycle = 10000;
                 for (int i = 0; i < cycle; i++)
                 {
                     string Body;
                     PutObjectS("POST", @"http://localhost:5000/WeatherForecast", "{\"Count\":" + i + "}", out Body);
                 }

                 */

            //    Samples.CreateDelegate();
            /*   int cycle = 1;
               for(int i=0; i < cycle;i++)
               {
                   string Body;
                   PutObjectS("POST", @"http://localhost:5000/WeatherForecast", "{\"Count\":" + i + "}",out Body);
               }

               string aa987 = File.ReadAllText(@"C:\D\aa35870.json");
            */
            if (0 == 1)
            {
                /*  var pip1 = new Pipeline();
                  pip1.Save();
                */
                DateTime time1 = DateTime.Now;

                /*                var pips = Step.load();
                                foreach (var pip in pips)
                                    pip.run();
                */
                var milli = (DateTime.Now - time1).TotalMilliseconds;
                foreach (var item in AbstrParser.chronos)
                {
                    var all = item.intervals.Sum();
                    var avg = item.intervals.Average();
                    Console.WriteLine(item);
                }
                File.WriteAllText(@"c:\d\a1.stat", milli.ToString());

            }

            var aa = AbstrParser.PathBuilder(new string[] { "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name", "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/Params/Param/-Name", "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/Params/Param/String/Model" });          
            drawFactory = new TreeDrawerFactory(treeView1);

            //            AbstrParser.treeView1 = treeView1;
            //            var file_name = @"C:\Data\All_MBCH.postman_collection.json";
            //            var file_name = @"C:\Data\packet_beat_example.json";
            var file_name = @"C:\Data\scratch_1.txt";
//            ParseInput(file_name);
        }

        private void ParseInput(string file_name)
        {
            int ind = 0;
            using (StreamReader sr = new StreamReader(file_name))
            {
                AbstrParser.UniEl rootEl = AbstrParser.CreateNode(null, list, "Item");
                var line =sr.ReadToEnd();
                if (line != "")
                {
                    AbstrParser.getApropriateParser("", line, rootEl, list);
/*                    foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor(line, rootEl, list))
                            break;*/
                }
/*                while (!sr.EndOfStream && ind < 50)
                {
                    ind++;
                    AbstrParser.UniEl rootEl = AbstrParser.CreateNode(null, list, "Item");

                    var line = sr.ReadLine();
                    int pos = line.IndexOf(delim);
                    if (pos >= 0)
                        line = line.Substring(0, pos);
                    if (line != "")
                    {
                        foreach (var pars in AbstrParser.availParser)
                            if (pars.canRazbor(line, rootEl, list))
                                break;
                    }
                }*/

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        int indexSearch = 0;
        const string dirPath = @"C:\Users\User\Documents\PacketOut";
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBoxFromDir.Checked)
            {
                int count = 0;
                comboBoxFoundedFiles.Items.Clear();
                foreach(var fileName in Directory.GetFiles(dirPath))
                {
                    using(StreamReader sw = new StreamReader(fileName))
                    {
                        if(sw.ReadToEnd().Contains(textBoxSearch.Text))
                        {
                            count++;
                            comboBoxFoundedFiles.Items.Add(fileName);
                        }
                    }
                }
                this.Text = "founded " + count + "files";
            }
            else
            {
                found();
            }
        }
        private void buttonPlantUml_Click(object sender, EventArgs e)
        {
        }

        void found()
        {
            checkBox1.Checked = false;
            int index = 0;
            treeView1.Nodes[0].Collapse();
            foreach (TreeNode node1 in treeView1.Nodes)
            {
                var s = searchInTree(node1, textBoxSearch.Text, indexSearch, ref index);
                if (s != null)
                {
                    var s1 = s;
                    while (s1.Parent != null && s1.Parent.Parent != null)
                        s1 = s1.Parent;
                    s1.ExpandAll();
                    indexSearch = index;
                    treeView1.SelectedNode = s;
                    s.EnsureVisible();
                    return;
                }
            }
            MessageBox.Show("not found");

        }

        TreeNode searchInTree(TreeNode node, string searhedText, int indexSearch, ref int index)
        {
            index++;
            if (node.Text.Contains(searhedText) )
                if(index > indexSearch)
                    return node;
            foreach (TreeNode node1 in node.Nodes)
            {
                index++;
                var s = searchInTree(node1, searhedText, indexSearch, ref index);
                if (s != null)
                    return s;
            }
            return null;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*    dynamic script = CSScript.Evaluator
                             .LoadMethod(@"using System;
                                            int Product(int a, int b)
                                           {
                                               return a * b;
                                           }");

                int result = script.Product(3, 3)
            */
         //   Samples.LoadCodeWithInterface();
            //            CScript
            if (checkBox1.Checked)
                indexSearch = 0;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var el = e.Node.Tag as AbstrParser.UniEl;
            if (el != null)
            {
                textBox1.Text = el.path;
                Clipboard.SetText(textBox1.Text);
            }

        }
        void checkGEOIP()
        {
            var client = new WebServiceClient(584367, "F7QJiBtS6s178J7x");
            var cc = client.Country("128.101.101.101");
            // To query the GeoLite2 web service, you must set the optional `host` parameter
            // to `geolite.info`
            //   var client = new WebServiceClient(10, "LICENSEKEY", host: "geolite.info");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //"RXh0ZW5zaW9ucz1yYmFTY29yaW5nHEEwMDAwMDA2NTgwMRwwHHsidG90YWxTY29yZSI6IC0xMDAsInJ1bGVTZXRzIjogW3sicnVsZVNldElkIjogNiwic2NvcmUiOiA1MCwiZGVzY3JpcHRpb24iOiAiTWVyY2hhbnQgdHJhbnNhY3Rpb25zIERldGFpbHMifSx7InJ1bGVTZXRJZCI6IDIsInNjb3JlIjogLTUwLCJkZXNjcmlwdGlvbiI6ICJNZXJjaGFudCBBY2NvdW50IFJ1bGVzIn0seyJydWxlU2V0SWQiOiA1LCJzY29yZSI6IC0zMCwiZGVzY3JpcHRpb24iOiAiVHJhbnNhY3Rpb24gU2NvcmluZyJ9LHsicnVsZVNldElkIjogNCwic2NvcmUiOiAzMCwiZGVzY3JpcHRpb24iOiAiQ2xpZW50IFNjb3JpbmcifSx7InJ1bGVTZXRJZCI6IDEsInNjb3JlIjogLTEyMCwiZGVzY3JpcHRpb24iOiAiVXNlciBEZXZpY2UgcnVsZXMifSx7InJ1bGVTZXRJZCI6IDMsInNjb3JlIjogMjAsImRlc2NyaXB0aW9uIjogIk1lcmNoYW50IFNjb3JpbmcifV19"
//                  var bytes = Convert.FromBase64String("RXh0ZW5zaW9ucz1yYmFTY29yaW5nHEEwMDAwMDA2NTgwMRwwHHsidG90YWxTY29yZSI6IC0xMDAsInJ1bGVTZXRzIjogW3sicnVsZVNldElkIjogNiwic2NvcmUiOiA1MCwiZGVzY3JpcHRpb24iOiAiTWVyY2hhbnQgdHJhbnNhY3Rpb25zIERldGFpbHMifSx7InJ1bGVTZXRJZCI6IDIsInNjb3JlIjogLTUwLCJkZXNjcmlwdGlvbiI6ICJNZXJjaGFudCBBY2NvdW50IFJ1bGVzIn0seyJydWxlU2V0SWQiOiA1LCJzY29yZSI6IC0zMCwiZGVzY3JpcHRpb24iOiAiVHJhbnNhY3Rpb24gU2NvcmluZyJ9LHsicnVsZVNldElkIjogNCwic2NvcmUiOiAzMCwiZGVzY3JpcHRpb24iOiAiQ2xpZW50IFNjb3JpbmcifSx7InJ1bGVTZXRJZCI6IDEsInNjb3JlIjogLTEyMCwiZGVzY3JpcHRpb24iOiAiVXNlciBEZXZpY2UgcnVsZXMifSx7InJ1bGVTZXRJZCI6IDMsInNjb3JlIjogMjAsImRlc2NyaXB0aW9uIjogIk1lcmNoYW50IFNjb3JpbmcifV19");
            var bytes = Convert.FromBase64String("QWNxRXh0UFNGaWVsZHM9Y2NjNDIzNDtndmd2c2Znc2RmPXNkZj12c2RmPXZzZGY9dnM9ZGZ2O3NzPWR2PXNkO2Z2PXNkO2Z2PXNkZjt2cz1kZnZzZGY7dnNkZj12c2RmdjtzZGZ2PXNkZnY7c2Rmdj1zZGZ2O3NkZnZzZGZ2YXN3NGdoO2FlYT1yYjtocWFyYj1oOw==");

            string value = System.Text.Encoding.UTF8.GetString(bytes);
            //  checkGEOIP();
//            var ret = new RecordExtractor().selectAllCond(list);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //textBoxConditionField.Text = Clipboard.GetText();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxValueFieldSearch.Text = Clipboard.GetText();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBoxValueFieldPath.Text = Clipboard.GetText();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
                fields.Add(new ConstantValue() { outputPath = textBoxFieldName.Text, Value = textBoxConstant.Text });
            else
                fields.Add(new ExtractFromInputValue() { outputPath = textBoxFieldName.Text , conditionPath = textBoxValueFieldSearch.Text, conditionCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text))), valuePath = ((textBoxValueFieldPath.Text == "") ? "" : textBoxValueFieldPath.Text) }  );
            listBox1.Items.Add(fields.Last());
        }
        List<OutputValue> fields = new List<OutputValue>();
        int index_list = -1;
        
        // TODO: delete DemoSender once DemoSenderV2 is confirmed to work
        // public class DemoSender : Sender
        // {
        //     TreeView tree;
        //     public static bool noDraw = false;
        //
        //     public override TypeContent typeContent => TypeContent.internal_list;// throw new NotImplementedException();
        //
        //     public DemoSender(TreeView  tree1)
        //     {
        //         tree = tree1;
        //         Clear();
        //     }
        //
        //     public void Clear()
        //     {
        //         if (!noDraw)
        //         {
        //             tree.Nodes.Clear();
        //             tree.Nodes.Add("Output");
        //         }
        //     }
        //
        //     public async override Task<string> sendInternal(AbstrParser.UniEl root,ContextItem context)
        //     {
        //         if (!noDraw)
        //         {
        //             tree.Nodes[0].Nodes.Add("Item");
        //             var rootNode = tree.Nodes[0].Nodes[tree.Nodes[0].Nodes.Count - 1];
        //             foreach (var el in root.childs)
        //                 rootNode.Nodes.Add(el.Name + "\":\"" + el.Value);
        //         }
        //         return "";
        //     }
        // }

  /*      public class DemoSenderV2 : ISender
        {
            TreeView tree;
            public static bool noDraw = false;

            public ISenderHost host { get; set; }

            public Task<string> send(string JsonBody, ContextItem context)
            {
                throw new NotImplementedException();
            }

            public String getTemplate(String key)
            {
                return string.Empty;
            }

            public void setTemplate(String key, String body)
            {
            }

            public TypeContent typeContent => TypeContent.internal_list;// throw new NotImplementedException();
            public void Init()
            {
                // TODO: the previous implementation, Sender, initializes metrics in the base abstract class. Duplicating the code here. Deduplicate.
                metricUpTimeError = new Metrics.MetricHistogram("iu_outbound_errors_total", "handle performance receiver", new double[] { 30, 100, 500, 1000, 5000, 10000 });
                metricUpTimeError.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });

                metricUpTime = new Metrics.MetricHistogram("iu_outbound_request_duration_msec", "handle performance receiver");
                metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
            }

            public DemoSenderV2(TreeView  tree1)
            {
                tree = tree1;
                Clear();
            }

            public void Clear()
            {
                if (!noDraw)
                {
                    tree.Nodes.Clear();
                    tree.Nodes.Add("Output");
                }
            }

            public Task<string> send(UniElLib.AbstrParser.UniEl root, ContextItem context)
            {
                if (!noDraw)
                {
                    tree.Nodes[0].Nodes.Add("Item");
                    var rootNode = tree.Nodes[0].Nodes[tree.Nodes[0].Nodes.Count - 1];
                    foreach (var el in root.childs)
                        rootNode.Nodes.Add(el.Name + "\":\"" + el.Value);
                }
                return "";
            }

            void ISender.setTemplate(string key, string body)
            {
                throw new NotImplementedException();
            }

            void ISender.Init()
            {
                throw new NotImplementedException();
            }
        }
        */
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
/*                Step pip = new Step() { receiver = new FileReceiver(),sender = new DemoSender(treeView2) };
                pip.filters.Clear();
                pip.filters.Add(new ConditionFilter() { conditionPath = textBoxFilterFieldPath.Text, conditionCalcer = new ComparerForValue() { value_for_compare = textBoxFilterValue.Text } });
                pip.outputFields = fields;
                pip.run().GetAwaiter().GetResult();
                listBox2.Items.Clear();*/
              /*  foreach (var item in pip.sender.outString)
                {
                    listBox2.Items.Add(item);
                }*/

                textBoxYaml.Text = Pipeline.ToStringValue(new Pipeline(), Assembly.GetAssembly(typeof(HTTPSender)));
                
/*                RecordExtractor ext = buildExtractor();

                //                Serialize(ext);

                var ret = ext.selectAllCond(list);
                listView1.Items.Clear();
                foreach (var it in ret)
                {
                    string[] ff = new string[2];
                    ff[0] = it.nameRecord;
                    ff[1] = it.fields.Aggregate(
"", // start with empty string to handle empty list case.
(current, next) => current + "; " + next);
                    listView1.Items.Add(new ListViewItem(ff));

                }*/
            }
            catch (Exception e77)
            {

            }

        }

        private RecordExtractor buildExtractor()
        {
            return null;
           // return new RecordExtractor() { /*nameRecord = textBoxRecordName.Text,*/ condCalcer = new ComparerForValue(textBoxConditionValue.Text), condPath = textBoxConditionField.Text };
        }

        void Serialize(RecordExtractor obj, string path = @"C:\Data\aa.xml")
        {

            BinaryFormatter b = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                b.Serialize(stream, obj);
            }


            //            stream.Close();
        }

        RecordExtractor Deserialize(string path = @"C:\Data\aa.xml")
        {
            BinaryFormatter b = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return b.Deserialize(stream) as RecordExtractor;
            }

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
            else
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = true;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Serialize(buildExtractor(), saveFileDialog1.FileName);
            }
        }
        public class ii : ITypeResolver,INodeTypeResolver
        {
            public Type Resolve(Type staticType, object actualValue)
            {
                if (actualValue == null)
                    return staticType;
                return actualValue.GetType();
//                throw new NotImplementedException();
            }

            public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
            {
                if(currentType == typeof(IReceiver))
                    {
                    currentType = typeof(PacketBeatReceiver);
                }
                if (currentType == typeof(ISender))
                {
                    currentType = typeof(HTTPSender);
                }

                return true;
//                throw new NotImplementedException();
            }

      
        }
        public class ScalarOrSequenceConverter : IYamlTypeConverter
        {
            public bool Accepts(Type type)
            {
                return typeof(IEnumerable<string>).IsAssignableFrom(type);
            }
            public object ReadYaml(IParser parser, Type type)
            {
                if (parser.TryConsume<Scalar>(out var scalar))
                {
                    return new List<string> { scalar.Value };
                }
                return null;
            }

            public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
            {
                throw new NotImplementedException();
            }

            public void WriteYaml(IEmitter emitter, object value, Type type)
            {
                var sequence = (IEnumerable<string>)value;
                if (sequence.Count() == 1)
                {
                    emitter.Emit(new Scalar(default, sequence.First()));
                }
                else
                {
                    emitter.Emit(new SequenceStart(default, default, false, SequenceStyle.Any));
                    foreach (var item in sequence)
                    {
                        emitter.Emit(new Scalar(default, item));
                    }
                    emitter.Emit(new SequenceEnd());
                }
            }

            public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        string lastFile = "";
        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lastFile = openFileDialog1.FileName;
                treeView1.Nodes[0].Nodes.Clear();
                ParseInput(lastFile);
            }


                //            Pipeline pip = new Pipeline();
                //            Pipeline.Save(new Pipeline[] { new Pipeline(), new Pipeline() });
                /*            var pips=Pipeline.load();
                            Pipeline pip = pips[0];
                            var filt = pip.filters[0] as ConditionFilter;

                            textBoxFilterFieldPath.Text = filt.conditionPath;
                            var compFilter=filt.conditionCalcer as ComparerForValue;
                            if(filt.conditionCalcer is ComparerForValue)
                                textBoxFilterValue.Text=compFilter.value_for_compare;


                            fields = pip.outputFields;
                            listBox1.Items.Clear();
                            listBox1.Items.AddRange(fields.ToArray());*/
                //            var serializer = new Serializer();
                /*            var serializer = new SerializerBuilder()
                    .WithTypeResolver(new ii())
                    .WithTagMapping(new YamlDotNet.Core.TagName("!PacketBeatReceiver"),typeof(PacketBeatReceiver))
                    .WithTagMapping(new YamlDotNet.Core.TagName("!JsonSender"), typeof(JsonSender))
                    .WithTagMapping(new YamlDotNet.Core.TagName("!ComparerForValue"), typeof(ComparerForValue))
                                    .WithTypeConverter(new ScalarOrSequenceConverter())
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();


                                        var deserializer = new DeserializerBuilder()
                    .WithTagMapping(new YamlDotNet.Core.TagName("!PacketBeatReceiver"),typeof(PacketBeatReceiver))
                    .WithTagMapping(new YamlDotNet.Core.TagName("!JsonSender"), typeof(JsonSender))
                    .WithTagMapping(new YamlDotNet.Core.TagName("!ComparerForValue"), typeof(ComparerForValue))
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                 .Build();

                            var result = deserializer.Deserialize<Pipeline>(File.OpenText(@"C:\d\aa1.yml")); 
                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                var f = Deserialize(openFileDialog1.FileName);
                //                textBoxRecordName.Text = f.nameRecord;
                                textBoxConditionValue.Text = (f.condCalcer as ComparerForValue).value_for_compare;
                                textBoxConditionField.Text = f.condPath;
                //                fields = f.fields.ToList();
                                listBox1.Items.Clear();
                                listBox1.Items.AddRange(f.fields);
                                Pipeline pock = new Pipeline() {  };
                                using (StreamWriter sw = new StreamWriter(@"C:\d\aa1.yml"))
                                {
                                    serializer.Serialize(sw, pock);
                                }
                //                var ser = YamlDotNet.Serialization.Serializer();
                            }*/
            }

            private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            index_list = listBox1.SelectedIndex;
            if (index_list >= 0)
            {
                textBoxFieldName.Text = fields[index_list].outputPath;
                /*
                textBoxFalueFieldSearchValue.Text = ((fields[index_list].condCalcer == null) ? "" : (fields[index_list].condCalcer as ComparerForValue).value_for_compare);
                textBoxValueFieldSearch.Text = fields[index_list].condPath;
                textBoxValueFieldPath.Text = fields[index_list].valuePath;
                */
                button10.Enabled = button11.Enabled = true;
            }
            else
            {
                button10.Enabled = button11.Enabled = false;
                textBoxFieldName.Text = textBoxFalueFieldSearchValue.Text =textBoxValueFieldSearch.Text = textBoxValueFieldPath.Text = "";

            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (index_list >= 0)
            {
/*                fields[index_list].nameField = textBoxFieldName.Text;
                fields[index_list].condCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text)));
                fields[index_list].condPath = textBoxValueFieldSearch.Text;
                fields[index_list].valuePath = ((textBoxValueFieldPath.Text == "") ? "" : textBoxValueFieldPath.Text);*/
                listBox1.Items[index_list] = fields[index_list];
                listBox1.Refresh();
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (index_list >= 0)
            {
                fields.RemoveAt(index_list);
                listBox1.Items.RemoveAt(index_list);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( comboBox1.SelectedIndex != 0)
            {
                textBoxFilterValue.Visible=label3.Visible = false;
//                this.tabPage2.Focus();
            } else
            {
                textBoxFilterValue.Visible = label3.Visible = true;

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
           textBoxFilterFieldPath.Text = Clipboard.GetText();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBoxAddFieldPath.Text = Clipboard.GetText();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                panel3.Visible = false;
                panel4.Visible = true;
            } else
            {
                panel3.Visible = true;
                panel4.Visible = false;

            }

        }

        private void comboBoxFoundedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxFoundedFiles.SelectedIndex >=0)
            {
                treeView1.Nodes[0].Nodes.Clear();
                indexSearch = 0;
                lastFile = comboBoxFoundedFiles.SelectedItem.ToString();
                ParseInput(lastFile);
                found();

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int iCycle = 0;
            using (StreamWriter sw = new StreamWriter(@"C:\Users\User\Documents\forDetect.csv"))
            {
                foreach (var fileName in Directory.GetFiles(dirPath))
                {
                    Console.WriteLine("exec " + (++iCycle));
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        list.Clear();
                        AbstrParser.UniEl rootEl = AbstrParser.CreateNode(null, list, "Item");
                        var line = sr.ReadToEnd();
                        if (line != "")
                        {
                            foreach (var pars in AbstrParser.availParser)
                                if (pars.canRazbor("",line, rootEl, list))
                                    break;
                        }

                        foreach (var it in list)
                        {
                            if (it.childs.Count == 0 && it.Value != null)
                                sw.WriteLine(it.path + ";" + it.Value.ToString().Replace("\n","").Replace("\r", ""));

//                                it.Value
                        }

                    }
                }
            }
//            this.Text = "founded " + count + "files";
            return;






   /*         if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var pip = Pipeline.load(openFileDialog2.FileName);
                    pip.SelfTest();
                    TestReceiver rec;
                    if (checkBoxFromDir.Checked)
                        rec = new TestReceiver() { path = dirPath, pattern = "*.*" };
                    else
                        rec = new TestReceiver() { path = lastFile, pattern = "" };
                    pip.steps[0].receiver = rec;
                    pip.steps[0].sender = new DemoSenderV2(treeView2);

                    int cycle = 1000;
                    ConditionFilter.isNew = true;
                        DemoSenderV2.noDraw= true;
                    DateTime time1 = DateTime.Now;
                    for (int i = 0; i < cycle; i++)
                    {
                        pip.run().GetAwaiter().GetResult();
                    }
                    var milli = (DateTime.Now - time1).TotalMilliseconds;
                    int index=this.Text.IndexOf("::");
                    string text = ":: exec " + treeView2.Nodes[0].Nodes.Count;
                    if (index < 0)
                        this.Text += text;
                    else
                        this.Text = this.Text.Substring(0, index) + this.Text;
                    treeView2.ExpandAll();
                } 
                catch(Exception e78)
                {
                    MessageBox.Show(e78.ToString());
                }
            }*/
        }

        private void splitContainer5_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public interface ICalc
    {
        int Sum(int a, int b);
    }

}
