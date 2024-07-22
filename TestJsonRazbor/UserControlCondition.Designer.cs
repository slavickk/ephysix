/******************************************************************
 * File: UserControlCondition.Designer.cs
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
    partial class UserControlCondition
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
            this.comboBoxTypeCompare = new System.Windows.Forms.ComboBox();
            this.textBoxFilterValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFilterFieldPath = new System.Windows.Forms.TextBox();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBoxListValues = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.checkBoxNegative = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxTypeCompare
            // 
            this.comboBoxTypeCompare.FormattingEnabled = true;
            this.comboBoxTypeCompare.Items.AddRange(new object[] {
            "value",
            "path only",
            "script",
            "list"});
            this.comboBoxTypeCompare.Location = new System.Drawing.Point(749, 56);
            this.comboBoxTypeCompare.Name = "comboBoxTypeCompare";
            this.comboBoxTypeCompare.Size = new System.Drawing.Size(141, 40);
            this.comboBoxTypeCompare.TabIndex = 19;
            this.comboBoxTypeCompare.SelectedIndexChanged += new System.EventHandler(this.comboBoxTypeCompare_SelectedIndexChanged);
            // 
            // textBoxFilterValue
            // 
            this.textBoxFilterValue.Location = new System.Drawing.Point(330, 57);
            this.textBoxFilterValue.Multiline = false;
            this.textBoxFilterValue.Name = "textBoxFilterValue";
            this.textBoxFilterValue.Size = new System.Drawing.Size(191, 93);
            this.textBoxFilterValue.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 32);
            this.label3.TabIndex = 17;
            this.label3.Text = "Searched text:";
            // 
            // textBoxFilterFieldPath
            // 
            this.textBoxFilterFieldPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilterFieldPath.Location = new System.Drawing.Point(274, 1);
            this.textBoxFilterFieldPath.Name = "textBoxFilterFieldPath";
            this.textBoxFilterFieldPath.Size = new System.Drawing.Size(616, 39);
            this.textBoxFilterFieldPath.TabIndex = 16;
            this.textBoxFilterFieldPath.TextChanged += new System.EventHandler(this.textBoxFilterFieldPath_TextChanged);
            // 
            // buttonPaste
            // 
            this.buttonPaste.Location = new System.Drawing.Point(178, 0);
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(85, 40);
            this.buttonPaste.TabIndex = 15;
            this.buttonPaste.Text = "paste";
            this.buttonPaste.UseVisualStyleBackColor = true;
            this.buttonPaste.Click += new System.EventHandler(this.buttonPaste_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(172, 32);
            this.label8.TabIndex = 14;
            this.label8.Text = "Select on field:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 46);
            this.button1.TabIndex = 20;
            this.button1.Text = "OR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(79, 95);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 46);
            this.button2.TabIndex = 21;
            this.button2.Text = "AND";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(157, 95);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 46);
            this.button3.TabIndex = 22;
            this.button3.Text = "DEL";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBoxListValues
            // 
            this.comboBoxListValues.FormattingEnabled = true;
            this.comboBoxListValues.Location = new System.Drawing.Point(527, 56);
            this.comboBoxListValues.Name = "comboBoxListValues";
            this.comboBoxListValues.Size = new System.Drawing.Size(128, 40);
            this.comboBoxListValues.TabIndex = 23;
            this.comboBoxListValues.Visible = false;
            this.comboBoxListValues.SelectedIndexChanged += new System.EventHandler(this.comboBoxListValues_SelectedIndexChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(661, 55);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(29, 37);
            this.buttonAdd.TabIndex = 25;
            this.buttonAdd.Text = "+";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Visible = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDel
            // 
            this.buttonDel.Location = new System.Drawing.Point(696, 55);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(29, 37);
            this.buttonDel.TabIndex = 26;
            this.buttonDel.Text = "-";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Visible = false;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // checkBoxNegative
            // 
            this.checkBoxNegative.AutoSize = true;
            this.checkBoxNegative.Location = new System.Drawing.Point(274, 56);
            this.checkBoxNegative.Name = "checkBoxNegative";
            this.checkBoxNegative.Size = new System.Drawing.Size(53, 36);
            this.checkBoxNegative.TabIndex = 27;
            this.checkBoxNegative.Text = "!";
            this.checkBoxNegative.UseVisualStyleBackColor = true;
            this.checkBoxNegative.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(178, 53);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 40);
            this.button4.TabIndex = 28;
            this.button4.Text = "paste";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // UserControlCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.checkBoxNegative);
            this.Controls.Add(this.buttonDel);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.comboBoxListValues);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxTypeCompare);
            this.Controls.Add(this.textBoxFilterValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxFilterFieldPath);
            this.Controls.Add(this.buttonPaste);
            this.Controls.Add(this.label8);
            this.Name = "UserControlCondition";
            this.Size = new System.Drawing.Size(1087, 150);
            this.Load += new System.EventHandler(this.UserControlCondition_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public  System.Windows.Forms.ComboBox comboBoxTypeCompare;
        public System.Windows.Forms.TextBox textBoxFilterValue;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBoxFilterFieldPath;
        public System.Windows.Forms.Button buttonPaste;
        public System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.ComboBox comboBoxListValues;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDel;
        public  System.Windows.Forms.CheckBox checkBoxNegative;
        public System.Windows.Forms.Button button4;
    }
}
