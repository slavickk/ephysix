using ETL_DB_Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsETLPackagedCreator
{
    public partial class FormSelectSynonymLink : Form
    {
        public List<DBInterface.SourceTableItemAgg> list;
        string name;
        public FormSelectSynonymLink(List<DBInterface.SourceTableItemAgg> list,string name)
        {
            this.list = list;
            this.name = name;
            InitializeComponent();
        }

        private void FormSelectSynonymLink_Load(object sender, EventArgs e)
        {
            this.Text = $"Таблицы, связанные с {name}";
            checkedListBox1.Items.AddRange(list.ToArray());
        }


        private void buttonFinish_Click(object sender, EventArgs e)
        {
            list.Clear();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                list.Add((DBInterface.SourceTableItemAgg)item);
            }
        }
    }
}
