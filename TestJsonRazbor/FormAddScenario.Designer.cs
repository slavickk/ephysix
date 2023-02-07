namespace TestJsonRazbor
{
    partial class FormAddScenario
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxStep = new System.Windows.Forms.ComboBox();
            this.checkBoxRec = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonAddRecMoc = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.checkBoxSendEnable = new System.Windows.Forms.CheckBox();
            this.buttonDel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(669, 23);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Наименование";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(107, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(573, 23);
            this.textBox1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 61);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(719, 162);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Step";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Rec Enabled";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Moc";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Send Enabled";
            this.columnHeader4.Width = 90;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Moc";
            this.columnHeader5.Width = 80;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(686, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "New";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(11, 258);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBoxStep
            // 
            this.comboBoxStep.FormattingEnabled = true;
            this.comboBoxStep.Location = new System.Drawing.Point(11, 229);
            this.comboBoxStep.Name = "comboBoxStep";
            this.comboBoxStep.Size = new System.Drawing.Size(121, 23);
            this.comboBoxStep.TabIndex = 6;
            // 
            // checkBoxRec
            // 
            this.checkBoxRec.AutoSize = true;
            this.checkBoxRec.Location = new System.Drawing.Point(138, 229);
            this.checkBoxRec.Name = "checkBoxRec";
            this.checkBoxRec.Size = new System.Drawing.Size(107, 19);
            this.checkBoxRec.TabIndex = 7;
            this.checkBoxRec.Text = "RecMoc Enable";
            this.checkBoxRec.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(251, 227);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(106, 23);
            this.textBox2.TabIndex = 8;
            // 
            // buttonAddRecMoc
            // 
            this.buttonAddRecMoc.Location = new System.Drawing.Point(363, 227);
            this.buttonAddRecMoc.Name = "buttonAddRecMoc";
            this.buttonAddRecMoc.Size = new System.Drawing.Size(69, 23);
            this.buttonAddRecMoc.TabIndex = 9;
            this.buttonAddRecMoc.Text = "AddMoc";
            this.buttonAddRecMoc.UseVisualStyleBackColor = true;
            this.buttonAddRecMoc.Click += new System.EventHandler(this.buttonAddRecMoc_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(659, 228);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "AddMoc";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(547, 228);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(106, 23);
            this.textBox3.TabIndex = 11;
            // 
            // checkBoxSendEnable
            // 
            this.checkBoxSendEnable.AutoSize = true;
            this.checkBoxSendEnable.Location = new System.Drawing.Point(434, 230);
            this.checkBoxSendEnable.Name = "checkBoxSendEnable";
            this.checkBoxSendEnable.Size = new System.Drawing.Size(114, 19);
            this.checkBoxSendEnable.TabIndex = 10;
            this.checkBoxSendEnable.Text = "SendMoc Enable";
            this.checkBoxSendEnable.UseVisualStyleBackColor = true;
            // 
            // buttonDel
            // 
            this.buttonDel.Location = new System.Drawing.Point(686, 31);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(45, 23);
            this.buttonDel.TabIndex = 13;
            this.buttonDel.Text = "Del";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // FormAddScenario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 285);
            this.Controls.Add(this.buttonDel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.checkBoxSendEnable);
            this.Controls.Add(this.buttonAddRecMoc);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.checkBoxRec);
            this.Controls.Add(this.comboBoxStep);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "FormAddScenario";
            this.Text = "FormAddScenario";
            this.Load += new System.EventHandler(this.FormAddScenario_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxStep;
        private System.Windows.Forms.CheckBox checkBoxRec;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonAddRecMoc;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.CheckBox checkBoxSendEnable;
        private System.Windows.Forms.Button buttonDel;
    }
}