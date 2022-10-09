//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsETLPackagedCreator;
using static ParserLibrary.DMNExecutorSender;

namespace TestJsonRazbor
{
    [GUI(typeof(DMNExecutorSender))]
    public partial class FormDMNView : Form,SenderDataExchanger
    {
        public string xml;
        DMNExecutorSender sender;
        public FormDMNView(DMNExecutorSender sender1)
        {
            sender = sender1;
//            xml = xml1;
//            using (StreamReader sr = new StreamReader(@"C:\Camunda\DMNSummator.xml"))
         /*   using (StreamReader sr = new StreamReader(@"C:\Camunda\DMNEmpty.xml"))
            {
                xml = sr.ReadToEnd();
            }*/

            InitializeComponent();
        }

        private void FormDMNView_Load(object sender, EventArgs e)
        {
            if(this.sender!= null)
            {
                xml = this.sender.XML;

            }
            string var_body;
            using (StreamReader sr = new StreamReader(fileNameVars))
            {
                var_body = sr.ReadToEnd();
            }
            var vars=DMNExecutorSender.formJson(var_body);
            foreach(var item in vars)
            {
                treeView1.Nodes[0].Nodes.Add(item.Name);
                var last= treeView1.Nodes[0].LastNode;
                if (item.Name == "SignalledRules")
                {
                    foreach(var item2 in (item.Value as List<SignalledRule>))
                    {
                        last.Nodes.Add("RuleID");
                        last.LastNode.Nodes.Add(item2.RuleID);
                        last.Nodes.Add("Result");
                        last.LastNode.Nodes.Add(item2.Result);
                        last.Nodes.Add("Severity");
                        last.LastNode.Nodes.Add(item2.Severity.ToString());
                    }
                }
                if (item.Name == "InputRecord")
                {
                    foreach (var item2 in (item.Value as List<RecordField>))
                    {
                        //                    var item2 = (item.Value as List<RecordField>)[0];
                        last.Nodes.Add("Key");
                        last.LastNode.Nodes.Add(item2.Key);
                        last.Nodes.Add("Value");
                        last.LastNode.Nodes.Add(item2.Value);
                    }

                }
                if (item.Name == "InactiveRules")
                {
                    foreach (var item2 in (item.Value as List<string>))
                    {
                        //                    var item2 = (item.Value as List<RecordField>)[0];
                        last.Nodes.Add(item2);
                    }
                }

            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
          //  var el = e.Node.Tag as AbstrParser.UniEl;
            if (e.Node != null)
            {
               // textBox1.Text = el.path;
                Clipboard.SetText(e.Node.Text);
            }

        }
        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            FillDMN();

        }

        private void FillDMN()
        {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(xml);

            webView21.ExecuteScriptAsync($"InitMod1({data})");
            //   await JS.InvokeVoidAsync($"InitMod1({data})}", st, alreadyLoaded);
        }

        public class Item
        {
            public string xml { get; set; }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            /*          var result = await webView21.ExecuteScriptAsync("exportDiagram()");
          //            var result = await webView21.ExecuteScriptAsync("Math.sin(Math.PI/2)");
                      MessageBox.Show(result);*/

            await getXML();
            this.sender.setXML(xml);
            //            MessageBox.Show(xml);

        }

        private async Task getXML()
        {
            var res = await webView21.ExecuteScriptAsync("getXML()");
            //            MessageBox.Show(xml);
            Thread.Sleep(500);
            string ss = await webView21.ExecuteScriptAsync("getXML2()");
            Item it = JsonSerializer.Deserialize<Item>(ss.Trim());
            xml = it.xml;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                {
                    xml = sr.ReadToEnd();
                }

                this.webView21.Reload();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                await getXML();
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    sw.Write(xml);
                }
            }
        }

        public string getContent()
        {
            return xml;
        }

        public void setContent(string content)
        {
            xml=content;
            this.webView21.Reload();
        }

        
        void messageSend(string mess)
        {
            MessageBox.Show(mess);

        }
        string fileNameVars = @"C:\Users\User\source\repos\Polygons\DMN_DATA_EXAMPLE\vars.json";
        private async void button4_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            await getXML();
            string var_body;
            using(StreamReader sr = new StreamReader(fileNameVars))
            {
                var_body = sr.ReadToEnd();
            }
            string message ;
           var variables = DMNExecutorSender.ExecDMNForXML(xml, var_body,out message);
            if(variables == null)
            {

                await Task.Run(() => MessageBox.Show(message));
                return;
                int index = message.IndexOf("\r\n")-1;
                if (index == -1)
                    index = message.Length;
                toolStripStatusLabel1.Text =  message.Substring(0,index);
//                this.Invoke(messageSend, new object[] { message });
                return;
            }
            FormViewDMNResults frm = new FormViewDMNResults();
            frm.setVars(variables);
            frm.Show();

           // this.sender.setXML(xml);

        }
    }
}
