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
