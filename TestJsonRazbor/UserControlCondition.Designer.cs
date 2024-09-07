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
            comboBoxTypeCompare = new System.Windows.Forms.ComboBox();
            textBoxFilterValue = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            textBoxFilterFieldPath = new System.Windows.Forms.TextBox();
            buttonPaste = new System.Windows.Forms.Button();
            label8 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            comboBoxListValues = new System.Windows.Forms.ComboBox();
            buttonAdd = new System.Windows.Forms.Button();
            buttonDel = new System.Windows.Forms.Button();
            checkBoxNegative = new System.Windows.Forms.CheckBox();
            button4 = new System.Windows.Forms.Button();
            buttonEQ = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // comboBoxTypeCompare
            // 
            comboBoxTypeCompare.FormattingEnabled = true;
            comboBoxTypeCompare.Items.AddRange(new object[] { "value", "path only", "script", "list" });
            comboBoxTypeCompare.Location = new System.Drawing.Point(749, 56);
            comboBoxTypeCompare.Name = "comboBoxTypeCompare";
            comboBoxTypeCompare.Size = new System.Drawing.Size(141, 40);
            comboBoxTypeCompare.TabIndex = 19;
            comboBoxTypeCompare.SelectedIndexChanged += comboBoxTypeCompare_SelectedIndexChanged;
            // 
            // textBoxFilterValue
            // 
            textBoxFilterValue.Location = new System.Drawing.Point(330, 57);
            textBoxFilterValue.Name = "textBoxFilterValue";
            textBoxFilterValue.Size = new System.Drawing.Size(191, 39);
            textBoxFilterValue.TabIndex = 18;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(8, 60);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(164, 32);
            label3.TabIndex = 17;
            label3.Text = "Searched text:";
            // 
            // textBoxFilterFieldPath
            // 
            textBoxFilterFieldPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxFilterFieldPath.Location = new System.Drawing.Point(274, 1);
            textBoxFilterFieldPath.Name = "textBoxFilterFieldPath";
            textBoxFilterFieldPath.Size = new System.Drawing.Size(616, 39);
            textBoxFilterFieldPath.TabIndex = 16;
            textBoxFilterFieldPath.TextChanged += textBoxFilterFieldPath_TextChanged;
            // 
            // buttonPaste
            // 
            buttonPaste.Location = new System.Drawing.Point(178, 0);
            buttonPaste.Name = "buttonPaste";
            buttonPaste.Size = new System.Drawing.Size(85, 40);
            buttonPaste.TabIndex = 15;
            buttonPaste.Text = "paste";
            buttonPaste.UseVisualStyleBackColor = true;
            buttonPaste.Click += buttonPaste_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(3, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(172, 32);
            label8.TabIndex = 14;
            label8.Text = "Select on field:";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(8, 95);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(65, 46);
            button1.TabIndex = 20;
            button1.Text = "OR";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(79, 95);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(72, 46);
            button2.TabIndex = 21;
            button2.Text = "AND";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(348, 95);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(72, 46);
            button3.TabIndex = 22;
            button3.Text = "DEL";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // comboBoxListValues
            // 
            comboBoxListValues.FormattingEnabled = true;
            comboBoxListValues.Location = new System.Drawing.Point(527, 56);
            comboBoxListValues.Name = "comboBoxListValues";
            comboBoxListValues.Size = new System.Drawing.Size(128, 40);
            comboBoxListValues.TabIndex = 23;
            comboBoxListValues.Visible = false;
            comboBoxListValues.SelectedIndexChanged += comboBoxListValues_SelectedIndexChanged;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new System.Drawing.Point(661, 55);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new System.Drawing.Size(29, 37);
            buttonAdd.TabIndex = 25;
            buttonAdd.Text = "+";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Visible = false;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDel
            // 
            buttonDel.Location = new System.Drawing.Point(696, 55);
            buttonDel.Name = "buttonDel";
            buttonDel.Size = new System.Drawing.Size(29, 37);
            buttonDel.TabIndex = 26;
            buttonDel.Text = "-";
            buttonDel.UseVisualStyleBackColor = true;
            buttonDel.Visible = false;
            buttonDel.Click += buttonDel_Click;
            // 
            // checkBoxNegative
            // 
            checkBoxNegative.AutoSize = true;
            checkBoxNegative.Location = new System.Drawing.Point(274, 56);
            checkBoxNegative.Name = "checkBoxNegative";
            checkBoxNegative.Size = new System.Drawing.Size(53, 36);
            checkBoxNegative.TabIndex = 27;
            checkBoxNegative.Text = "!";
            checkBoxNegative.UseVisualStyleBackColor = true;
            checkBoxNegative.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(178, 53);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(85, 40);
            button4.TabIndex = 28;
            button4.Text = "paste";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // buttonEQ
            // 
            buttonEQ.Location = new System.Drawing.Point(156, 95);
            buttonEQ.Name = "buttonEQ";
            buttonEQ.Size = new System.Drawing.Size(65, 46);
            buttonEQ.TabIndex = 29;
            buttonEQ.Text = "EQ";
            buttonEQ.UseVisualStyleBackColor = true;
            buttonEQ.Click += buttonEQ_Click;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(227, 95);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(115, 46);
            button5.TabIndex = 30;
            button5.Text = "NOT_EQ";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // UserControlCondition
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(button5);
            Controls.Add(buttonEQ);
            Controls.Add(button4);
            Controls.Add(checkBoxNegative);
            Controls.Add(buttonDel);
            Controls.Add(buttonAdd);
            Controls.Add(comboBoxListValues);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(comboBoxTypeCompare);
            Controls.Add(textBoxFilterValue);
            Controls.Add(label3);
            Controls.Add(textBoxFilterFieldPath);
            Controls.Add(buttonPaste);
            Controls.Add(label8);
            Name = "UserControlCondition";
            Size = new System.Drawing.Size(1087, 150);
            Load += UserControlCondition_Load;
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Button buttonEQ;
        private System.Windows.Forms.Button button5;
    }
}
