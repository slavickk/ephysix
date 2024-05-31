/******************************************************************
 * File: FormAddVariable.cs
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class FormAddVariable : Form
    {
        public FormAddVariable()
        {
            InitializeComponent();
        }
        public string VariableName;
        public string VariableDescription;
        public string VariableType="String";
        public string VariableDefaultValue="Empty";

        private void button4_Click(object sender, EventArgs e)
        {
            VariableName = textBox6.Text;
            VariableDescription = textBox7.Text;
            VariableDefaultValue = textBoxDefaultValue.Text;
            VariableType=comboBoxVarType.Text;
        }

        private void FormAddVariable_Load(object sender, EventArgs e)
        {
            textBox6.Text=VariableName;
             textBox7.Text= VariableDescription;
            textBoxDefaultValue.Text=VariableDefaultValue;
            comboBoxVarType.SelectedItem=VariableType;

        }
    }
}
