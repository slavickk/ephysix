/******************************************************************
 * File: FormSelectSynonymLink.cs
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

using ETL_DB_Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsETLPackagedCreator
{
    public partial class FormSelectSynonymLink : Form
    {
        public List<DBInterface.SourceTableItemAgg> list;
        string name;
        public FormSelectSynonymLink(List<DBInterface.SourceTableItemAgg> list,string name)
        {
            this.list = list;
            this.name = name;
            InitializeComponent();
        }

        private void FormSelectSynonymLink_Load(object sender, EventArgs e)
        {
            this.Text = $"Таблицы, связанные с {name}";
            checkedListBox1.Items.AddRange(list.ToArray());
        }


        private void buttonFinish_Click(object sender, EventArgs e)
        {
            list.Clear();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                list.Add((DBInterface.SourceTableItemAgg)item);
            }
        }
    }
}
