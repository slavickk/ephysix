using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class FormConv : Form
    {
        public FormConv()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in Directory.GetFiles(this.folderBrowserDialog1.SelectedPath, "*.html"))
                {
                    string body;
                    using (StreamReader sr = new StreamReader(file))
                    {
                        body = sr.ReadToEnd();
                    }
                    using (StreamWriter sw = new StreamWriter(file)) { sw.Write(body.Replace("/Files/", "")); }
                }
            }
        }
    }
}
