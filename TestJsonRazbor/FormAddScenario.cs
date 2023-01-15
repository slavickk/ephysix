using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class FormAddScenario : Form
    {
        Pipeline pip;
        public FormAddScenario(Pipeline pip)
        {
            this.pip = pip;
            InitializeComponent();
        }

        private void FormAddScenario_Load(object sender, EventArgs e)
        {
            foreach (var scen in pip.scenarios)
                comboBox1.Items.Add(scen);
            foreach(var step in pip.steps)
                comboBoxStep.Items.Add(step.IDStep);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                var scen = comboBox1.SelectedItem as Scenario;
                listView1.Items.Clear();
                foreach(var it in scen.mocs)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { it.IDStep,(it.isMocReceiverEnabled?"Y":"N"),it.MocFileReceiver, (it.isMocSenderEnabled ? "Y" : "N"), it.MocFileSender }));
                }

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count>0)
            {
                int index = listView1.SelectedIndices[0];
                string[] arr = new string[comboBoxStep.Items.Count];
                comboBoxStep.Items.CopyTo(arr, 0);
                comboBoxStep.SelectedIndex = arr.ToList().IndexOf(pip.scenarios[index]..IDStep);
                checkBoxRec.Checked = true;
            }
        }

        private void buttonAddRecMoc_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Save
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Add
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {

        }
    }
}
