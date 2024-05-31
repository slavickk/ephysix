/******************************************************************
 * File: UserControlFormOutputField.Designer.cs
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

namespace TestJsonRazbor
{
    partial class UserControlFormOutputField
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBoxPackToJson = new System.Windows.Forms.CheckBox();
            this.checkBoxReturnFirstField = new System.Windows.Forms.CheckBox();
            this.checkBoxNameOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxCopyChildOnly = new System.Windows.Forms.CheckBox();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.textBoxValueFieldSearch = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFalueFieldSearchValue = new System.Windows.Forms.TextBox();
            this.textBoxAddFieldPath = new System.Windows.Forms.TextBox();
            this.button13 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBoxConstant = new System.Windows.Forms.TextBox();
            this.comboBoxTypeConvert = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxTemplate = new System.Windows.Forms.TextBox();
            this.buttonSelectTemplate = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBoxPackToJson);
            this.panel3.Controls.Add(this.checkBoxReturnFirstField);
            this.panel3.Controls.Add(this.checkBoxNameOnly);
            this.panel3.Controls.Add(this.checkBoxCopyChildOnly);
            this.panel3.Controls.Add(this.buttonDown);
            this.panel3.Controls.Add(this.buttonUp);
            this.panel3.Controls.Add(this.textBoxScript);
            this.panel3.Controls.Add(this.textBoxValueFieldSearch);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.comboBox2);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.textBoxFalueFieldSearchValue);
            this.panel3.Controls.Add(this.textBoxAddFieldPath);
            this.panel3.Controls.Add(this.button13);
            this.panel3.Controls.Add(this.checkBox2);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(704, 335);
            this.panel3.TabIndex = 37;
            this.panel3.Visible = false;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // checkBoxPackToJson
            // 
            this.checkBoxPackToJson.AutoSize = true;
            this.checkBoxPackToJson.Location = new System.Drawing.Point(576, 15);
            this.checkBoxPackToJson.Name = "checkBoxPackToJson";
            this.checkBoxPackToJson.Size = new System.Drawing.Size(114, 19);
            this.checkBoxPackToJson.TabIndex = 50;
            this.checkBoxPackToJson.Text = "PackToJsonValue";
            this.checkBoxPackToJson.UseVisualStyleBackColor = true;
            // 
            // checkBoxReturnFirstField
            // 
            this.checkBoxReturnFirstField.AutoSize = true;
            this.checkBoxReturnFirstField.Location = new System.Drawing.Point(766, 11);
            this.checkBoxReturnFirstField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxReturnFirstField.Name = "checkBoxReturnFirstField";
            this.checkBoxReturnFirstField.Size = new System.Drawing.Size(105, 19);
            this.checkBoxReturnFirstField.TabIndex = 34;
            this.checkBoxReturnFirstField.Text = "returnFirstField";
            this.checkBoxReturnFirstField.UseVisualStyleBackColor = true;
            // 
            // checkBoxNameOnly
            // 
            this.checkBoxNameOnly.AutoSize = true;
            this.checkBoxNameOnly.Location = new System.Drawing.Point(576, 0);
            this.checkBoxNameOnly.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxNameOnly.Name = "checkBoxNameOnly";
            this.checkBoxNameOnly.Size = new System.Drawing.Size(79, 19);
            this.checkBoxNameOnly.TabIndex = 33;
            this.checkBoxNameOnly.Text = "Get Name";
            this.checkBoxNameOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopyChildOnly
            // 
            this.checkBoxCopyChildOnly.AutoSize = true;
            this.checkBoxCopyChildOnly.Location = new System.Drawing.Point(655, 0);
            this.checkBoxCopyChildOnly.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxCopyChildOnly.Name = "checkBoxCopyChildOnly";
            this.checkBoxCopyChildOnly.Size = new System.Drawing.Size(109, 19);
            this.checkBoxCopyChildOnly.TabIndex = 32;
            this.checkBoxCopyChildOnly.Text = "Copy child only";
            this.checkBoxCopyChildOnly.UseVisualStyleBackColor = true;
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDown.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonDown.Location = new System.Drawing.Point(1125, 78);
            this.buttonDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(23, 22);
            this.buttonDown.TabIndex = 31;
            this.buttonDown.Text = "V";
            this.buttonDown.UseVisualStyleBackColor = true;
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUp.Location = new System.Drawing.Point(1125, 54);
            this.buttonUp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(22, 22);
            this.buttonUp.TabIndex = 30;
            this.buttonUp.Text = "^";
            this.buttonUp.UseVisualStyleBackColor = true;
            // 
            // textBoxScript
            // 
            this.textBoxScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScript.Location = new System.Drawing.Point(6, 54);
            this.textBoxScript.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxScript.Size = new System.Drawing.Size(687, 279);
            this.textBoxScript.TabIndex = 29;
            this.textBoxScript.Visible = false;
            // 
            // textBoxValueFieldSearch
            // 
            this.textBoxValueFieldSearch.Location = new System.Drawing.Point(146, 4);
            this.textBoxValueFieldSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxValueFieldSearch.Name = "textBoxValueFieldSearch";
            this.textBoxValueFieldSearch.Size = new System.Drawing.Size(414, 23);
            this.textBoxValueFieldSearch.TabIndex = 18;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(102, 3);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(46, 22);
            this.button4.TabIndex = 22;
            this.button4.Text = "paste";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "value",
            "script"});
            this.comboBox2.Location = new System.Drawing.Point(464, 29);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(96, 23);
            this.comboBox2.TabIndex = 14;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 15);
            this.label6.TabIndex = 20;
            this.label6.Text = "Input field path:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 30);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 15);
            this.label5.TabIndex = 23;
            this.label5.Text = "Searched text:";
            // 
            // textBoxFalueFieldSearchValue
            // 
            this.textBoxFalueFieldSearchValue.Location = new System.Drawing.Point(147, 28);
            this.textBoxFalueFieldSearchValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxFalueFieldSearchValue.Name = "textBoxFalueFieldSearchValue";
            this.textBoxFalueFieldSearchValue.Size = new System.Drawing.Size(111, 23);
            this.textBoxFalueFieldSearchValue.TabIndex = 24;
            // 
            // textBoxAddFieldPath
            // 
            this.textBoxAddFieldPath.Location = new System.Drawing.Point(148, 54);
            this.textBoxAddFieldPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxAddFieldPath.Name = "textBoxAddFieldPath";
            this.textBoxAddFieldPath.Size = new System.Drawing.Size(414, 23);
            this.textBoxAddFieldPath.TabIndex = 26;
            this.textBoxAddFieldPath.Visible = false;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(100, 54);
            this.button13.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(46, 22);
            this.button13.TabIndex = 28;
            this.button13.Text = "paste";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Visible = false;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(270, 30);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(162, 19);
            this.checkBox2.TabIndex = 25;
            this.checkBox2.Text = "get another field for value";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 55);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 15);
            this.label9.TabIndex = 27;
            this.label9.Text = "Add field path :";
            this.label9.Visible = false;
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "constant",
            "select",
            "template"});
            this.comboBox3.Location = new System.Drawing.Point(609, 30);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(82, 23);
            this.comboBox3.TabIndex = 49;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.textBoxConstant);
            this.panel4.Controls.Add(this.comboBoxTypeConvert);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Location = new System.Drawing.Point(2, 2);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(700, 74);
            this.panel4.TabIndex = 35;
            this.panel4.Visible = false;
            // 
            // textBoxConstant
            // 
            this.textBoxConstant.Location = new System.Drawing.Point(148, 7);
            this.textBoxConstant.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxConstant.Name = "textBoxConstant";
            this.textBoxConstant.Size = new System.Drawing.Size(110, 23);
            this.textBoxConstant.TabIndex = 1;
            // 
            // comboBoxTypeConvert
            // 
            this.comboBoxTypeConvert.FormattingEnabled = true;
            this.comboBoxTypeConvert.Location = new System.Drawing.Point(716, 11);
            this.comboBoxTypeConvert.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxTypeConvert.Name = "comboBoxTypeConvert";
            this.comboBoxTypeConvert.Size = new System.Drawing.Size(132, 23);
            this.comboBoxTypeConvert.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 8);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Constant value";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxTemplate);
            this.panel1.Controls.Add(this.buttonSelectTemplate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 335);
            this.panel1.TabIndex = 36;
            this.panel1.Visible = false;
            // 
            // textBoxTemplate
            // 
            this.textBoxTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTemplate.Location = new System.Drawing.Point(1, 24);
            this.textBoxTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxTemplate.Multiline = true;
            this.textBoxTemplate.Name = "textBoxTemplate";
            this.textBoxTemplate.Size = new System.Drawing.Size(1160, 476);
            this.textBoxTemplate.TabIndex = 1;
            // 
            // buttonSelectTemplate
            // 
            this.buttonSelectTemplate.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonSelectTemplate.Name = "buttonSelectTemplate";
            this.buttonSelectTemplate.Size = new System.Drawing.Size(111, 22);
            this.buttonSelectTemplate.TabIndex = 0;
            this.buttonSelectTemplate.Text = "Select Template";
            this.buttonSelectTemplate.UseVisualStyleBackColor = true;
            // 
            // UserControlFormOutputField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "UserControlFormOutputField";
            this.Size = new System.Drawing.Size(704, 335);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox checkBoxReturnFirstField;
        private System.Windows.Forms.CheckBox checkBoxNameOnly;
        private System.Windows.Forms.CheckBox checkBoxCopyChildOnly;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.TextBox textBoxScript;
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
        private System.Windows.Forms.ComboBox comboBoxTypeConvert;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxTemplate;
        private System.Windows.Forms.Button buttonSelectTemplate;
        private System.Windows.Forms.CheckBox checkBoxPackToJson;
        private System.Windows.Forms.ComboBox comboBox3;
    }
}
