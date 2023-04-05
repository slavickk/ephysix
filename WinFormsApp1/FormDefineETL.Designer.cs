namespace WinFormsApp1
{
    partial class FormDefineETL
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
            this.textBoxOutputName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxETLName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxAddPar = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDestTables = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAddParameter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxOutputName
            // 
            this.textBoxOutputName.Location = new System.Drawing.Point(189, 51);
            this.textBoxOutputName.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxOutputName.Name = "textBoxOutputName";
            this.textBoxOutputName.Size = new System.Drawing.Size(184, 23);
            this.textBoxOutputName.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 30);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 23;
            this.label6.Text = "outputName";
            // 
            // textBoxETLName
            // 
            this.textBoxETLName.Location = new System.Drawing.Point(188, 3);
            this.textBoxETLName.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxETLName.Name = "textBoxETLName";
            this.textBoxETLName.Size = new System.Drawing.Size(184, 23);
            this.textBoxETLName.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 15);
            this.label5.TabIndex = 21;
            this.label5.Text = "Имя ETL пакета";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(391, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 20);
            this.button1.TabIndex = 25;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 218);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 26;
            this.label1.Text = "Описание";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(7, 236);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(454, 124);
            this.textBox1.TabIndex = 27;
            // 
            // textBoxAddPar
            // 
            this.textBoxAddPar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddPar.Location = new System.Drawing.Point(615, 221);
            this.textBoxAddPar.Multiline = true;
            this.textBoxAddPar.Name = "textBoxAddPar";
            this.textBoxAddPar.Size = new System.Drawing.Size(72, 39);
            this.textBoxAddPar.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 15);
            this.label2.TabIndex = 28;
            this.label2.Text = "Тип выходной таблицы/потока";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(188, 75);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(184, 23);
            this.comboBox1.TabIndex = 29;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(233, 103);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(39, 23);
            this.numericUpDown1.TabIndex = 30;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.Maximum = 100;

            this.numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 15);
            this.label3.TabIndex = 31;
            this.label3.Text = "Additional par ";
            this.label3.Visible = false;
            // 
            // comboBoxDestTables
            // 
            this.comboBoxDestTables.FormattingEnabled = true;
            this.comboBoxDestTables.Location = new System.Drawing.Point(189, 29);
            this.comboBoxDestTables.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.comboBoxDestTables.Name = "comboBoxDestTables";
            this.comboBoxDestTables.Size = new System.Drawing.Size(183, 23);
            this.comboBoxDestTables.TabIndex = 32;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(7, 51);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(47, 22);
            this.buttonAdd.TabIndex = 33;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDel
            // 
            this.buttonDel.Location = new System.Drawing.Point(59, 51);
            this.buttonDel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(47, 22);
            this.buttonDel.TabIndex = 34;
            this.buttonDel.Text = "Del";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 15);
            this.label4.TabIndex = 35;
            this.label4.Text = "KeyCount";
            // 
            // textBoxAddParameter
            // 
            this.textBoxAddParameter.Location = new System.Drawing.Point(7, 123);
            this.textBoxAddParameter.Multiline = true;
            this.textBoxAddParameter.Name = "textBoxAddParameter";
            this.textBoxAddParameter.Size = new System.Drawing.Size(454, 92);
            this.textBoxAddParameter.TabIndex = 36;
            // 
            // FormDefineETL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 364);
            this.Controls.Add(this.textBoxAddParameter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonDel);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.comboBoxDestTables);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxOutputName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxETLName);
            this.Controls.Add(this.label5);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "FormDefineETL";
            this.Text = "FormDefineETL";
            this.Load += new System.EventHandler(this.FormDefineETL_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private TextBox textBoxOutputName;
        private Label label6;
        private TextBox textBoxETLName;
        private Label label5;
        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBoxAddPar;
        private Label label2;
        private ComboBox comboBox1;
        private NumericUpDown numericUpDown1;
        private Label label3;
        private ComboBox comboBoxDestTables;
        private Button buttonAdd;
        private Button buttonDel;
        private Label label4;
        private TextBox textBoxAddParameter;
    }
}