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
            RefreshScenarious();

        }

        private void RefreshScenarious()
        {
            comboBox1.Items.Clear();
            comboBoxStep.Items.Clear();
            foreach (var scen in pip.scenarios)
                comboBox1.Items.Add(scen);
            foreach (var step in pip.steps)
                comboBoxStep.Items.Add(step.IDStep);
        }

        Scenario currentScenarion= null;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                var scen = comboBox1.SelectedItem as Scenario;

                currentScenarion = scen;
                RefreshScenario(currentScenarion);

            }
        }

        private void RefreshScenario(Scenario scen)
        {
            textBox1.Text = currentScenarion.Description;
            listView1.Items.Clear();
            foreach (var it in scen.mocs)
            {
//???                listView1.Items.Add(new ListViewItem(new string[] { it.IDStep, (it.isMocReceiverEnabled ? "Y" : "N"), it.MocFileReceiver, (it.isMocSenderEnabled ? "Y" : "N"), it.MocFileSender }));
            }
        }
        int currentListItemSelection = -1;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count>0)
            {
                SaveLastMocs();
                int index = listView1.SelectedIndices[0];
                currentListItemSelection= index;
                string[] arr = new string[comboBoxStep.Items.Count];
                comboBoxStep.Items.CopyTo(arr, 0);
                comboBoxStep.SelectedIndex = arr.ToList().IndexOf(currentScenarion.mocs[index].IDStep);
//                checkBoxRec.Checked = currentScenarion.mocs[index].isMocReceiverEnabled;
              //  checkBoxSendEnable.Checked = currentScenarion.mocs[index].isMocSenderEnabled;
                textBox2.Text = currentScenarion.mocs[index].MocFileReceiver;
                textBox3.Text = currentScenarion.mocs[index].MocFileResponce;
            }
        }

        private void SaveLastMocs()
        {
            if (currentListItemSelection >= 0)
            {
                var it = currentScenarion.mocs[currentListItemSelection];
                it.IDStep = comboBoxStep.SelectedItem.ToString();
//                it.isMocReceiverEnabled = checkBoxRec.Checked;//
//                it.isMocSenderEnabled = checkBoxSendEnable.Checked;
                it.MocFileReceiver = textBox2.Text;
                it.MocFileResponce = textBox3.Text;
                listView1.Items[currentListItemSelection] = new ListViewItem(new string[] { it.IDStep, (!string.IsNullOrEmpty(it.MocFileReceiver) ? "Y" : "N"), it.MocFileReceiver, (!string.IsNullOrEmpty(it.MocFileResponce) ? "Y" : "N"), it.MocFileResponce });




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
            SaveLastMocs();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Add
            currentScenarion = new Scenario() { mocs = pip.steps.Select(ii => new Scenario.Item() { IDStep = ii.IDStep }).ToList(), Description="new Scenario" };
            RefreshScenario(currentScenarion);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
           // pip.scenarios.Remove(currentScenarion);
            RefreshScenarious();
        }
    }
}
