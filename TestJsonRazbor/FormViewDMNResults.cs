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

        public void setVars(DMNExecutorSender.ItemVar[] vars)
        {
            listView1.Items.Clear();
            foreach(var item in vars)
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name, item.Value?.ToString() }));
            }
        }


    }
}
