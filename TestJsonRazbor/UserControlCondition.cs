/******************************************************************
 * File: UserControlCondition.cs
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

using UniElLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class UserControlCondition : UserControl
    {

        // public enum Action { OR,AND,DEL};
        public delegate void ExecAction(AndOrFilter.Action action);
        public event ExecAction action;

        bool canDel;

        public AndOrFilter.Action mainAction
        {
            set
            {
                if (value == AndOrFilter.Action.OR)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;// false;
                }
                else
                {
                    button1.Enabled = true;// false;
                    button2.Enabled = true;
                }
            }
        }
        public bool CanDel
        {
            set
            {
                canDel = value;
                button3.Visible = canDel;
            }
        }
        public UserControlCondition(bool canDel1)
        {
            InitializeComponent();
            canDel = canDel1;
        }
        public UserControlCondition()
        {
            InitializeComponent();
            //            canDel = canDel1;
        }

        private void comboBoxTypeCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTypeCompare.SelectedIndex != 0 && comboBoxTypeCompare.SelectedIndex != 3)
            {
                textBoxFilterValue.Visible = label3.Visible = comboBoxListValues.Visible = buttonAdd.Visible = buttonDel.Visible = false;

                //                this.tabPage2.Focus();
            }
            else
            {
                textBoxFilterValue.Visible = label3.Visible = true;
                if (comboBoxTypeCompare.SelectedIndex == 3)
                    comboBoxListValues.Visible = buttonAdd.Visible = buttonDel.Visible = true;
                else
                    comboBoxListValues.Visible = buttonAdd.Visible = buttonDel.Visible = false;


            }

        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            textBoxFilterFieldPath.Text = Clipboard.GetText();
        }

        private void textBoxFilterFieldPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserControlCondition_Load(object sender, EventArgs e)
        {
            if (comboBoxTypeCompare.SelectedItem == null)
                comboBoxTypeCompare.SelectedIndex = 0;
            if (canDel)
                button3.Visible = true;
            else
                button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            action?.Invoke(AndOrFilter.Action.OR);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            action?.Invoke(AndOrFilter.Action.AND);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            action?.Invoke(AndOrFilter.Action.DEL);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxFilterValue.Text = Clipboard.GetText();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            comboBoxListValues.Items.Add(textBoxFilterValue.Text);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (comboBoxListValues.SelectedIndex != -1)
                comboBoxListValues.Items.RemoveAt(comboBoxListValues.SelectedIndex);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxListValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxFilterValue.Text = comboBoxListValues.Items[comboBoxListValues.SelectedIndex].ToString();
        }

        private void buttonEQ_Click(object sender, EventArgs e)
        {
            action?.Invoke(AndOrFilter.Action.EQ);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            action?.Invoke(AndOrFilter.Action.NOT_EQ);

        }
    }
}
