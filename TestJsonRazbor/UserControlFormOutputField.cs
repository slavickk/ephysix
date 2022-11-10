﻿using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            comboBox3.SelectedIndex = 0;
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    panel1.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = true;
                    break;
                case 1:
                    panel1.Visible = false;
                    panel3.Visible = true;
                    panel4.Visible = false;
                    break;
                case 2:
                    panel1.Visible = true;
                    panel3.Visible = false;
                    panel4.Visible = false;

                    var type_frm = GetTypeOfForm();

                    if (type_frm != null)
                    {
                        buttonSelectTemplate.Text = "Configure";
                    }

                    // Ищем  форму , которая обрабатывает Sendera

                    break;
                default:
                    break;
            }


        }
        private Type GetTypeOfForm()
        {
            return Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.CustomAttributes.Count(ii => ii.AttributeType == typeof(GUIAttribute) /*&& ii.ConstructorArguments[0].ArgumentType == currentStep.sender.GetType()*/) > 0);
        }

    }
}
