using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class FormSelectField : Form
    {
        int indexSearch = 0;
        public FormSelectField()
        {
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
            else
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = true;

        }

        int index_list;
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
/*            index_list = listBox1.SelectedIndex;
            if (index_list >= 0)
            {
                textBoxFieldName.Text = fields[index_list].outputPath;
                button10.Enabled = button11.Enabled = true;
            }
            else
            {
                button10.Enabled = button11.Enabled = false;
                textBoxFieldName.Text = textBoxFalueFieldSearchValue.Text = textBoxValueFieldSearch.Text = textBoxValueFieldPath.Text = "";

            }
*/
        }

        private void button10_Click(object sender, EventArgs e)
        {
/*            if (index_list >= 0)
            {
                listBox1.Items[index_list] = fields[index_list];
                listBox1.Refresh();
            }*/

        }

        private void button11_Click(object sender, EventArgs e)
        {
/*            if (index_list >= 0)
            {
                fields.RemoveAt(index_list);
                listBox1.Items.RemoveAt(index_list);

            }*/
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                textBoxFilterValue.Enabled = false;
//                this.tabPage2.Focus();
            }
            else
            {
                textBoxFilterValue.Enabled = true;

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
            }
            else
            {
                panel3.Visible = true;
                panel4.Visible = false;

            }

        }

        private void comboBoxFoundedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
        /*    if (comboBoxFoundedFiles.SelectedIndex >= 0)
            {
                treeView1.Nodes[0].Nodes.Clear();
                indexSearch = 0;
                lastFile = comboBoxFoundedFiles.SelectedItem.ToString();
                ParseInput(lastFile);
                found();

            }*/
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*
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
                                if (pars.canRazbor(line, rootEl, list))
                                    break;
                        }

                        foreach (var it in list)
                        {
                            if (it.childs.Count == 0 && it.Value != null)
                                sw.WriteLine(it.path + ";" + it.Value.ToString().Replace("\n", "").Replace("\r", ""));

                            //                                it.Value
                        }

                    }
                }
            }
            //            this.Text = "founded " + count + "files";
            return;






            if (openFileDialog2.ShowDialog() == DialogResult.OK)
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
                    pip.steps[0].sender = new DemoSender(treeView2);

                    int cycle = 1000;
                    ConditionFilter.isNew = true;
                    DemoSender.noDraw = true;
                    DateTime time1 = DateTime.Now;
                    for (int i = 0; i < cycle; i++)
                    {
                        pip.run().GetAwaiter().GetResult();
                    }
                    var milli = (DateTime.Now - time1).TotalMilliseconds;
                    int index = this.Text.IndexOf("::");
                    string text = ":: exec " + treeView2.Nodes[0].Nodes.Count;
                    if (index < 0)
                        this.Text += text;
                    else
                        this.Text = this.Text.Substring(0, index) + this.Text;
                    treeView2.ExpandAll();
                }
                catch (Exception e78)
                {
                    MessageBox.Show(e78.ToString());
                }
            }
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxValueFieldSearch.Text = Clipboard.GetText();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                found();
            }
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
            if (node.Text.Contains(searhedText))
                if (index > indexSearch)
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

        string lastFile = "";
        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lastFile = openFileDialog1.FileName;
                treeView1.Nodes[0].Nodes.Clear();
                ParseInput(lastFile);
            }



        }
        private void ParseInput(string file_name)
        {
            int ind = 0;
            using (StreamReader sr = new StreamReader(file_name))
            {
                AbstrParser.UniEl rootEl = AbstrParser.CreateNode(null, list, "Item");
                var line = sr.ReadToEnd();
                if (line != "")
                {
                    foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor(line, rootEl, list))
                            break;
                }

            }
        }
        List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
        TreeDrawerFactory drawFactory;

        private void button8_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Serialize(buildExtractor(), saveFileDialog1.FileName);
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

        private void FormSelectField_Load(object sender, EventArgs e)
        {
            drawFactory = new TreeDrawerFactory(treeView1);

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
    }
}