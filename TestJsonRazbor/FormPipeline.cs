using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
//using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ParserLibrary;

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
            var newStep = new Step() { owner=pip, IDStep = stepName, IDPreviousStep = ((selectedNode == null?"": (selectedNode.Tag as Step)?.IDStep)??"") };
            List<Step> steps = pip.steps.ToList();
            steps.Add(newStep);
            pip.steps = steps.ToArray();
            (selectedNode==null?treeView1.Nodes:selectedNode.Nodes).Add(new TreeNode(stepName) { ContextMenuStrip = this.contextMenuStrip1, Tag = newStep});
            treeView1.ExpandAll();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }
        TreeNode selectedNode;
        Step currentStep;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node;
            currentStep = e.Node.Tag as Step;
            if(currentStep != null)
            {
                textBoxIDStep.Text = currentStep.IDStep;
                textBoxIDPrevStep.Text = currentStep.IDPreviousStep;
                checkBox1.Checked = currentStep.isBridge;
                checkBoxHandleSendError.Checked = currentStep.isHandleSenderError;
                textBoxResponceStep.Text = currentStep.IDResponsedReceiverStep;
                if (currentStep.IDPreviousStep == "")
                    buttonSetupReceiver.Enabled = true;
                else
                    buttonSetupReceiver.Enabled = false;

                textBoxStepDescription.Text = currentStep.description;
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
            if (currentStep.converters == null)
                currentStep.converters = new List<Step.ItemFilter>();
            listBox1.Items.AddRange(currentStep.converters.ToArray());
        }

        Pipeline pip;
        string pipelinePath="";
        string fileNameStorageContext = "lastPipeline.inf";
        void saveStorageContext(string fileName)
        {
            using(StreamWriter sw = new StreamWriter(fileNameStorageContext))
            {
                sw.WriteLine(fileName);
                this.Text = fileName;
            }
        }

        void dd()
        {
            var targetDirectory = @"C:\\dd";
            List<string> ll = new List<string>();
            foreach(var file in Directory.GetFiles(targetDirectory).OrderBy(ii=> File.GetLastWriteTime(ii)))
            {
                ll.Add(file+" "+File.GetLastWriteTime(file).ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK"));
            }
        }

        private async void FormPipeline_Load(object sender, EventArgs e)
        {

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
                string fileName=Program.ExecutedPath;
                this.Text = fileName;
                if (File.Exists(fileName))
                    try
                    {
                        LoadYaml(fileName);
                    }
                    catch( Exception e67)
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
                    this.Text = fileName;
                    LoadYaml(fileName);
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
                pip = Pipeline.load(fileName);
                RefreshPipeline();

            }
        }

        private void RefreshPipeline()
        {
            treeView1.Nodes.Clear();
            listBox1.Items.Clear();
            textBoxPipelineDescription.Text = pip.pipelineDescription;


            buttonReceiverMoc.Enabled = buttonSenderMoc.Enabled = false;
            var prevStep = "";
            FillStep(pip, prevStep, treeView1.Nodes);
            if (treeView1.Nodes.Count > 0)
                selectedNode = treeView1.Nodes[0];
            else
                selectedNode = null;
        }

        private void FillStep(Pipeline pip, string prevStep,TreeNodeCollection col)
        {
            foreach (var step in pip.steps.Where(ii => ii.IDPreviousStep == prevStep))
            {
                var node = new TreeNode() { Text = step.IDStep, Tag = step ,ContextMenuStrip = this.contextMenuStrip1 };
                col.Add(node);
                FillStep(pip, step.IDStep,node.Nodes);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;
            ((TreeView)sender).SelectedNode = e.Node;
        }
        Receiver rec;
        private void button1_Click(object sender, EventArgs e)
        {
            FormTypeDefiner frm = new FormTypeDefiner() { tDefine = typeof(Receiver),tObject= (currentStep.receiver== null)?new PacketBeatReceiver(): currentStep.receiver };
            if(frm.ShowDialog() == DialogResult.OK)
            {
                SetReceiverObject(frm.tObject);
                currentStep.receiver = rec;
            }

        }

        private void SetReceiverObject(object tObject)
        {
            if (tObject != null)
            {
                this.buttonSetupReceiver.Text = "Receiver:" + tObject.GetType().Name;
                buttonTestReceiver.Enabled = true;
                rec = tObject as Receiver;
                buttonReceiverMoc.Enabled = true;
            } 
        }

        async Task proxyRec(string body,object Context)
        {
            string path = @"C:\D\Out";
            var fileName = "tic"+Path.GetRandomFileName();
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, fileName)))
            {
                sw.Write(body);

            }



        }
        private void buttonTestReceiver_Click(object sender, EventArgs e)
        {
            rec.stringReceived = proxyRec;
            Task taskA = Task.Run(async () =>
            {
                await rec.startInternal();
            });
        }
        Sender sender1;
        private void button3_Click(object sender, EventArgs e)
        {
            FormTypeDefiner frm = new FormTypeDefiner() { tDefine = typeof(Sender), tObject =((currentStep.sender== null)? new TICSender(): currentStep.sender) };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SetSenderObject(frm.tObject);
                currentStep.sender = sender1;

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
            if (currentStep != null && (rec.MocFile != ""||(rec.MocBody??"") != "" )&& withoutFilter== false)
            {
                Task taskA = Task.Run(async () =>
                {

                    string input;
                    if ((rec.MocBody ?? "") != "" )
                    { 
                        input = rec.MocBody;    
                    } else
                    using (StreamReader sr = new StreamReader(rec.MocFile))
                    {
                        input = sr.ReadToEnd();
                    }
                    DateTime time2 = DateTime.Now;
                    List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                    var rootElement = AbstrParser.CreateNode(null, list, currentStep.IDStep);
                    rootElement = AbstrParser.CreateNode(rootElement, list, "Rec");
                    var res = await currentStep.FilterInfo1(input, time2, list, rootElement);
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
                    var res = await sender1.send(sends);
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
                FormSelectField frm = new FormSelectField() { itemFilter = currentStep.converters[listBox1.SelectedIndex], currentStep= currentStep };
                frm.ShowDialog();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (currentStep != null)
            {
                Step.ItemFilter filter = new Step.ItemFilter();
                currentStep.converters.Add(filter);
                RedrawFilters();
            }

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (currentStep != null && listBox1.SelectedIndex>=0)
            {
                currentStep.converters.RemoveAt(listBox1.SelectedIndex);
                RedrawFilters();
            }

        }

        private void buttonSavePipeline_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pip.Save(saveFileDialog1.FileName);
                    saveStorageContext(saveFileDialog1.FileName);
                }
            }
            catch(Exception e77)
            {
                Console.WriteLine(e77.ToString());
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
                var el = currentStep.converters[index];
                currentStep.converters.Insert(index - 1, el);
                currentStep.converters.RemoveAt(index + 1);
//                redrawOutput();
            }

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            (new FormTestPipeline() { pip = pip }).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            currentStep.IDStep=textBoxIDStep.Text;
            currentStep.IDPreviousStep=textBoxIDPrevStep.Text;
            currentStep.isBridge = checkBox1.Checked;
            currentStep.isHandleSenderError=checkBoxHandleSendError.Checked;    
            currentStep.IDResponsedReceiverStep=textBoxResponceStep.Text ;
            currentStep.description=textBoxStepDescription.Text  ;
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
            var sw= new StringWriter();
            pip.Save(sw);
            sw.Flush();
            new FormYamlCode(sw.ToString()).ShowDialog();
//            MessageBox.Show(sw.ToString());
            sw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var frm = new FormYamlCode(currentStep.receiver.MocBody,"Set Moc");
            if(frm.ShowDialog() == DialogResult.OK)
            {
                currentStep.receiver.MocBody=frm.Body;
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadYaml(openFileDialog1.FileName);
                saveStorageContext(openFileDialog1.FileName);
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            pip = new Pipeline();
            this.Text = "new Pipeline";
            RefreshPipeline();
        }

        private async  void button3_Click_1(object sender, EventArgs e)
        {
            PostgresSender.Test();
            (new FormDMNView(null) {  xml=""}).ShowDialog();
            return;
            dynamic employee = new ExpandoObject();
            ((IDictionary<String, Object>)employee).Add("Age", 19);
            Console.WriteLine(employee.Age);
//            await DMNExecutorSender.execDmn(null);



            //            DMNExecutorSender.execDmn("");
            return;
             string json =@"
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
    }
}
