namespace TestJsonRazbor
{
    partial class FormSwaggerFromXML
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
            listViewInput = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            textBoxXMLName = new System.Windows.Forms.TextBox();
            textBoxAPIPath = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            buttonFormJson = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            listViewOutput = new System.Windows.Forms.ListView();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader7 = new System.Windows.Forms.ColumnHeader();
            textBoxNewNameInput = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            checkBox1 = new System.Windows.Forms.CheckBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            textBoxNewNameOutput = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // listViewInput
            // 
            listViewInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listViewInput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader5, columnHeader6 });
            listViewInput.FullRowSelect = true;
            listViewInput.Location = new System.Drawing.Point(0, 86);
            listViewInput.Name = "listViewInput";
            listViewInput.Size = new System.Drawing.Size(1046, 368);
            listViewInput.TabIndex = 0;
            listViewInput.UseCompatibleStateImageBehavior = false;
            listViewInput.View = System.Windows.Forms.View.Details;
            listViewInput.SelectedIndexChanged += listViewInput_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Key";
            columnHeader1.Width = 560;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Description";
            columnHeader2.Width = 400;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "NewName";
            columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Param";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(0, 51);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(99, 32);
            label1.TabIndex = 1;
            label1.Text = "Request";
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new System.Drawing.Point(1057, 88);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(151, 46);
            button1.TabIndex = 2;
            button1.Text = "Select";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(179, 32);
            label2.TabIndex = 3;
            label2.Text = "XML Command";
            label2.Click += label2_Click;
            // 
            // textBoxXMLName
            // 
            textBoxXMLName.Location = new System.Drawing.Point(197, 9);
            textBoxXMLName.Name = "textBoxXMLName";
            textBoxXMLName.Size = new System.Drawing.Size(200, 39);
            textBoxXMLName.TabIndex = 4;
            textBoxXMLName.Text = "PuPay";
            // 
            // textBoxAPIPath
            // 
            textBoxAPIPath.Location = new System.Drawing.Point(535, 12);
            textBoxAPIPath.Name = "textBoxAPIPath";
            textBoxAPIPath.Size = new System.Drawing.Size(200, 39);
            textBoxAPIPath.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(416, 12);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(101, 32);
            label3.TabIndex = 5;
            label3.Text = "API Path";
            // 
            // buttonFormJson
            // 
            buttonFormJson.Location = new System.Drawing.Point(1046, 5);
            buttonFormJson.Name = "buttonFormJson";
            buttonFormJson.Size = new System.Drawing.Size(150, 46);
            buttonFormJson.TabIndex = 7;
            buttonFormJson.Text = "Form JSON";
            buttonFormJson.UseVisualStyleBackColor = true;
            buttonFormJson.Click += buttonFormJson_Click;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button2.Location = new System.Drawing.Point(1057, 504);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(151, 46);
            button2.TabIndex = 10;
            button2.Text = "Select";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(0, 467);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(99, 32);
            label4.TabIndex = 9;
            label4.Text = "Request";
            // 
            // listViewOutput
            // 
            listViewOutput.AllowColumnReorder = true;
            listViewOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listViewOutput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader3, columnHeader4, columnHeader7 });
            listViewOutput.FullRowSelect = true;
            listViewOutput.Location = new System.Drawing.Point(0, 502);
            listViewOutput.Name = "listViewOutput";
            listViewOutput.Size = new System.Drawing.Size(1046, 368);
            listViewOutput.TabIndex = 8;
            listViewOutput.UseCompatibleStateImageBehavior = false;
            listViewOutput.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Key";
            columnHeader3.Width = 560;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Description";
            columnHeader4.Width = 400;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "NewName";
            // 
            // textBoxNewNameInput
            // 
            textBoxNewNameInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxNewNameInput.Location = new System.Drawing.Point(1057, 174);
            textBoxNewNameInput.Name = "textBoxNewNameInput";
            textBoxNewNameInput.Size = new System.Drawing.Size(139, 39);
            textBoxNewNameInput.TabIndex = 11;
            // 
            // label5
            // 
            label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(1057, 139);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(133, 32);
            label5.TabIndex = 12;
            label5.Text = "New Name";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(1057, 222);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(111, 36);
            checkBox1.TabIndex = 13;
            checkBox1.Text = "Param";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "PUT", "GET", "POST", "DELETE" });
            comboBox1.Location = new System.Drawing.Point(759, 14);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(174, 40);
            comboBox1.TabIndex = 14;
            // 
            // label6
            // 
            label6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(1057, 564);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(133, 32);
            label6.TabIndex = 16;
            label6.Text = "New Name";
            // 
            // textBoxNewNameOutput
            // 
            textBoxNewNameOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxNewNameOutput.Location = new System.Drawing.Point(1057, 599);
            textBoxNewNameOutput.Name = "textBoxNewNameOutput";
            textBoxNewNameOutput.Size = new System.Drawing.Size(139, 39);
            textBoxNewNameOutput.TabIndex = 15;
            // 
            // FormSwaggerFromXML
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1220, 895);
            Controls.Add(label6);
            Controls.Add(textBoxNewNameOutput);
            Controls.Add(comboBox1);
            Controls.Add(checkBox1);
            Controls.Add(label5);
            Controls.Add(textBoxNewNameInput);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(listViewOutput);
            Controls.Add(buttonFormJson);
            Controls.Add(textBoxAPIPath);
            Controls.Add(label3);
            Controls.Add(textBoxXMLName);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(listViewInput);
            Name = "FormSwaggerFromXML";
            Text = "FormSwaggerFromXML";
            Load += FormSwaggerFromXML_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listViewInput;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxXMLName;
        private System.Windows.Forms.TextBox textBoxAPIPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonFormJson;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listViewOutput;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox textBoxNewNameInput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxNewNameOutput;
    }
}