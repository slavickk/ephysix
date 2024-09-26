/******************************************************************
 * File: FormPipeline.cs
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
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
//using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using DotLiquid;
using Newtonsoft.Json;
using ParserLibrary;
using PluginBase;
using Plugins;
using UniElLib;
using static ScintillaNET.Style;
//using TICSender = Plugins.TICSender;

namespace TestJsonRazbor
{
    public partial class FormPipeline : Form
    {
        public FormPipeline()
        {
            InitializeComponent();
        }

        private void AddNode(object sender, EventArgs e)
        {
            var stepName = "Step_" + pip.steps.Length;
            var newStep = new Step() { owner = pip, IDStep = stepName, IDPreviousStep = ((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? "") };
            List<Step> steps = pip.steps.ToList();
            steps.Add(newStep);
            pip.steps = steps.ToArray();
            (selectedNode == null ? treeView1.Nodes : selectedNode.Nodes).Add(new TreeNode(stepName) { ContextMenuStrip = this.contextMenuStrip1, Tag = newStep });
            treeView1.ExpandAll();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }
        TreeNode selectedNode;
        Step currentStep;
        private void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            currentStep.isBridge = checkBox1.Checked;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node;
            currentStep = e.Node.Tag as Step;
            if (currentStep != null)
            {
                textBoxIDStep.Text = currentStep.IDStep;
                textBoxIDPrevStep.Text = currentStep.IDPreviousStep;
                textBoxFamilyStep.Text = currentStep.IDFamilyStep;
                textBoxIDFamilyPrevious.Text = currentStep.IDFamilyPreviousStep;
                checkBox1.Checked = currentStep.isBridge;
                checkBoxHandleSendError.Checked = currentStep.isHandleSenderError;
                textBoxResponceStep.Text = currentStep.IDResponsedReceiverStep;
                if (currentStep.IDPreviousStep == "")
                    buttonSetupReceiver.Enabled = true;
                else
                    buttonSetupReceiver.Enabled = false;

                textBoxStepDescription.Text = currentStep.description;
                textBoxRestorePath.Text = currentStep.SaveErrorSendDirectory;

                RedrawFilters();
                if (currentStep.receiver == null)
                    buttonReceiverMoc.Enabled = false;
                else
                    buttonReceiverMoc.Enabled = true;
                if (currentStep.sender == null)
                    buttonSenderMoc.Enabled = false;
                else
                    buttonSenderMoc.Enabled = true;
                SetSenderObject(currentStep.sender);
                SetReceiverObject(currentStep.receiver);

            }
        }

        private void RedrawFilters()
        {
            listBox1.Items.Clear();
            if (currentStep.filterCollection == null)
                currentStep.filterCollection = new List<Step.ItemFilter>();
            listBox1.Items.AddRange(currentStep.filterCollection.ToArray());
        }

        Pipeline pip;
        string pipelinePath = "";
        string fileNameStorageContext = "lastPipeline.inf";
        void saveStorageContext(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileNameStorageContext))
            {
                sw.WriteLine(fileName);
                this.Text = fileName;
            }
        }

        void dd()
        {
            var targetDirectory = @"C:\\dd";
            List<string> ll = new List<string>();
            foreach (var file in Directory.GetFiles(targetDirectory).OrderBy(ii => File.GetLastWriteTime(ii)))
            {
                ll.Add(file + " " + File.GetLastWriteTime(file).ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK"));
            }
        }

        private async void FormPipeline_Load(object sender, EventArgs e)
        {
            Pipeline.AgentPort = -1;

            //  await TIPTICRecieverTests.Test();

            //   await Pipeline.GetContentFromGit("https://github.com/tk42/swagger-to-html-standalone/blob/master/README.md"/*"https://github.com/slavickk/projectx/blob/dev/.gitlab-ci.yml"*/);
            string json_body;
            /*
            using (StreamReader sr = new StreamReader(@"C:\Users\ygasnikov\source\repos\swagger-to-html-standalone-master\example\swagger.json"))
            {
                json_body = sr.ReadToEnd();
            }
            using(StreamWriter sw = new StreamWriter(@"C:\Users\ygasnikov\source\repos\swagger-to-html-standalone-master\doc2.html"))
            {
                sw.Write(HTTPReceiver.GetSwaggerHtmlBody(json_body));

            }
            */
            //   dd();
            if (Program.ExecutedPath != "")
            {
                string fileName = Program.ExecutedPath;
                this.Text = fileName;
                if (File.Exists(fileName))
                    try
                    {
                        LoadYaml(fileName);
                    }
                    catch (Exception e67)
                    {
                        MessageBox.Show(e67.ToString());
                    }
            }
            else
            {
                if (File.Exists(fileNameStorageContext))
                {
                    string fileName = "";
                    using (StreamReader sr = new StreamReader(fileNameStorageContext))
                    {
                        fileName = sr.ReadLine();
                    }
                    if (File.Exists(fileName))
                    {
                        this.Text = fileName;
                        LoadYaml(fileName);
                    }
                    else
                    {
                        pip = new Pipeline();
                        this.Text = (pipelinePath == "") ? "new Pipeline" : pipelinePath;

                    }
                }
                else
                {
                    pip = new Pipeline();
                    this.Text = (pipelinePath == "") ? "new Pipeline" : pipelinePath;
                }
            }
        }

        private void LoadYaml(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                //                pipelinePath = sr.ReadToEnd();
                try
                {
                    pip = Pipeline.load(fileName, null);

                    RefreshPipeline();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void RefreshPipeline()
        {
            treeView1.Nodes.Clear();
            listBox1.Items.Clear();
            textBoxPipelineDescription.Text = pip.pipelineDescription;
            button6.Text = pip.saver?.path ?? "no_log";

            buttonReceiverMoc.Enabled = buttonSenderMoc.Enabled = false;
            var prevStep = "";
            FillStep(pip, prevStep, treeView1.Nodes);
            if (treeView1.Nodes.Count > 0)
                selectedNode = treeView1.Nodes[0];
            else
                selectedNode = null;
            if (pip.allMocks != null)
            {
                comboBoxAllMocks.Items.Clear();
                comboBoxAllMocks.Items.AddRange(pip.allMocks.ToArray());    
            }
        }

        private void FillStep(Pipeline pip, string prevStep, TreeNodeCollection col)
        {
            foreach (var step in pip.steps.Where(ii => ii.IDPreviousStep == prevStep))
            {
                var node = new TreeNode() { Text = step.IDStep, Tag = step, ContextMenuStrip = this.contextMenuStrip1 };
                col.Add(node);
                FillStep(pip, step.IDStep, node.Nodes);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;
            ((TreeView)sender).SelectedNode = e.Node;
        }
        Receiver rec;
        IReceiver irec;
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentStep == null)
            {
                MessageBox.Show("current step is null");
                return;
            }
            FormTypeDefiner frm = new FormTypeDefiner() { tDefine = new Type[] { typeof(Receiver), typeof(IReceiver) }, tObject = (currentStep.receiver == null && currentStep.ireceiver == null) ? new HTTPReceiverSwagger/*PacketBeatReceiver*/() : (currentStep.receiver == null) ? currentStep.ireceiver : currentStep.receiver };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SetReceiverObject(frm.tObject);
                if (frm.tObject.GetType().IsAssignableTo(typeof(IReceiver)))
                    currentStep.ireceiver = irec;
                else
                    currentStep.receiver = rec;
            }

        }

        private void SetReceiverObject(object tObject)
        {
            if (tObject != null)
            {
                this.buttonSetupReceiver.Text = "Receiver:" + tObject.GetType().Name;
                buttonTestReceiver.Enabled = true;
                if (tObject.GetType().IsAssignableTo(typeof(IReceiver)))
                    irec = tObject as IReceiver;
                else
                    rec = tObject as Receiver;
                buttonReceiverMoc.Enabled = true;
            }
        }

        async Task proxyRec(string body, object Context)
        {
            string path = @"C:\D\Out";
            var fileName = "tic" + Path.GetRandomFileName();
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, fileName)))
            {
                sw.Write(body);

            }



        }
        private void buttonTestReceiver_Click(object sender, EventArgs e)
        {
            if ((rec != null))
            {

                rec.stringReceived = proxyRec;
                Task taskA = Task.Run(async () =>
                {
                    await rec.start();
                });
            }
            else
            {

                //  irec.stringReceived = proxyRec;
                Task taskA = Task.Run(async () =>
                {
                    await irec.start();
                });
            }

        }
        Sender sender1;
        private void button3_Click(object sender, EventArgs e)
        {
            if (currentStep != null)
            {
                FormTypeDefiner frm = new FormTypeDefiner() { tDefine = new Type[] { typeof(Sender), typeof(ISender) }, tObject = ((currentStep.sender == null) ? new ParserLibrary.HTTPSender() : currentStep.sender) };
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    SetSenderObject(frm.tObject);
                    currentStep.sender = sender1;

                }
            }
        }

        private void SetSenderObject(object tObject)
        {
            if (tObject != null)
            {
                this.buttonSetupSender.Text = "Sender:" + tObject.GetType().Name;
                buttonTestServer.Enabled = true;
                sender1 = tObject as Sender;
                buttonSenderMoc.Enabled = true;
            }
        }

        private void buttonTestServer_Click(object sender, EventArgs e)
        {
            bool withoutFilter = true;
            if (currentStep != null && (rec.MocFile != "" || (rec.MocBody ?? "") != "") && withoutFilter == false)
            {
                Task taskA = Task.Run(async () =>
                {

                    string input;
                    if ((rec.MocBody ?? "") != "")
                    {
                        input = rec.MocBody;
                    }
                    else
                        using (StreamReader sr = new StreamReader(rec.MocFile))
                        {
                            input = sr.ReadToEnd();
                        }
                    DateTime time2 = DateTime.Now;
                    List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                    var rootElement = AbstrParser.CreateNode(null, list, currentStep.IDStep);
                    rootElement = AbstrParser.CreateNode(rootElement, list, "Rec");
                    var res = await currentStep.FilterInfo1(input, time2, list, rootElement, null);
                    string path = @"C:\D\Out";
                    var fileName = Path.Combine(path, "s_" + sender1.GetType().Name + "_" + Path.GetRandomFileName());
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        sw.Write(res);

                    }
                    sender1.MocFile = fileName;

                });

            }
            else
            {



                string sends;
                using (StreamReader sr = new StreamReader(@"C:\D\Out\tic3.json"))
                {
                    sends = sr.ReadToEnd();
                }
                Task taskA = Task.Run(async () =>
                {
                    var res = await sender1.send(sends, null);
                    int yy = 0;
                    string path = @"C:\D\Out";
                    var fileName = "s_tic" + Path.GetRandomFileName();
                    using (StreamWriter sw = new StreamWriter(Path.Combine(path, fileName)))
                    {
                        sw.Write(res);

                    }

                });
            }
            //            MessageBox.Show(res);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                FormSelectField frm = new FormSelectField() { itemFilter = currentStep.filterCollection[listBox1.SelectedIndex], currentStep = currentStep };
                frm.Show();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (currentStep != null)
            {
                Step.ItemFilter filter = new Step.ItemFilter();
                currentStep.filterCollection.Add(filter);
                RedrawFilters();
            }

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (currentStep != null && listBox1.SelectedIndex >= 0)
            {
                currentStep.filterCollection.RemoveAt(listBox1.SelectedIndex);
                RedrawFilters();
            }

        }

        private void buttonSavePipeline_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(saveFileDialog1.FileName, saveFileDialog1.FileName.Replace(".yml", ".bak"), true);
                    pip.Save(saveFileDialog1.FileName, Assembly.GetAssembly(typeof(Samples)));
                    saveStorageContext(saveFileDialog1.FileName);
                    if (checkBoxShowVars.Checked)
                    {
                        using (StreamReader sr = new StreamReader(saveFileDialog1.FileName))
                        {
                            var body = sr.ReadToEnd();
                            MessageBox.Show(string.Join(',', Pipeline.getEnvVariables1(body).Select(ii => ii.pattern).Distinct()));
                        }
                    }
                }
            }
            catch (Exception e77)
            {
                MessageBox.Show(e77.ToString());
            }
        }

        private void buttonReceiverMoc_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentStep.receiver.MocFile = openFileDialog1.FileName;
                currentStep.receiver.MocBody = "";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                currentStep.sender.MocFile = openFileDialog1.FileName;

        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index > 0)
            {
                var el = currentStep.filterCollection[index];
                currentStep.filterCollection.Insert(index - 1, el);
                currentStep.filterCollection.RemoveAt(index + 1);
                //                redrawOutput();
            }

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            (new FormTestPipeline() { pip = pip }).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var oldID = currentStep.IDStep;
            currentStep.IDStep = textBoxIDStep.Text;
            currentStep.IDPreviousStep = textBoxIDPrevStep.Text;
            currentStep.IDFamilyStep = textBoxFamilyStep.Text;
            currentStep.IDFamilyPreviousStep = textBoxIDFamilyPrevious.Text;
            currentStep.isBridge = checkBox1.Checked;
            currentStep.isHandleSenderError = checkBoxHandleSendError.Checked;
            currentStep.IDResponsedReceiverStep = textBoxResponceStep.Text;
            currentStep.description = textBoxStepDescription.Text;
            currentStep.SaveErrorSendDirectory = textBoxRestorePath.Text;
            foreach (var step in pip.steps.Where(ii => ii.IDPreviousStep == oldID))
                step.IDPreviousStep = currentStep.IDStep;
            treeView1.Refresh();

        }

        private void textBoxPipelineDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonYaml_Click(object sender, EventArgs e)
        {
            var sw = new StringWriter();
            pip.Save(sw, Assembly.GetAssembly(typeof(ParserLibrary.HTTPSender)));
            sw.Flush();
            new FormYamlCode(sw.ToString()).ShowDialog();
            //            MessageBox.Show(sw.ToString());
            sw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var frm = new FormYamlCode(currentStep.receiver == null ? currentStep.ireceiver.MocBody : currentStep.ireceiver.MocBody, "Set Moc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (currentStep.receiver != null)
                    currentStep.receiver.MocBody = frm.Body;
                else
                    currentStep.ireceiver.MocBody = frm.Body;

            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadYaml(openFileDialog1.FileName);
                saveStorageContext(openFileDialog1.FileName);
            }
        }

        //        string addLinkItem(string input,string output)
        private void buttonPlantUml_Click(object sender, EventArgs e)
        {
            if (pip != null)
            {
                string TemplateBody3 = @"
## Flow diagram
### {{pipeline.Description}}

```plantuml
@startuml
box ""PCI DSS zone"" #EEEEFF
{% for chld in pipeline.Childs %}participant ""{{chld.Name}}""     as {{chld.Name}}  order {{chld.nOrder}}  #FF9999
{% endfor %}

end box
legend left
<#FFFFFF,#FFFFFF>|<#99FF99>   | CCFA components|   |<#FFFFFF,#FFFFFF>|<#FF9999>   | Other C+ components|
endlegend
autonumber
  == AReq ==
{% for chld in pipeline.Childs %}
{% for chld1 in chld.Childs %}
{% if chld.isFirst == true %}  [o->{{chld.Name}}:{{chld.Protocol}}{% endif %}
 {{chld.Name}} -[#FF3333]> {{chld1.Name}} : <b>{{chld.Step.Name}}</b> {{chld.Step.Description}}
{% if chld1.isFirst == true %}  {{chld1.Name}}->[:{{chld1.Protocol}}{% endif %}{% endfor %}{% endfor %}
@enduml
```
{% for step in pipeline.Steps %}
## {{step.Name}}
```plantuml
@startuml
class {{step.Name}} << (S,orchid) >>
{
{% for flt in step.filters %}{% if flt.Input != """" %} 
+ {{flt.Input}}{% endif %}{% endfor %}
}
class {{step.sender.Name}} << (D,orchid) >>
{
{% for flt in step.filters %}
+ {{flt.Output}}{% endfor %}
}
{% for flt in step.filters %}
{{step.Name}}::{{flt.Input}}->{{step.sender.Name}}::{{flt.Output}}{% endfor %}
@enduml
```
{% endfor %}
";
                string TemplateBody2 = @"
@startuml
!pragma svginterface true
title =ACS->TWO transform{% for object in objects %}
class {{object.Name}} << ({{object.Type}},orchid) >>
{
{% for member in object.members %}
+{{member.Name}}{% endfor %}
}{% endfor %}{% for object in objects %}{% for member in object.members %}{% for dest in member.destinators %}
{{object.Name}}::{{member.Name}}->{{dest.Owner}}::{{dest.Name}}{% endfor %}{% endfor %}{% endfor %}
@enduml
";
                //        RenderParameters param1= new RenderParameters()
                Template template = Template.Parse(TemplateBody3); // Parses and compiles the template
                                                                   //        Template template = Template.Parse("hi {{name}}"); // Parses and compiles the template
                var res = template.Render((DotLiquid.Hash.FromDictionary(new Dictionary<string, object>() { { "pipeline", pip } }))); // => "hi tobi"
                                                                                                                                      //        var res = template.Render((Hash.FromDictionary(new Dictionary<string, object>() { { "products", new Product[] {new Product(1),new Product(2) } }, { "products1", 2 } }))); // => "hi tobi"
                                                                                                                                      // var res =template.Render((Hash.FromAnonymousObject(new { name = "tobi" }))); // => "hi tobi"
                Console.WriteLine($"Hello, World! {res}");

            }
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            pip = new Pipeline();
            this.Text = "new Pipeline";
            RefreshPipeline();
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            // ParserLibrary.PostgresSender.Test();
            //  (new FormDMNView(null) {  xml=""}).ShowDialog();
            return;
            dynamic employee = new ExpandoObject();
            ((IDictionary<String, Object>)employee).Add("Age", 19);
            Console.WriteLine(employee.Age);
            //            await DMNExecutorSender.execDmn(null);



            //            DMNExecutorSender.execDmn("");
            return;
            string json = @"
    {
        ""Name"": ""Squid Game"",
        ""Genre"": ""Thriller"",
        ""Rating"": {
            ""Imdb"": 8.1,
            ""Rotten Tomatoes"": 0.94
        },
        ""Year"": 2021,
        ""Stars"": [""Lee Jung-jae"", ""Park Hae-soo""],
        ""Language"": ""Korean"",
        ""Budget"": ""$21.4 million""
    }";
            //            dynamic d = JObject.Parse(json);

            var data = JsonConvert.DeserializeObject<dynamic>(json)!;
            var genre = data.Genre;
            var imdb = data.Rating.Imdb;
        }

        private void textBoxIDPrevStep_TextChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void removeStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Step> steps = pip.steps.ToList();
            steps.Remove(currentStep);
            pip.steps = steps.ToArray();

            (selectedNode == null ? treeView1.Nodes : selectedNode.Nodes).Remove(selectedNode);//.Add(new TreeNode(stepName) { ContextMenuStrip = this.contextMenuStrip1, Tag = newStep });
            treeView1.ExpandAll();
        }

        private void FormPipeline_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormFormCounter frm = new FormFormCounter(pip);
            frm.ShowDialog();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        Step.ItemFilter copyFilter = null;
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                copyFilter = currentStep.filterCollection[listBox1.SelectedIndex];

        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            if (copyFilter != null)
            {
                try
                {
                    var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                    string ans = Newtonsoft.Json.JsonConvert.SerializeObject(copyFilter, jset);
                    var newEl = Newtonsoft.Json.JsonConvert.DeserializeObject<Step.ItemFilter>(ans, jset);
                    newEl.Name += "_1";
                    currentStep.filterCollection.Add(newEl);
                    RedrawFilters();
                }
                catch (Exception e77)
                {
                    MessageBox.Show(e77.Message);
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pip.saver == null)
                    pip.saver = new ReplaySaver();
                pip.saver.path = this.folderBrowserDialog1.SelectedPath;
                //                pip.saver.enable = true;
                button6.Text = pip.saver.path;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormSwaggerFromXML frm = new FormSwaggerFromXML();
            frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var frm = new FormConv();
            // var frm = new FormTestShablon();
            frm.ShowDialog();
        }

        private void buttonCorrectOccurences_Click(object sender, EventArgs e)
        {
            foreach (var item in Pipeline.configuration.GetChildren())
            {

            }
        }

        private void iNSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var stepName = "Step_" + pip.steps.Length;
            var IDNextStep = ((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? "");
            var oldStep = pip.steps.FirstOrDefault(ii => ii.IDPreviousStep == IDNextStep);
            var newStep = new Step() { owner = pip, IDStep = stepName, IDPreviousStep = ((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? "") };
            List<Step> steps = pip.steps.ToList();
            steps.Add(newStep);
            pip.steps = steps.ToArray();
            (selectedNode == null ? treeView1.Nodes : selectedNode.Nodes).Add(new TreeNode(stepName) { ContextMenuStrip = this.contextMenuStrip1, Tag = newStep });
            if (!string.IsNullOrEmpty(IDNextStep) && oldStep != null)
            {
                oldStep.IDPreviousStep = newStep.IDStep;
            }
            treeView1.ExpandAll();
        }


        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? ""));

        }
        public static T DeepCopyJSON<T>(T input)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var jsonString = JsonConvert.SerializeObject(input, settings); // new Newtonsoft.Json.JsonSerializer().Serialize();//.Serialize(input,);
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
            // return Newtonsoft.Json.JsonSerializer.Deserialize<T>(jsonString);
        }
        public static T DeepCopyXML<T>(T input)
        {
            using var stream = new MemoryStream();

            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, input);
            stream.Position = 0;
            return (T)serializer.Deserialize(stream);
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var idStep = Clipboard.GetText();

            var copyStep = pip.steps.FirstOrDefault(ii => ii.IDStep == idStep);
            try
            {
                if (copyStep != null)
                {
                    var stepName = "Step_" + pip.steps.Length;
                    var IDNextStep = ((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? "");
                    var oldStep = pip.steps.FirstOrDefault(ii => ii.IDPreviousStep == IDNextStep);
                    var newStep = DeepCopyJSON<Step>(copyStep);
                    newStep.owner = pip;
                    newStep.IDStep = stepName;
                    newStep.IDPreviousStep = ((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? "");
                    // new Step() { owner = pip, IDStep = stepName, IDPreviousStep = ((selectedNode == null ? "" : (selectedNode.Tag as Step)?.IDStep) ?? "") };
                    List<Step> steps = pip.steps.ToList();
                    steps.Add(newStep);
                    pip.steps = steps.ToArray();
                    // newStep.filterCollection.AddRange(copyStep.filterCollection.Select(ii=>new Step.ItemFilter() {  condition=ii.condition, Name=ii.Name, outputFields=ii.outputFields.})

                    if (!string.IsNullOrEmpty(IDNextStep) && oldStep != null)
                    {
                        oldStep.IDPreviousStep = newStep.IDStep;
                    }
                    (selectedNode == null ? treeView1.Nodes : selectedNode.Nodes).Add(new TreeNode(stepName) { ContextMenuStrip = this.contextMenuStrip1, Tag = newStep });
                    treeView1.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pip.steps.First().ireceiver.MocBody = comboBoxAllMocks.SelectedItem?.ToString();
        }
    }
}
