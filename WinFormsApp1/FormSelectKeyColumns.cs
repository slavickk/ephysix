/******************************************************************
 * File: FormSelectKeyColumns.cs
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
using System.Text;
using System.Windows.Forms;

namespace WinFormsETLPackagedCreator
{
    public partial class FormSelectKeyColumns : Form
    {
        string[] columns;
        public FormSelectKeyColumns(string[] columns)
        {
            this.columns = columns;
            InitializeComponent();
        }
        public List<string> keyColumns= new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {

            foreach(var check in checkedListBox1.CheckedItems)
            {
                keyColumns.Add(check.ToString());
            }
        }

        private void FormSelectKeyColumns_Load(object sender, EventArgs e)
        {
            foreach(string column in columns)
            {
                checkedListBox1.Items.Add(column);
            }
        }
    }
}
