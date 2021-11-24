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
            var newStep = new Step() { owner=pip, IDStep = stepName, IDPreviousStep = (selectedNode == null?"": (selectedNode.Tag as Step).IDStep) };
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
        private void FormPipeline_Load(object sender, EventArgs e)
        {
            /*var pip1 = new Pipeline() { steps = new Step[] { new Step() { receiver = new TICReceiver() { Port = 4001 } } } };
            pip1.Save(@"c:\d\a2.yml");*/
            pip =  Pipeline.load(@"C:\D\Out\s3.yml");
            treeView1.Nodes.Clear();
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
            FormTypeDefiner frm = new FormTypeDefiner() { tDefine = typeof(Receiver),tObject=new PacketBeatReceiver() };
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
            FormTypeDefiner frm = new FormTypeDefiner() { tDefine = typeof(Sender), tObject = new TICSender() };
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
            if (currentStep != null && rec.MocFile != "" && withoutFilter== false)
            {
                Task taskA = Task.Run(async () =>
                {

                    string input;
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
                    pip.Save(saveFileDialog1.FileName);
            }
            catch(Exception e77)
            {
                Console.WriteLine(e77.ToString());
            }
        }

        private void buttonReceiverMoc_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                currentStep.receiver.MocFile = openFileDialog1.FileName;
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
            (new FormTestPipeline() { pip = pip }).ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
