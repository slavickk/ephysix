/******************************************************************
 * File: FormStreamFromJson.cs
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

//using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class FormStreamFromJson : Form
    {
        public FormStreamFromJson()
        {
            InitializeComponent();
        }

        public FormStreamSenderSetup.JsonStream retValue;
        private void button1_Click(object sender, EventArgs e)
        {
            retValue=JsonSerializer.Deserialize<FormStreamSenderSetup.JsonStream>(textBox1.Text);
        }
    }
}
