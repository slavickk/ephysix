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
