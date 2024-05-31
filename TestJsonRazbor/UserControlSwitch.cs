/******************************************************************
 * File: UserControlSwitch.cs
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ParserLibrary;
using UniElLib;

namespace TestJsonRazbor
{
    public partial class UserControlSwitch : UserControl
    {
        public List<ExtractFromInputValueWithSwitch.SwitchItem> switches
        {
            get => switches1;
            set
            {
                switches1 = value;
                redrawList();
            }
        }
        List<ExtractFromInputValueWithSwitch.SwitchItem> switches1 = new List<ExtractFromInputValueWithSwitch.SwitchItem>();
        public UserControlSwitch()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                var it = switches1.FirstOrDefault(ii => ii.overwise);
                if (it == null)
                    switches1.Add(new ExtractFromInputValueWithSwitch.SwitchItem() { overwise = true, Value = textBox2.Text });
                else
                    it.Value = textBox2.Text;
            }
            else
                switches1.Add(new ExtractFromInputValueWithSwitch.SwitchItem() { overwise = false, Key = textBox1.Text, Value = textBox2.Text });
            redrawList();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        void redrawList()
        {
            listView1.Items.Clear();
            foreach(var item in  switches1)
            {
                if(item.overwise)
                    listView1.Items.Add(new ListViewItem(new string[] { "overwise", item.Value }));
                else
                    listView1.Items.Add(new ListViewItem(new string[] { item.Key, item.Value }));

            }
        }
        private void UserControlSwitch_Load(object sender, EventArgs e)
        {
            if (switches1.Count == 0)
                switches1.Add(new ExtractFromInputValueWithSwitch.SwitchItem() { overwise = true });
            redrawList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked) 
            {
                var it=switches1.FirstOrDefault(ii => ii.overwise);
                if(it  != null)
                {
                    it.Value = textBox2.Text;
                }
            } else
            {
                var it = switches1.FirstOrDefault(ii => ii.Key==textBox1.Text);
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
                switches1.RemoveAt(index);
                redrawList();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                var index = listView1.SelectedIndices[0];
                textBox1.Text=switches1[index].Key;
                textBox2.Text = switches1[index].Value;
                checkBox1.Checked = switches1[index].overwise;
            }
        }
    }
}
