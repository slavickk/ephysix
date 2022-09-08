//using Newtonsoft.Json;
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
    }
}
