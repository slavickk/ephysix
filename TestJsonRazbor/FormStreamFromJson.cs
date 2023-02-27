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
