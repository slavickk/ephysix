/******************************************************************
 * File: FormSelectField.Designer.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/


using System.Collections.Generic;

namespace TestJsonRazbor
{
    partial class FormSelectField
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectField));
            TreeNode treeNode5 = new TreeNode("Root");
            TreeNode treeNode6 = new TreeNode("Root");
            button7 = new Button();
            groupBox2 = new GroupBox();
            checkBoxTransformOutput = new CheckBox();
            button11 = new Button();
            buttonAddExtract = new Button();
            button10 = new Button();
            button6 = new Button();
            checkBoxPackToJson = new CheckBox();
            checkBoxIsUniq = new CheckBox();
            comboBoxTypeAlias = new ComboBox();
            checkBoxHash = new CheckBox();
            button5 = new Button();
            button3 = new Button();
            buttonDel = new Button();
            buttonMod = new Button();
            buttonAdd = new Button();
            comboBoxTypeTest = new ComboBox();
            label10 = new Label();
            comboBox3 = new ComboBox();
            listBox1 = new ListBox();
            textBoxFieldName = new TextBox();
            label4 = new Label();
            panel3 = new Panel();
            checkBoxNoEq = new CheckBox();
            checkBoxCompare2field = new CheckBox();
            textBoxCallFunction = new TextBox();
            userControlSwitch1 = new UserControlSwitch();
            checkBoxReturnFirstField = new CheckBox();
            checkBoxNameOnly = new CheckBox();
            checkBoxCopyChildOnly = new CheckBox();
            buttonDown = new Button();
            buttonUp = new Button();
            textBoxScript = new TextBox();
            textBoxValueFieldSearch = new TextBox();
            button4 = new Button();
            comboBox2 = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            textBoxFalueFieldSearchValue = new TextBox();
            textBoxAddFieldPath = new TextBox();
            button13 = new Button();
            checkBox2 = new CheckBox();
            label9 = new Label();
            panel4 = new Panel();
            textBoxConstant = new TextBox();
            comboBoxTypeConvert = new ComboBox();
            label11 = new Label();
            panel1 = new Panel();
            textBoxTemplate = new TextBox();
            buttonSelectTemplate = new Button();
            groupBox1 = new GroupBox();
            checkBoxRootOnly = new CheckBox();
            button2 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            textBoxNameFilter = new TextBox();
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            splitContainer2 = new SplitContainer();
            splitContainer3 = new SplitContainer();
            treeView2 = new TreeView();
            button8 = new Button();
            button9 = new Button();
            textBox1 = new TextBox();
            checkBox1 = new CheckBox();
            button1 = new Button();
            textBoxSearch = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            openFileDialog2 = new OpenFileDialog();
            openFileDialog3 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            toolTip1 = new ToolTip(components);
            openFileDialogFromFile = new OpenFileDialog();
            label1 = new Label();
            comboBoxOutType = new ComboBox();
            groupBox2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // button7
            // 
            button7.Location = new Point(4, 421);
            button7.Margin = new Padding(2);
            button7.Name = "button7";
            button7.Size = new Size(80, 22);
            button7.TabIndex = 36;
            button7.Text = "Test";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(checkBoxTransformOutput);
            groupBox2.Controls.Add(button11);
            groupBox2.Controls.Add(comboBoxOutType);
            groupBox2.Controls.Add(buttonAddExtract);
            groupBox2.Controls.Add(button10);
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(checkBoxPackToJson);
            groupBox2.Controls.Add(checkBoxIsUniq);
            groupBox2.Controls.Add(comboBoxTypeAlias);
            groupBox2.Controls.Add(checkBoxHash);
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(buttonDel);
            groupBox2.Controls.Add(buttonMod);
            groupBox2.Controls.Add(buttonAdd);
            groupBox2.Controls.Add(comboBoxTypeTest);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(comboBox3);
            groupBox2.Controls.Add(button7);
            groupBox2.Controls.Add(listBox1);
            groupBox2.Controls.Add(textBoxFieldName);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(panel3);
            groupBox2.Controls.Add(panel4);
            groupBox2.Controls.Add(panel1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(837, 445);
            groupBox2.TabIndex = 35;
            groupBox2.TabStop = false;
            groupBox2.Text = "OutputValues";
            // 
            // checkBoxTransformOutput
            // 
            checkBoxTransformOutput.AutoSize = true;
            checkBoxTransformOutput.Location = new Point(121, 7);
            checkBoxTransformOutput.Margin = new Padding(2, 1, 2, 1);
            checkBoxTransformOutput.Name = "checkBoxTransformOutput";
            checkBoxTransformOutput.Size = new Size(116, 19);
            checkBoxTransformOutput.TabIndex = 53;
            checkBoxTransformOutput.Text = "transformOutput";
            checkBoxTransformOutput.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Location = new Point(247, 266);
            button11.Margin = new Padding(2);
            button11.Name = "button11";
            button11.Size = new Size(43, 22);
            button11.TabIndex = 52;
            button11.Text = "reset";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click_1;
            // 
            // buttonAddExtract
            // 
            buttonAddExtract.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddExtract.Location = new Point(745, 8);
            buttonAddExtract.Margin = new Padding(2, 1, 2, 1);
            buttonAddExtract.Name = "buttonAddExtract";
            buttonAddExtract.Size = new Size(64, 19);
            buttonAddExtract.TabIndex = 51;
            buttonAddExtract.Text = "FromFile";
            buttonAddExtract.UseVisualStyleBackColor = true;
            buttonAddExtract.Click += buttonAddExtract_Click;
            // 
            // button10
            // 
            button10.Location = new Point(199, 266);
            button10.Margin = new Padding(2);
            button10.Name = "button10";
            button10.Size = new Size(43, 22);
            button10.TabIndex = 50;
            button10.Text = "V";
            button10.UseVisualStyleBackColor = true;
            button10.Click += buttonDown_Click;
            // 
            // button6
            // 
            button6.Location = new Point(152, 266);
            button6.Margin = new Padding(2);
            button6.Name = "button6";
            button6.Size = new Size(43, 22);
            button6.TabIndex = 49;
            button6.Text = "^";
            button6.UseVisualStyleBackColor = true;
            button6.Click += buttonUp_Click;
            // 
            // checkBoxPackToJson
            // 
            checkBoxPackToJson.AutoSize = true;
            checkBoxPackToJson.Location = new Point(573, 7);
            checkBoxPackToJson.Name = "checkBoxPackToJson";
            checkBoxPackToJson.Size = new Size(114, 19);
            checkBoxPackToJson.TabIndex = 48;
            checkBoxPackToJson.Text = "PackToJsonValue";
            checkBoxPackToJson.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsUniq
            // 
            checkBoxIsUniq.AutoSize = true;
            checkBoxIsUniq.Location = new Point(491, 26);
            checkBoxIsUniq.Margin = new Padding(2);
            checkBoxIsUniq.Name = "checkBoxIsUniq";
            checkBoxIsUniq.Size = new Size(51, 19);
            checkBoxIsUniq.TabIndex = 47;
            checkBoxIsUniq.Text = "Uniq";
            checkBoxIsUniq.UseVisualStyleBackColor = true;
            // 
            // comboBoxTypeAlias
            // 
            comboBoxTypeAlias.FormattingEnabled = true;
            comboBoxTypeAlias.Location = new Point(545, 7);
            comboBoxTypeAlias.Margin = new Padding(2);
            comboBoxTypeAlias.Name = "comboBoxTypeAlias";
            comboBoxTypeAlias.Size = new Size(23, 23);
            comboBoxTypeAlias.TabIndex = 43;
            comboBoxTypeAlias.Visible = false;
            comboBoxTypeAlias.SelectedIndexChanged += comboBoxTypeAlias_SelectedIndexChanged;
            // 
            // checkBoxHash
            // 
            checkBoxHash.AutoSize = true;
            checkBoxHash.Location = new Point(818, 9);
            checkBoxHash.Margin = new Padding(2);
            checkBoxHash.Name = "checkBoxHash";
            checkBoxHash.Size = new Size(66, 19);
            checkBoxHash.TabIndex = 42;
            checkBoxHash.Text = "Hashed";
            checkBoxHash.UseVisualStyleBackColor = true;
            checkBoxHash.CheckedChanged += checkBoxHash_CheckedChanged;
            // 
            // button5
            // 
            button5.Location = new Point(120, 23);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(46, 22);
            button5.TabIndex = 41;
            button5.Text = "paste";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.DialogResult = DialogResult.OK;
            button3.Location = new Point(703, 418);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(123, 22);
            button3.TabIndex = 36;
            button3.Text = "Save filter";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // buttonDel
            // 
            buttonDel.Enabled = false;
            buttonDel.Location = new Point(97, 266);
            buttonDel.Margin = new Padding(2);
            buttonDel.Name = "buttonDel";
            buttonDel.Size = new Size(43, 22);
            buttonDel.TabIndex = 40;
            buttonDel.Text = "del";
            buttonDel.UseVisualStyleBackColor = true;
            buttonDel.Click += buttonDel_Click;
            // 
            // buttonMod
            // 
            buttonMod.Enabled = false;
            buttonMod.Location = new Point(51, 266);
            buttonMod.Margin = new Padding(2);
            buttonMod.Name = "buttonMod";
            buttonMod.Size = new Size(43, 22);
            buttonMod.TabIndex = 39;
            buttonMod.Text = "mod";
            buttonMod.UseVisualStyleBackColor = true;
            buttonMod.Click += buttonMod_Click;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(4, 266);
            buttonAdd.Margin = new Padding(2);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(43, 22);
            buttonAdd.TabIndex = 38;
            buttonAdd.Text = "add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // comboBoxTypeTest
            // 
            comboBoxTypeTest.FormattingEnabled = true;
            comboBoxTypeTest.Items.AddRange(new object[] { "All output+filter", "All output", "Current only" });
            comboBoxTypeTest.Location = new Point(100, 421);
            comboBoxTypeTest.Margin = new Padding(2);
            comboBoxTypeTest.Name = "comboBoxTypeTest";
            comboBoxTypeTest.Size = new Size(104, 23);
            comboBoxTypeTest.TabIndex = 37;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(537, 27);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(31, 15);
            label10.TabIndex = 33;
            label10.Text = "Type";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "constant", "select", "template" });
            comboBox3.Location = new Point(572, 26);
            comboBox3.Margin = new Padding(2);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(82, 23);
            comboBox3.TabIndex = 14;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(4, 290);
            listBox1.Margin = new Padding(2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(822, 124);
            listBox1.TabIndex = 29;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // textBoxFieldName
            // 
            textBoxFieldName.Location = new Point(171, 25);
            textBoxFieldName.Margin = new Padding(2);
            textBoxFieldName.Name = "textBoxFieldName";
            textBoxFieldName.Size = new Size(315, 23);
            textBoxFieldName.TabIndex = 21;
            textBoxFieldName.TextChanged += textBoxFieldName_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 24);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(101, 15);
            label4.TabIndex = 19;
            label4.Text = "Output field path:";
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(checkBoxNoEq);
            panel3.Controls.Add(checkBoxCompare2field);
            panel3.Controls.Add(textBoxCallFunction);
            panel3.Controls.Add(userControlSwitch1);
            panel3.Controls.Add(checkBoxReturnFirstField);
            panel3.Controls.Add(checkBoxNameOnly);
            panel3.Controls.Add(checkBoxCopyChildOnly);
            panel3.Controls.Add(buttonDown);
            panel3.Controls.Add(buttonUp);
            panel3.Controls.Add(textBoxScript);
            panel3.Controls.Add(textBoxValueFieldSearch);
            panel3.Controls.Add(button4);
            panel3.Controls.Add(comboBox2);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(textBoxFalueFieldSearchValue);
            panel3.Controls.Add(textBoxAddFieldPath);
            panel3.Controls.Add(button13);
            panel3.Controls.Add(checkBox2);
            panel3.Controls.Add(label9);
            panel3.Location = new Point(6, 52);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(823, 212);
            panel3.TabIndex = 34;
            panel3.Visible = false;
            // 
            // checkBoxNoEq
            // 
            checkBoxNoEq.AutoSize = true;
            checkBoxNoEq.Location = new Point(445, 46);
            checkBoxNoEq.Margin = new Padding(2);
            checkBoxNoEq.Name = "checkBoxNoEq";
            checkBoxNoEq.Size = new Size(62, 19);
            checkBoxNoEq.TabIndex = 38;
            checkBoxNoEq.Text = "Not eq";
            checkBoxNoEq.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompare2field
            // 
            checkBoxCompare2field.AutoSize = true;
            checkBoxCompare2field.Location = new Point(445, 31);
            checkBoxCompare2field.Margin = new Padding(2);
            checkBoxCompare2field.Name = "checkBoxCompare2field";
            checkBoxCompare2field.Size = new Size(99, 19);
            checkBoxCompare2field.TabIndex = 37;
            checkBoxCompare2field.Text = "Comp.2 fields";
            checkBoxCompare2field.UseVisualStyleBackColor = true;
            checkBoxCompare2field.CheckedChanged += checkBoxCompare2field_CheckedChanged;
            // 
            // textBoxCallFunction
            // 
            textBoxCallFunction.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxCallFunction.Location = new Point(710, 10);
            textBoxCallFunction.Margin = new Padding(2, 1, 2, 1);
            textBoxCallFunction.Name = "textBoxCallFunction";
            textBoxCallFunction.Size = new Size(86, 23);
            textBoxCallFunction.TabIndex = 36;
            toolTip1.SetToolTip(textBoxCallFunction, "Embedded function expression");
            textBoxCallFunction.Visible = false;
            textBoxCallFunction.TextChanged += textBox2_TextChanged;
            // 
            // userControlSwitch1
            // 
            userControlSwitch1.Location = new Point(290, 54);
            userControlSwitch1.Margin = new Padding(6);
            userControlSwitch1.Name = "userControlSwitch1";
            userControlSwitch1.Size = new Size(268, 182);
            userControlSwitch1.TabIndex = 35;
            userControlSwitch1.Visible = false;
            // 
            // checkBoxReturnFirstField
            // 
            checkBoxReturnFirstField.AutoSize = true;
            checkBoxReturnFirstField.Location = new Point(771, 34);
            checkBoxReturnFirstField.Margin = new Padding(2);
            checkBoxReturnFirstField.Name = "checkBoxReturnFirstField";
            checkBoxReturnFirstField.Size = new Size(105, 19);
            checkBoxReturnFirstField.TabIndex = 34;
            checkBoxReturnFirstField.Text = "returnFirstField";
            checkBoxReturnFirstField.UseVisualStyleBackColor = true;
            // 
            // checkBoxNameOnly
            // 
            checkBoxNameOnly.AutoSize = true;
            checkBoxNameOnly.Location = new Point(571, 33);
            checkBoxNameOnly.Margin = new Padding(2);
            checkBoxNameOnly.Name = "checkBoxNameOnly";
            checkBoxNameOnly.Size = new Size(79, 19);
            checkBoxNameOnly.TabIndex = 33;
            checkBoxNameOnly.Text = "Get Name";
            checkBoxNameOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopyChildOnly
            // 
            checkBoxCopyChildOnly.AutoSize = true;
            checkBoxCopyChildOnly.Location = new Point(658, 34);
            checkBoxCopyChildOnly.Margin = new Padding(2);
            checkBoxCopyChildOnly.Name = "checkBoxCopyChildOnly";
            checkBoxCopyChildOnly.Size = new Size(109, 19);
            checkBoxCopyChildOnly.TabIndex = 32;
            checkBoxCopyChildOnly.Text = "Copy child only";
            checkBoxCopyChildOnly.UseVisualStyleBackColor = true;
            // 
            // buttonDown
            // 
            buttonDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDown.Font = new Font("Segoe UI", 6F, FontStyle.Regular, GraphicsUnit.Point);
            buttonDown.Location = new Point(788, 78);
            buttonDown.Margin = new Padding(2);
            buttonDown.Name = "buttonDown";
            buttonDown.Size = new Size(23, 22);
            buttonDown.TabIndex = 31;
            buttonDown.Text = "V";
            buttonDown.UseVisualStyleBackColor = true;
            // 
            // buttonUp
            // 
            buttonUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonUp.Location = new Point(788, 54);
            buttonUp.Margin = new Padding(2);
            buttonUp.Name = "buttonUp";
            buttonUp.Size = new Size(22, 22);
            buttonUp.TabIndex = 30;
            buttonUp.Text = "^";
            buttonUp.UseVisualStyleBackColor = true;
            buttonUp.Click += button6_Click;
            // 
            // textBoxScript
            // 
            textBoxScript.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxScript.Location = new Point(6, 123);
            textBoxScript.Margin = new Padding(2);
            textBoxScript.Multiline = true;
            textBoxScript.Name = "textBoxScript";
            textBoxScript.ScrollBars = ScrollBars.Vertical;
            textBoxScript.Size = new Size(783, 82);
            textBoxScript.TabIndex = 29;
            // 
            // textBoxValueFieldSearch
            // 
            textBoxValueFieldSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxValueFieldSearch.Location = new Point(146, 9);
            textBoxValueFieldSearch.Margin = new Padding(2);
            textBoxValueFieldSearch.Name = "textBoxValueFieldSearch";
            textBoxValueFieldSearch.Size = new Size(556, 23);
            textBoxValueFieldSearch.TabIndex = 18;
            // 
            // button4
            // 
            button4.Location = new Point(102, 8);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.Size = new Size(46, 22);
            button4.TabIndex = 22;
            button4.Text = "paste";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "value", "script", "switch" });
            comboBox2.Location = new Point(571, 54);
            comboBox2.Margin = new Padding(2);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(96, 23);
            comboBox2.TabIndex = 14;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 8);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(91, 15);
            label6.TabIndex = 20;
            label6.Text = "Input field path:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(4, 30);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(81, 15);
            label5.TabIndex = 23;
            label5.Text = "Searched text:";
            // 
            // textBoxFalueFieldSearchValue
            // 
            textBoxFalueFieldSearchValue.Location = new Point(147, 28);
            textBoxFalueFieldSearchValue.Margin = new Padding(2);
            textBoxFalueFieldSearchValue.Name = "textBoxFalueFieldSearchValue";
            textBoxFalueFieldSearchValue.Size = new Size(111, 23);
            textBoxFalueFieldSearchValue.TabIndex = 24;
            textBoxFalueFieldSearchValue.TextChanged += textBoxFalueFieldSearchValue_TextChanged;
            // 
            // textBoxAddFieldPath
            // 
            textBoxAddFieldPath.Location = new Point(148, 54);
            textBoxAddFieldPath.Margin = new Padding(2);
            textBoxAddFieldPath.Name = "textBoxAddFieldPath";
            textBoxAddFieldPath.Size = new Size(414, 23);
            textBoxAddFieldPath.TabIndex = 26;
            textBoxAddFieldPath.Visible = false;
            // 
            // button13
            // 
            button13.Location = new Point(100, 54);
            button13.Margin = new Padding(2);
            button13.Name = "button13";
            button13.Size = new Size(46, 22);
            button13.TabIndex = 28;
            button13.Text = "paste";
            button13.UseVisualStyleBackColor = true;
            button13.Visible = false;
            button13.Click += button13_Click;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(270, 30);
            checkBox2.Margin = new Padding(2);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(162, 19);
            checkBox2.TabIndex = 25;
            checkBox2.Text = "get another field for value";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 55);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(88, 15);
            label9.TabIndex = 27;
            label9.Text = "Add field path :";
            label9.Visible = false;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel4.Controls.Add(textBoxConstant);
            panel4.Controls.Add(comboBoxTypeConvert);
            panel4.Controls.Add(label11);
            panel4.Location = new Point(6, 52);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(823, 74);
            panel4.TabIndex = 29;
            panel4.Visible = false;
            // 
            // textBoxConstant
            // 
            textBoxConstant.Location = new Point(148, 7);
            textBoxConstant.Margin = new Padding(2);
            textBoxConstant.Name = "textBoxConstant";
            textBoxConstant.Size = new Size(110, 23);
            textBoxConstant.TabIndex = 1;
            // 
            // comboBoxTypeConvert
            // 
            comboBoxTypeConvert.FormattingEnabled = true;
            comboBoxTypeConvert.Location = new Point(716, 11);
            comboBoxTypeConvert.Margin = new Padding(2);
            comboBoxTypeConvert.Name = "comboBoxTypeConvert";
            comboBoxTypeConvert.Size = new Size(132, 23);
            comboBoxTypeConvert.TabIndex = 32;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 8);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(86, 15);
            label11.TabIndex = 0;
            label11.Text = "Constant value";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(textBoxTemplate);
            panel1.Controls.Add(buttonSelectTemplate);
            panel1.Location = new Point(6, 52);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(823, 212);
            panel1.TabIndex = 32;
            panel1.Visible = false;
            // 
            // textBoxTemplate
            // 
            textBoxTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTemplate.Location = new Point(1, 24);
            textBoxTemplate.Margin = new Padding(2);
            textBoxTemplate.Multiline = true;
            textBoxTemplate.Name = "textBoxTemplate";
            textBoxTemplate.Size = new Size(822, 188);
            textBoxTemplate.TabIndex = 1;
            // 
            // buttonSelectTemplate
            // 
            buttonSelectTemplate.Location = new Point(0, 0);
            buttonSelectTemplate.Margin = new Padding(2);
            buttonSelectTemplate.Name = "buttonSelectTemplate";
            buttonSelectTemplate.Size = new Size(111, 22);
            buttonSelectTemplate.TabIndex = 0;
            buttonSelectTemplate.Text = "Select Template";
            buttonSelectTemplate.UseVisualStyleBackColor = true;
            buttonSelectTemplate.Click += buttonSelectTemplate_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBoxRootOnly);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(tabControl1);
            groupBox1.Controls.Add(textBoxNameFilter);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(837, 119);
            groupBox1.TabIndex = 34;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filter";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // checkBoxRootOnly
            // 
            checkBoxRootOnly.AutoSize = true;
            checkBoxRootOnly.Location = new Point(156, 1);
            checkBoxRootOnly.Margin = new Padding(2, 1, 2, 1);
            checkBoxRootOnly.Name = "checkBoxRootOnly";
            checkBoxRootOnly.Size = new Size(93, 19);
            checkBoxRootOnly.TabIndex = 0;
            checkBoxRootOnly.Text = "getRootOnly";
            checkBoxRootOnly.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(70, 0);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(80, 18);
            button2.TabIndex = 14;
            button2.Text = "Test";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabControl1
            // 
            tabControl1.Alignment = TabAlignment.Bottom;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(2, 18);
            tabControl1.Margin = new Padding(2);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(833, 99);
            tabControl1.TabIndex = 15;
            tabControl1.Visible = false;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 4);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2);
            tabPage1.Size = new Size(825, 71);
            tabPage1.TabIndex = 0;
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxNameFilter
            // 
            textBoxNameFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxNameFilter.Location = new Point(661, -2);
            textBoxNameFilter.Margin = new Padding(2);
            textBoxNameFilter.Name = "textBoxNameFilter";
            textBoxNameFilter.Size = new Size(172, 23);
            textBoxNameFilter.TabIndex = 39;
            // 
            // splitContainer1
            // 
            splitContainer1.Cursor = Cursors.VSplit;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1221, 568);
            splitContainer1.SplitterDistance = 212;
            splitContainer1.SplitterWidth = 2;
            splitContainer1.TabIndex = 38;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.HideSelection = false;
            treeView1.Location = new Point(0, 0);
            treeView1.Margin = new Padding(2);
            treeView1.Name = "treeView1";
            treeNode5.Name = "Node0";
            treeNode5.Text = "Root";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode5 });
            treeView1.Size = new Size(212, 568);
            treeView1.TabIndex = 1;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseDoubleClick += treeView1_NodeMouseDoubleClick;
            treeView1.DoubleClick += treeView1_DoubleClick;
            // 
            // splitContainer2
            // 
            splitContainer2.Cursor = Cursors.VSplit;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(2);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(treeView2);
            splitContainer2.Size = new Size(1007, 568);
            splitContainer2.SplitterDistance = 837;
            splitContainer2.SplitterWidth = 2;
            splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(groupBox2);
            splitContainer3.Size = new Size(837, 568);
            splitContainer3.SplitterDistance = 119;
            splitContainer3.TabIndex = 40;
            // 
            // treeView2
            // 
            treeView2.Dock = DockStyle.Fill;
            treeView2.HideSelection = false;
            treeView2.Location = new Point(0, 0);
            treeView2.Margin = new Padding(2);
            treeView2.Name = "treeView2";
            treeNode6.Name = "Node0";
            treeNode6.Text = "Root";
            treeView2.Nodes.AddRange(new TreeNode[] { treeNode6 });
            treeView2.Size = new Size(168, 568);
            treeView2.TabIndex = 2;
            treeView2.AfterSelect += treeView1_AfterSelect;
            // 
            // button8
            // 
            button8.Location = new Point(352, 0);
            button8.Margin = new Padding(2);
            button8.Name = "button8";
            button8.Size = new Size(80, 22);
            button8.TabIndex = 27;
            button8.Text = "Save";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(268, 0);
            button9.Margin = new Padding(2);
            button9.Name = "button9";
            button9.Size = new Size(80, 22);
            button9.TabIndex = 28;
            button9.Text = "Load";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(766, 4);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(1778, 23);
            textBox1.TabIndex = 26;
            textBox1.Visible = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(177, 0);
            checkBox1.Margin = new Padding(2);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(85, 19);
            checkBox1.TabIndex = 25;
            checkBox1.Text = "from begin";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button1
            // 
            button1.Location = new Point(115, 0);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(58, 22);
            button1.TabIndex = 24;
            button1.Text = "Search";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(3, 0);
            textBoxSearch.Margin = new Padding(2);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(110, 23);
            textBoxSearch.TabIndex = 23;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.InitialDirectory = "C:\\Users\\User\\Documents\\PacketOut";
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog2";
            openFileDialog2.Filter = "Yaml files |*.yml";
            openFileDialog2.InitialDirectory = "C:\\Users\\User\\Documents";
            // 
            // openFileDialog3
            // 
            openFileDialog3.FileName = "openFileDialog1";
            openFileDialog3.InitialDirectory = "C:\\Users\\User\\Documents\\PacketOut";
            // 
            // toolTip1
            // 
            toolTip1.ForeColor = Color.LavenderBlush;
            // 
            // openFileDialogFromFile
            // 
            openFileDialogFromFile.FileName = "openFileDialog4";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(672, 26);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 55;
            label1.Text = "OutType";
            label1.Click += label1_Click;
            // 
            // comboBoxOutType
            // 
            comboBoxOutType.FormattingEnabled = true;
            comboBoxOutType.Items.AddRange(new object[] { "string", "integer", "double", "boolean" });
            comboBoxOutType.Location = new Point(727, 25);
            comboBoxOutType.Margin = new Padding(2);
            comboBoxOutType.Name = "comboBoxOutType";
            comboBoxOutType.Size = new Size(82, 23);
            comboBoxOutType.TabIndex = 54;
            // 
            // FormSelectField
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1221, 568);
            Controls.Add(splitContainer1);
            Controls.Add(textBox1);
            Controls.Add(button8);
            Controls.Add(textBoxSearch);
            Controls.Add(button9);
            Controls.Add(button1);
            Controls.Add(checkBox1);
            Margin = new Padding(2);
            Name = "FormSelectField";
            Text = "FormSelectField";
            Load += FormSelectField_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBoxFieldName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBoxValueFieldSearch;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFalueFieldSearchValue;
        private System.Windows.Forms.TextBox textBoxAddFieldPath;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBoxConstant;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxTypeTest;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.Button buttonMod;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.CheckBox checkBoxHash;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.ComboBox comboBoxTypeConvert;
        private System.Windows.Forms.ComboBox comboBoxTypeAlias;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxTemplate;
        private System.Windows.Forms.Button buttonSelectTemplate;
        private System.Windows.Forms.CheckBox checkBoxIsUniq;
        private System.Windows.Forms.CheckBox checkBoxCopyChildOnly;
        private System.Windows.Forms.CheckBox checkBoxNameOnly;
        private System.Windows.Forms.TextBox textBoxNameFilter;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxReturnFirstField;
        private System.Windows.Forms.CheckBox checkBoxPackToJson;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button6;
        private UserControlSwitch userControlSwitch1;
        private System.Windows.Forms.Button buttonAddExtract;
        private System.Windows.Forms.OpenFileDialog openFileDialogFromFile;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.CheckBox checkBoxTransformOutput;
        private System.Windows.Forms.TextBox textBoxCallFunction;
        private System.Windows.Forms.CheckBox checkBoxRootOnly;
        private System.Windows.Forms.CheckBox checkBoxCompare2field;
        private System.Windows.Forms.CheckBox checkBoxNoEq;
        private Label label1;
        private ComboBox comboBoxOutType;
    }
}