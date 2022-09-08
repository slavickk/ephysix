using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserLibrary;

namespace TestJsonRazbor
{
    public partial class FormTestPipeline : Form
    {
        public FormTestPipeline()
        {
            InitializeComponent();
        }
        int lastIndex = -1;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                if (index >= 0)
                {
                    var step = pip.steps[index];
                    if (step.receiver == null)
                    {
                        checkBoxMockReceiver.Enabled = checkBoxMockReceiver.Checked = false;
                    }
                    else
                    {
                        checkBoxMockReceiver.Enabled = true;
                        checkBoxMockReceiver.Checked = step.receiver.MocMode;
                    }
                    if (step.sender == null)
                    {
                        checkBoxSender.Enabled = checkBoxSender.Checked = false;
                    }
                    else
                    {
                        checkBoxSender.Enabled = true;
                        checkBoxSender.Checked = step.sender.MocMode;
                    }
                }
            }
        }
        public Pipeline pip;
        private void FormTestPipeline_Load(object sender, EventArgs e)
        {
            foreach (Step step in pip.steps)
            {
                listView1.Items.Add(new ListViewItem(new string[] { step.IDStep, (step.receiver == null) ? "" : step.receiver.MocMode.ToString(), (step.sender == null) ? "" : step.sender.MocMode.ToString() }));
            }
            listView1.SelectedIndices.Add(0);
        }

        private void checkBoxMockReceiver_CheckedChanged(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].receiver != null)
            {
                pip.steps[index].receiver.MocMode = checkBoxMockReceiver.Checked;
                listView1.Items[index].SubItems[1].Text = pip.steps[index].receiver.MocMode.ToString();
            }
        }

        private void checkBoxSender_CheckedChanged(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].sender!= null)
            {
                pip.steps[index].sender.MocMode = checkBoxSender.Checked;
                listView1.Items[index].SubItems[2].Text = pip.steps[index].sender.MocMode.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task taskA = Task.Run(async () =>
            {

                await pip.run();

                if(pip.lastExecutedEl!= null)
                {
                    var ex=pip.lastExecutedEl;
                    while (ex.ancestor != null)
                        ex = ex.ancestor;
                    foreach (var step in pip.steps)
                        if (step.receiver != null)
                            step.receiver.MocBody = ex.toJSON();
                }
                MessageBox.Show("Execution ended!");
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].receiver != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pip.steps[index].receiver.MocFile = openFileDialog1.FileName;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            if (index >= 0 && pip.steps[index].sender != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pip.steps[index].sender.MocFile = openFileDialog1.FileName;
                }
            }

        }
    }
}
