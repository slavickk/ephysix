using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsETLPackagedCreator
{
    public partial class FormViewDMNResults : Form
    {
        public FormViewDMNResults()
        {
            InitializeComponent();
        }

        public void setVars(IEnumerable<DMNExecutorSender.ItemTest> test)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(test.ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel =comboBox1.SelectedItem as DMNExecutorSender.ItemTest;
            if(sel != null)
            {
                listView1.Items.Clear();
                foreach (var item in sel.variables)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.Value?.ToString() }));
                }

            }
        }
    }
}
