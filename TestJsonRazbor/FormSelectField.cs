using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class FormSelectField : Form
    {
        public Step.ItemFilter itemFilter = new Step.ItemFilter() { outputFields = new List<OutputValue>() };
        public Step currentStep;
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
            if (indexTab(). comboBoxTypeCompare.SelectedIndex != 0)
            {
                indexTab().textBoxFilterValue.Visible = indexTab().label3.Visible = false;
                //                this.tabPage2.Focus();
            }
            else
            {
                indexTab().textBoxFilterValue.Visible = indexTab().label3.Visible = true;

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            indexTab().textBoxFilterFieldPath.Text = Clipboard.GetText();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBoxAddFieldPath.Text = Clipboard.GetText();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    panel1.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = true;
                    break;
                case 1:
                    panel1.Visible = false;
                    panel3.Visible = true;
                    panel4.Visible = false;
                    break;
                case 2:
                    panel1.Visible = true;
                    panel3.Visible = false;
                    panel4.Visible = false;

                    var type_frm = GetTypeOfForm();
                    
                    if (type_frm != null)
                    {
                        buttonSelectTemplate.Text = "Configure";
                    }

                    // Ищем  форму , которая обрабатывает Sendera

                    break;
                default:
                    break;
            }

        }

        private Type GetTypeOfForm()
        {
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.CustomAttributes.Count(ii => ii.AttributeType == typeof(GUIAttribute) /*&& ii.ConstructorArguments[0].ArgumentType == currentStep.sender.GetType()*/) > 0);
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
/*            var st = new string[] { "aa", "bb", "cc" };
            var sb = new StringBuilder();
            st.ToList().ForEach(s => sb.Append(s + ";"));
*/
            try
            {
                var el = fillOutput();
                //                itemFilter.

                AbstrParser.UniEl outRoot=null;
                var oldOutList = itemFilter.outputFields;
                int index=currentStep.converters.IndexOf(itemFilter);


                
                for (int i = 0; i <= index; i++)
                {
                    var filt = currentStep.converters[i];
//                    foreach (var filt in currentStep.filters)
                    {
                        AbstrParser.UniEl rEl1 = null;
                        foreach (var item1 in filt.filter.filter(list,ref rEl1))

                            filt.exec(item1, ref outRoot);
                    }
                }



                if (comboBoxTypeTest.SelectedIndex == 2)
                {
                    itemFilter.outputFields = new List<OutputValue>();
                    itemFilter.outputFields.Add(el);
                }

                //                itemFilter.outputFields.Add(el);
                AbstrParser.UniEl rEl = null;

                if (comboBoxTypeTest.SelectedIndex >0 &&   itemFilter.filter.filter(list,ref rEl).Count()  >0)
                itemFilter.exec(list[0], ref outRoot);
                treeView2.Nodes.Clear();
                TreeDrawerFactory fac = new TreeDrawerFactory(treeView2,true);

                if(outRoot != null)
                    redrawNode(fac, outRoot, null);
//                if (comboBoxTypeTest.SelectedIndex == 2)
                    itemFilter.outputFields = oldOutList;

                    //                itemFilter.outputFields.Remove(el);
                    treeView2.ExpandAll();
            }
            catch (Exception e56)
            {
                MessageBox.Show(e56.ToString());
            }

//            listBox1.Items.Contains
        }

        void redrawNode(TreeDrawerFactory fac,AbstrParser.UniEl el, AbstrParser.UniEl ancestor)
        {
            fac.Create(el, ancestor);
            foreach (var ch in el.childs)
                redrawNode(fac,ch, el);
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 1)
            {
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
                textBoxScript.Visible = true;
                checkBox2.Visible = false;
            }
            else
            {
                checkBox2.Visible = true;
                textBoxScript.Visible = false;
                if (checkBox2.Checked == false)
                    label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
                else
                    label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = true;

            }
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
                list.Clear();
                lastFile = openFileDialog1.FileName;
                treeView1.Nodes[0].Nodes.Clear();
                ParseInput(ReadFile(lastFile), new string[] { "Item" });
            }



        }

        string ReadFile(string file_name)
        {
            if (!File.Exists(file_name))
                return "";
            using (StreamReader sr = new StreamReader(file_name))
            {
                return sr.ReadToEnd();

            }
        }
            private void ParseInput(string line , string[] paths  )
        {
            int ind = 0;
//            using (StreamReader sr = new StreamReader(file_name))
            {
                AbstrParser.UniEl ancestor = null;
                AbstrParser.UniEl rootEl= null;
                if (list.Count > 0)
                    ancestor = list[0];
                foreach (var path in paths)
                {
                    if (ancestor == null || ancestor.Name != path)
                    {
                        if (ancestor != null)
                            rootEl = ancestor.childs.FirstOrDefault(ii => ii.Name == path);
                        if (rootEl == null)
                            rootEl = AbstrParser.CreateNode(ancestor, list, path);
                        ancestor = rootEl;
                    }
                }
  //              var line = sr.ReadToEnd();
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


        List<UserControlCondition> ctrlConditions= new List<UserControlCondition>();
        AndOrFilter.Action mainAction;

        private void FormSelectField_Load(object sender, EventArgs e)
        {
            //            userControlCondition1.action += UserControlCondition1_action;
            textBoxNameFilter.Text = itemFilter.Name;


            toolTip1.SetToolTip(this.textBoxValueFieldSearch, "Possible use of wildcard character (*) instead of host name in any part of the path");
            drawFactory = new TreeDrawerFactory(treeView1);
            /*            var pip=Pipeline.load();
                        itemFilter=  pip.steps[0].filters[0];*/
            var arrs = Enum.GetValues(typeof(ConstantValue.TypeObject));
            foreach (var ar in arrs)
                comboBoxTypeConvert.Items.Add(ar);
            comboBoxTypeConvert.SelectedIndex = 0;
            bool one = true;
            if (itemFilter.filter is ConditionFilter)
            {
                UserControlCondition ctrl = new UserControlCondition(false);
                ctrlConditions.Add(ctrl);
                FillCtrl(ctrl, true, AndOrFilter.Action.OR, itemFilter.filter as ConditionFilter);
            } else
            {
                var filt = itemFilter.filter as AndOrFilter;
                mainAction = filt.action;
                foreach(var filt2 in filt.filters)
                {
                    UserControlCondition ctrl = new UserControlCondition(true);
                    ctrlConditions.Add(ctrl);
                    FillCtrl(ctrl, false, AndOrFilter.Action.AND,filt2 as ConditionFilter);

                }
            }

            foreach (var item in itemFilter.outputFields)
                listBox1.Items.Add(item);
            if (currentStep != null)
            {
                bool first = true;

                FillReceiverMocs(first, currentStep);
            }
            foreach (var item in Assembly.GetAssembly(typeof(AliasProducer)).GetTypes().Where(t => t.IsAssignableTo(typeof(AliasProducer)) && !t.IsAbstract))
                comboBoxTypeAlias.Items.Add(item);
            if (1 == 1)
            {
               var ex = currentStep.sender.getTemplate("");
                if (ex != "")
                {

                    treeView2.Nodes.Clear();
                    TreeDrawerFactory fac = new TreeDrawerFactory(treeView2, true);
                    fac.isPale = true;
                    redrawNode(fac, AbstrParser.ParseString(ex), null);
                    treeView2.ExpandAll();
                }
            }
        }

        private void FillCtrl(UserControlCondition condCtrl,bool alone, AndOrFilter.Action action,ConditionFilter flt= null)
        {
            condCtrl.Location = new System.Drawing.Point(3, 3);
            condCtrl.Name = "userControlCondition1";
            condCtrl.Size = new System.Drawing.Size(1616, 135);
            condCtrl.TabIndex = 0;

            condCtrl.action += UserControlCondition1_action;
            condCtrl.Dock = DockStyle.Fill;
            condCtrl.mainAction = mainAction;
            if(!alone && !tabControl1.Visible)
            {
                while (this.tabControl1.TabPages.Count > 0)
                    this.tabControl1.TabPages.RemoveAt(0);
                if (ctrlConditions.First() != condCtrl)
                {
                    ctrlConditions.First().CanDel = true;
                    ctrlConditions.First().mainAction = mainAction;
                    this.groupBox1.Controls.Remove(ctrlConditions.First());
                    this.tabControl1.TabPages.Add(new TabPage() { Text = Enum.GetName(typeof(AndOrFilter.Action), action) });
                    tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(ctrlConditions.First());
                }
            }
            if (alone)
            {
//                panel3.Controls.Remove(tabControl1);
                tabControl1.Visible = false;
                this.groupBox1.Controls.Add(condCtrl);
            }
            else
            {
                tabControl1.Visible = true;
                this.tabControl1.TabPages.Add(new TabPage() { Text = Enum.GetName(typeof(AndOrFilter.Action), action) });
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(condCtrl);
            }

            if (flt != null)
            {
                condCtrl.textBoxFilterFieldPath.Text = flt.conditionPath;
                if (flt.conditionCalcer is ComparerForValue)
                {
                    condCtrl.comboBoxTypeCompare.SelectedIndex = 0;
                    condCtrl.textBoxFilterValue.Text = (flt.conditionCalcer as ComparerForValue).value_for_compare;
                }
                else
                {
                    condCtrl.comboBoxTypeCompare.SelectedIndex = 1;

                }
            }
//            condCtrl.ResumeLayout(false);

        }

        private void UserControlCondition1_action(AndOrFilter.Action action)
        {
            if (action == AndOrFilter.Action.OR || action == AndOrFilter.Action.AND)
            {
                if (ctrlConditions.Count == 1)
                    mainAction = action;
                UserControlCondition condCtrl = new UserControlCondition(true);
                ctrlConditions.Add(condCtrl);
                FillCtrl(condCtrl, false, action);
                tabControl1.SelectedIndex=tabControl1.TabPages.Count - 1;
/*                tabControl1.TabPages.Add(new TabPage() { Text = Enum.GetName(typeof(UserControlCondition.Action), action) });
                UserControlCondition cntr = new UserControlCondition(true);
                cntr.Dock = DockStyle.Fill;
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(cntr);*/
            }
            if(action == AndOrFilter.Action.DEL)
            {
                ctrlConditions.Remove(ctrlConditions[tabControl1.SelectedIndex]);
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
        }

        private void FillReceiverMocs(bool first,Step currentStep1)
        {
            if (currentStep1.owner.tempMocData != "")
            {
                ParseInput(currentStep1.owner.tempMocData, new string[] { currentStep1.owner.steps.First(ii=>(ii.IDPreviousStep == null || ii.IDPreviousStep =="")).IDStep });
            }
            else
            {
                if (currentStep1.receiver != null && (currentStep1.receiver.MocFile != null || (currentStep1.receiver.MocBody ?? "") != ""))
                {

                    ParseInput(((currentStep1.receiver.MocBody ?? "") != "") ? currentStep1.receiver.MocBody : ReadFile(currentStep1.receiver.MocFile), new string[] { currentStep1.IDStep, "Rec" });
                }

                if (currentStep1.sender != null && !first && (currentStep1.sender.MocFile != null || (currentStep1.sender.MocBody ?? "") != ""))
                {
                    ParseInput(((currentStep1.sender.MocBody ?? "") != "") ? currentStep1.sender.MocBody : ReadFile(currentStep1.sender.MocFile), new string[] { currentStep1.IDStep, "Send" });
                }
                first = false;
                if (currentStep1.IDPreviousStep != null && currentStep1.IDPreviousStep != "")
                {

                    FillReceiverMocs(first, currentStep1.owner.steps.First(ii => ii.IDStep == currentStep1.IDPreviousStep));
                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            fillFilter();
            try
            {
                AbstrParser.UniEl rEl = null;

                var res = itemFilter.filter.filter(list,ref rEl);
                MessageBox.Show("found " + res.ToList().Count + " item");
            } 
            catch(Exception e88 )
            {
                MessageBox.Show(e88.ToString());
            }
        }


//        public List<OutputValue> outputFields = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };
        OutputValue fillOutput()
        {
            ConverterOutput converter = null;
//            if (checkBoxHash.Checked)
             if(0==1/*radioButtonSimple.Checked*/)
            {

                converter = new HashOutput() { hashConverter = new Hasher(), aliasProducer = Activator.CreateInstance(comboBoxTypeAlias.SelectedItem as Type) as AliasProducer };
            }
            if (0==1/*radioButtonCrypto.Checked*/)
            {

                converter = new HashOutput() { hashConverter = new CryptoHash(), aliasProducer = Activator.CreateInstance(comboBoxTypeAlias.SelectedItem as Type) as AliasProducer };
            }
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    return new ConstantValue() {viewAsJsonString= checkBoxPackToJson.Checked, converter = converter, outputPath = textBoxFieldName.Text, isUniqOutputPath = checkBoxIsUniq.Checked, getNodeNameOnly = checkBoxNameOnly.Checked, typeConvert = (ConstantValue.TypeObject)comboBoxTypeConvert.SelectedItem, Value = ConstantValue.ConvertFromType(textBoxConstant.Text, (ConstantValue.TypeObject)comboBoxTypeConvert.SelectedItem) };
                case 1:
                    if (comboBox2.SelectedIndex != 1)
                        return new ExtractFromInputValue() { viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = textBoxFieldName.Text,isUniqOutputPath=checkBoxIsUniq.Checked, getNodeNameOnly=checkBoxNameOnly.Checked,returnOnlyFirstRow=checkBoxReturnFirstField.Checked, copyChildsOnly=checkBoxCopyChildOnly.Checked,  conditionPath = textBoxValueFieldSearch.Text, conditionCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text))), valuePath = (checkBox2.Checked ? textBoxAddFieldPath.Text : "") };
                    else
                        return new ExtractFromInputValueWithScript() {viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = textBoxFieldName.Text, isUniqOutputPath = checkBoxIsUniq.Checked, getNodeNameOnly = checkBoxNameOnly.Checked, conditionPath = textBoxValueFieldSearch.Text, conditionCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text))), ScriptBody = textBoxScript.Text };
                case 2:
                    return new TemplateOutputValue() { viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = textBoxFieldName.Text, getNodeNameOnly = checkBoxNameOnly.Checked, isUniqOutputPath = checkBoxIsUniq.Checked, templateBody=textBoxTemplate.Text };  
                    break;
                default:
                    break;
            }
                        return null;
        }

        UserControlCondition indexTab()
        {
            if(ctrlConditions.Count==1)
                return ctrlConditions[0];
            return ctrlConditions[tabControl1.SelectedIndex];
        }

        private void fillFilter()
        {
            itemFilter.Name=textBoxNameFilter.Text; 
           // ComparerV compar;
            if (ctrlConditions.Count == 1)
            {
                var ctrl = indexTab();
                itemFilter.filter  = FillElementaryFilter(ctrl);
            }
            else
            {
                var flt  = new AndOrFilter();
                itemFilter.filter = flt;
                flt.action = mainAction;
                List<Filter> filters= new List<Filter>();
                foreach (var ctrl in ctrlConditions)
                {
                    filters.Add(FillElementaryFilter(ctrl));
                }
                flt.filters = filters.ToArray();
            }
        }

        private Filter FillElementaryFilter(UserControlCondition ctrl)
        {
            ComparerV compar;
            if (ctrl.comboBoxTypeCompare.SelectedIndex == 0)
                compar = new ComparerForValue() { value_for_compare = ctrl.textBoxFilterValue.Text, isNegative = ctrl.checkBoxNegative.Checked };
            else
            if (ctrl.comboBoxTypeCompare.SelectedIndex == 3)
            {
                string[] values = new string[ctrl.comboBoxListValues.Items.Count];
                for(int i = 0; i < values.Length; i++)
                    values[i] = ctrl.comboBoxListValues.Items[i].ToString();   
                compar = new ComparerForValueList() { values_for_compare = values, isNegative = ctrl.checkBoxNegative.Checked };
            }
            else
                compar = new ComparerAlwaysTrue();
            return new ConditionFilter() { conditionPath = ctrl.textBoxFilterFieldPath.Text, conditionCalcer = compar };
//            return compar;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OutputValue val = listBox1.SelectedItem as OutputValue;
           // radioButtonNoHash.Checked = true;
            comboBoxTypeAlias.SelectedIndex = -1;
            if (val != null)
            {
                if (val.converter != null)
                { 
                   if(val.converter.GetType().IsAssignableTo(typeof(HashOutput)))
                    {
                        var conv =val.converter as HashOutput;  
                       /* if(conv.hashConverter.GetType() == typeof(Hasher))
                            radioButtonSimple.Checked = true;
                        else
                            radioButtonCrypto.Checked = true;*/
                        if (conv.aliasProducer != null)
                        {
                            for (int i = 0; i < comboBoxTypeAlias.Items.Count; i++)
                            {
                                if ((comboBoxTypeAlias.Items[i] as Type).Name == conv.aliasProducer.GetType().Name)
                                {
                                    comboBoxTypeAlias.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
//                        comboBoxTypeAlias.SelectedItem = conv.hashConverter.GetType();
                    }
                    checkBoxHash.Checked = true;
                }
//                    checkBoxHash.Checked = true;
                else
/*                    radioButtonNoHash.Checked = true;*/
                    checkBoxHash.Checked = false;

                buttonDel.Enabled = buttonMod.Enabled = true;
                textBoxFieldName.Text = val.outputPath;
                checkBoxIsUniq.Checked = val.isUniqOutputPath;
                checkBoxNameOnly.Checked = val.getNodeNameOnly;
                checkBoxPackToJson.Checked=val.viewAsJsonString;
                if (val is ConstantValue)
                {
                    comboBox3.SelectedIndex = 0;
                    textBoxConstant.Text=(val as ConstantValue).Value.ToString();
                    
                } else
                if (val is TemplateOutputValue)
                {
                    comboBox3.SelectedIndex = 2;
                    textBoxTemplate.Text=(val as TemplateOutputValue).templateBody;

                }
                else
                {

                    comboBox3.SelectedIndex = 1;


                    ExtractFromInputValue val1 = val as ExtractFromInputValue;
                    textBoxValueFieldSearch.Text = val1.conditionPath;
                    checkBoxCopyChildOnly.Checked = val1.copyChildsOnly;
                    checkBoxReturnFirstField.Checked = val1.returnOnlyFirstRow;
                    if (val1.conditionCalcer != null)
                    {
                        ComparerForValue cv = val1.conditionCalcer as ComparerForValue;
                        textBoxFalueFieldSearchValue.Text = cv.value_for_compare;
                    } else
                        textBoxFalueFieldSearchValue.Text = "";

                    if (val1 is ExtractFromInputValueWithScript)
                    {
                        comboBox2.SelectedIndex = 1;
                        textBoxScript.Text = (val1 as ExtractFromInputValueWithScript).ScriptBody.Replace("\n\n", "\n").Replace("\n","\r\n");
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 0;
                        if (val1.valuePath != "")
                        {
                            checkBox2.Checked = true;
                            textBoxAddFieldPath.Text = val1.valuePath;
                        }
                        else
                            checkBox2.Checked = false;
                    }
                    //                    , conditionCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text))), valuePath = ""/*((textBoxValueFieldPath.Text == "") ? "" : textBoxValueFieldPath.Text)*/ };
                }

            } else
                buttonDel.Enabled = buttonMod.Enabled = false;

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                itemFilter.outputFields.Add(fillOutput());
                redrawOutput();
            }
            catch(Exception e77)
            {
                MessageBox.Show(e77.ToString());
            }
        }

        private void redrawOutput()
        {
            listBox1.Items.Clear();
            foreach (var it in itemFilter.outputFields.Where(ii=>ii !=null))
                listBox1.Items.Add(it);
        }

        private void buttonMod_Click(object sender, EventArgs e)
        {
            try
            {
                itemFilter.outputFields[listBox1.SelectedIndex] = fillOutput();
                redrawOutput();
            } 
            catch(Exception e77)
            {
                MessageBox.Show(e77.ToString());
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            itemFilter.outputFields.RemoveAt(listBox1.SelectedIndex);
            redrawOutput();
        }

        private void button3_Click(object sender, EventArgs e)
        {
             fillFilter();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           textBoxFieldName.Text = Clipboard.GetText();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if(index>0)
            {
                var el = itemFilter.outputFields[index];
                itemFilter.outputFields.Insert(index - 1, el);
                itemFilter.outputFields.RemoveAt(index + 1);
                redrawOutput();
            }
        }

        private void checkBoxHash_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxFilterFieldPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFilterValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Nodes.Count==0)
            {
                Clipboard.SetText(e.Node.Text);
            }
        }

        private void buttonSelectTemplate_Click(object sender, EventArgs e)
        {
            var type_frm = GetTypeOfForm();

            if (type_frm != null)
            {

                Form frm = Activator.CreateInstance(type_frm, new object[] { currentStep.sender as Sender }) as Form;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var ss=(frm as SenderDataExchanger).getContent();
                    currentStep.sender.setTemplate("asd", ss);

                }
            }
            else
            {

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        textBoxTemplate.Text = sr.ReadToEnd();
                    }
                }
            }
        }

        private void textBoxFieldName_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTypeAlias_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}