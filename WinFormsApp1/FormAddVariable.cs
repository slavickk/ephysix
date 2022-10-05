using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class FormAddVariable : Form
    {
        public FormAddVariable()
        {
            InitializeComponent();
        }
        public string VariableName;
        public string VariableDescription;
        public string VariableType="String";
        public string VariableDefaultValue="Empty";

        private void button4_Click(object sender, EventArgs e)
        {
            VariableName = textBox6.Text;
            VariableDescription = textBox7.Text;
            VariableDefaultValue = textBoxDefaultValue.Text;
            VariableType=comboBoxVarType.Text;
        }

        private void FormAddVariable_Load(object sender, EventArgs e)
        {
            textBox6.Text=VariableName;
             textBox7.Text= VariableDescription;
            textBoxDefaultValue.Text=VariableDefaultValue;
            comboBoxVarType.SelectedItem=VariableType;

        }
    }
}
