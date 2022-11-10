using ParserLibrary;
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
    public partial class UserControlFormOutputField : UserControl
    {
        string outputPath;
        bool isUniq;
        public UserControlFormOutputField(string outputPath,bool isUniq)
        {
            this.outputPath = outputPath;
            this.isUniq = isUniq;
            InitializeComponent();
        }
        public UserControlFormOutputField()
        {
//            InitializeComponent();
        }
        public void Init()
        {
            InitializeComponent();

        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBoxValueFieldSearch.Text = Clipboard.GetText();

        }

        public OutputValue outValue
        {
            get
            {
                return fillOutput();
            }
        }
 
        OutputValue fillOutput()
        {
            ConverterOutput converter = null;
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    return new ConstantValue() { viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = outputPath, isUniqOutputPath = isUniq, getNodeNameOnly = checkBoxNameOnly.Checked, typeConvert = (ConstantValue.TypeObject)comboBoxTypeConvert.SelectedItem, Value = ConstantValue.ConvertFromType(textBoxConstant.Text, (ConstantValue.TypeObject)comboBoxTypeConvert.SelectedItem) };
                case 1:
                    if (comboBox2.SelectedIndex != 1)
                        return new ExtractFromInputValue() { viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = outputPath, isUniqOutputPath = isUniq, getNodeNameOnly = checkBoxNameOnly.Checked, returnOnlyFirstRow = checkBoxReturnFirstField.Checked, copyChildsOnly = checkBoxCopyChildOnly.Checked, conditionPath = textBoxValueFieldSearch.Text, conditionCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text))), valuePath = (checkBox2.Checked ? textBoxAddFieldPath.Text : "") };
                    else
                        return new ExtractFromInputValueWithScript() { viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = outputPath, isUniqOutputPath = isUniq, getNodeNameOnly = checkBoxNameOnly.Checked, conditionPath = textBoxValueFieldSearch.Text, conditionCalcer = ((textBoxFalueFieldSearchValue.Text == "") ? null : (new ComparerForValue(textBoxFalueFieldSearchValue.Text))), ScriptBody = textBoxScript.Text };
                case 2:
                    return new TemplateOutputValue() { viewAsJsonString = checkBoxPackToJson.Checked, converter = converter, outputPath = outputPath, getNodeNameOnly = checkBoxNameOnly.Checked, isUniqOutputPath = isUniq, templateBody = textBoxTemplate.Text };
                    break;
                default:
                    break;
            }
            return null;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 1)
            {
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
                textBoxScript.Visible = true;
                checkBox2.Visible = false;
            }
            else
            {
                checkBox2.Visible = true;
                textBoxScript.Visible = false;
                if (checkBox2.Checked == false)
                    label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
                else
                    label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = true;

            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = false;
            else
                label9.Visible = button13.Visible = textBoxAddFieldPath.Visible = true;


        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
