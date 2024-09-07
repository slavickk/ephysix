using DotLiquid;
using ParserLibrary.PlantUmlGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static ScintillaNET.Style;

namespace TestJsonRazbor
{
    public partial class FormTestShablon : Form
    {
        public FormTestShablon()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox3.Text = ShablonizeHelper.Shablonize(textBox2.Text, textBox1.Text);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
