/******************************************************************
 * File: FormYamlCode.cs
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

namespace TestJsonRazbor
{
    public partial class FormYamlCode : Form
    {
        string text;
        string textDialogComplete;
        public FormYamlCode(string text1,string textDialogComplete1="")
        {
            text = text1;
            textDialogComplete = textDialogComplete1;
            InitializeComponent();
        }

        private void FormYamlCode_Load(object sender, EventArgs e)
        {
            textBox1.Text = text;
            if(textDialogComplete!= "")
            {
                button1.Text= textDialogComplete;
                button1.Visible = true; 
            }
        }
        public string  Body
        {
            get { return textBox1.Text; }
        }
    }
}
