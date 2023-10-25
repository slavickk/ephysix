using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ParserLibrary;
using UniElLib;

namespace TestJsonRazbor
{
    public partial class UserControlSwitch : UserControl
    {
        public List<ExtractFromInputValueWithSwitch.SwitchItem> switches = new List<ExtractFromInputValueWithSwitch.SwitchItem>();
        public UserControlSwitch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                var it = switches.FirstOrDefault(ii => ii.overwise);
                if (it == null)
                    switches.Add(new ExtractFromInputValueWithSwitch.SwitchItem() { overwise = true, Value = textBox2.Text });
                else
                    it.Value = textBox2.Text;
            }
            else
                switches.Add(new ExtractFromInputValueWithSwitch.SwitchItem() { overwise = false, Key = textBox1.Text, Value = textBox2.Text });
            redrawList();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        void redrawList()
        {
            listView1.Items.Clear();
            foreach(var item in  switches)
            {
                if(item.overwise)
                    listView1.Items.Add(new ListViewItem(new string[] { "overwise", item.Value }));
                else
                    listView1.Items.Add(new ListViewItem(new string[] { item.Key, item.Value }));

            }
        }
        private void UserControlSwitch_Load(object sender, EventArgs e)
        {
            if (switches.Count == 0)
                switches.Add(new ExtractFromInputValueWithSwitch.SwitchItem() { overwise = true });
            redrawList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked) 
            {
                var it=switches.FirstOrDefault(ii => ii.overwise);
                if(it  != null)
                {
                    it.Value = textBox2.Text;
                }
            } else
            {
                var it = switches.FirstOrDefault(ii => ii.Key==textBox1.Text);
                if (it != null)
                {
                    it.Value = textBox2.Text;
                }

            }
            redrawList();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count > 0)
            {
                var index = listView1.SelectedIndices[0];
                switches.RemoveAt(index);
                redrawList();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                var index = listView1.SelectedIndices[0];
                textBox1.Text=switches[index].Key;
                textBox2.Text = switches[index].Value;
                checkBox1.Checked = switches[index].overwise;
            }
        }
    }
}
