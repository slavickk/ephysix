namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listViewSelectedField = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.listViewLinks = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxCondition = new System.Windows.Forms.TextBox();
            this.buttonAddCondition = new System.Windows.Forms.Button();
            this.listViewAddCondition = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.tableViewControl1 = new WinFormsApp1.TableViewControl();
            this.textBoxTableAdditional = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxFieldName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxColumnAlias = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxTableName = new System.Windows.Forms.TextBox();
            this.textBoxTableAlias = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.listViewVariableList = new System.Windows.Forms.ListView();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxEtlDescr = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBoxPackage = new System.Windows.Forms.ComboBox();
            this.buttonEditCondition = new System.Windows.Forms.Button();
            this.buttonEditField = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.comboBoxDestTable = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFromSrc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя поля(подстрока)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(138, 4);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(104, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(243, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "Искать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(10, 30);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(231, 23);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 98);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 20);
            this.button2.TabIndex = 4;
            this.button2.Text = "Добавить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listViewSelectedField
            // 
            this.listViewSelectedField.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader9,
            this.columnHeader2,
            this.columnHeader8,
            this.columnHeader14});
            this.listViewSelectedField.FullRowSelect = true;
            this.listViewSelectedField.Location = new System.Drawing.Point(10, 151);
            this.listViewSelectedField.Margin = new System.Windows.Forms.Padding(2);
            this.listViewSelectedField.MultiSelect = false;
            this.listViewSelectedField.Name = "listViewSelectedField";
            this.listViewSelectedField.Size = new System.Drawing.Size(311, 153);
            this.listViewSelectedField.TabIndex = 5;
            this.listViewSelectedField.UseCompatibleStateImageBehavior = false;
            this.listViewSelectedField.View = System.Windows.Forms.View.Details;
            this.listViewSelectedField.SelectedIndexChanged += new System.EventHandler(this.listViewSelectedField_SelectedIndexChanged);
            this.listViewSelectedField.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewSelectedField_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Поле";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Alias";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Исх.табл.";
            this.columnHeader2.Width = 160;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Alias";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Dest table";
            this.columnHeader14.Width = 200;
            // 
            // listViewLinks
            // 
            this.listViewLinks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewLinks.FullRowSelect = true;
            this.listViewLinks.Location = new System.Drawing.Point(331, 151);
            this.listViewLinks.Margin = new System.Windows.Forms.Padding(2);
            this.listViewLinks.MultiSelect = false;
            this.listViewLinks.Name = "listViewLinks";
            this.listViewLinks.Size = new System.Drawing.Size(385, 153);
            this.listViewLinks.TabIndex = 6;
            this.listViewLinks.UseCompatibleStateImageBehavior = false;
            this.listViewLinks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Таблица1";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Таблица2";
            this.columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Связь";
            this.columnHeader5.Width = 260;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 132);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Поля";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(331, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Связи";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 311);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дополнительные ограничения";
            // 
            // textBoxCondition
            // 
            this.textBoxCondition.Location = new System.Drawing.Point(103, 328);
            this.textBoxCondition.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCondition.Name = "textBoxCondition";
            this.textBoxCondition.Size = new System.Drawing.Size(198, 23);
            this.textBoxCondition.TabIndex = 11;
            // 
            // buttonAddCondition
            // 
            this.buttonAddCondition.Enabled = false;
            this.buttonAddCondition.Location = new System.Drawing.Point(304, 325);
            this.buttonAddCondition.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddCondition.Name = "buttonAddCondition";
            this.buttonAddCondition.Size = new System.Drawing.Size(40, 20);
            this.buttonAddCondition.TabIndex = 12;
            this.buttonAddCondition.Text = "Add";
            this.buttonAddCondition.UseVisualStyleBackColor = true;
            this.buttonAddCondition.Click += new System.EventHandler(this.buttonAddCondition_Click);
            // 
            // listViewAddCondition
            // 
            this.listViewAddCondition.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7});
            this.listViewAddCondition.FullRowSelect = true;
            this.listViewAddCondition.Location = new System.Drawing.Point(10, 354);
            this.listViewAddCondition.Margin = new System.Windows.Forms.Padding(2);
            this.listViewAddCondition.MultiSelect = false;
            this.listViewAddCondition.Name = "listViewAddCondition";
            this.listViewAddCondition.Size = new System.Drawing.Size(376, 126);
            this.listViewAddCondition.TabIndex = 13;
            this.listViewAddCondition.UseCompatibleStateImageBehavior = false;
            this.listViewAddCondition.View = System.Windows.Forms.View.Details;
            this.listViewAddCondition.SelectedIndexChanged += new System.EventHandler(this.listViewAddCondition_SelectedIndexChanged);
            this.listViewAddCondition.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewAddCondition_MouseDoubleClick);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Таблица";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Условие";
            this.columnHeader7.Width = 400;
            // 
            // tableViewControl1
            // 
            this.tableViewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableViewControl1.AutoSize = true;
            this.tableViewControl1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.tableViewControl1.Location = new System.Drawing.Point(402, 330);
            this.tableViewControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tableViewControl1.Name = "tableViewControl1";
            this.tableViewControl1.Size = new System.Drawing.Size(310, 348);
            this.tableViewControl1.TabIndex = 14;
            // 
            // textBoxTableAdditional
            // 
            this.textBoxTableAdditional.Enabled = false;
            this.textBoxTableAdditional.Location = new System.Drawing.Point(10, 328);
            this.textBoxTableAdditional.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTableAdditional.Name = "textBoxTableAdditional";
            this.textBoxTableAdditional.Size = new System.Drawing.Size(90, 23);
            this.textBoxTableAdditional.TabIndex = 15;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(634, 4);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(76, 20);
            this.button3.TabIndex = 18;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxFieldName
            // 
            this.textBoxFieldName.Location = new System.Drawing.Point(50, 53);
            this.textBoxFieldName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxFieldName.Name = "textBoxFieldName";
            this.textBoxFieldName.Size = new System.Drawing.Size(104, 23);
            this.textBoxFieldName.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 54);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "Поле";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(156, 53);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 15);
            this.label8.TabIndex = 23;
            this.label8.Text = "Алиас";
            // 
            // textBoxColumnAlias
            // 
            this.textBoxColumnAlias.Location = new System.Drawing.Point(201, 53);
            this.textBoxColumnAlias.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxColumnAlias.Name = "textBoxColumnAlias";
            this.textBoxColumnAlias.Size = new System.Drawing.Size(104, 23);
            this.textBoxColumnAlias.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 80);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 15);
            this.label9.TabIndex = 25;
            this.label9.Text = "Таблица";
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Enabled = false;
            this.textBoxTableName.Location = new System.Drawing.Point(69, 77);
            this.textBoxTableName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.Size = new System.Drawing.Size(85, 23);
            this.textBoxTableName.TabIndex = 26;
            // 
            // textBoxTableAlias
            // 
            this.textBoxTableAlias.Location = new System.Drawing.Point(201, 78);
            this.textBoxTableAlias.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTableAlias.Name = "textBoxTableAlias";
            this.textBoxTableAlias.Size = new System.Drawing.Size(104, 23);
            this.textBoxTableAlias.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(156, 78);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 15);
            this.label10.TabIndex = 27;
            this.label10.Text = "Алиас";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(331, 49);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 15);
            this.label11.TabIndex = 29;
            this.label11.Text = "Переменные";
            // 
            // listViewVariableList
            // 
            this.listViewVariableList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewVariableList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader11});
            this.listViewVariableList.FullRowSelect = true;
            this.listViewVariableList.Location = new System.Drawing.Point(331, 66);
            this.listViewVariableList.Margin = new System.Windows.Forms.Padding(2);
            this.listViewVariableList.MultiSelect = false;
            this.listViewVariableList.Name = "listViewVariableList";
            this.listViewVariableList.Size = new System.Drawing.Size(381, 78);
            this.listViewVariableList.TabIndex = 30;
            this.listViewVariableList.UseCompatibleStateImageBehavior = false;
            this.listViewVariableList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Name";
            this.columnHeader10.Width = 120;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Type";
            this.columnHeader12.Width = 120;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "DefValue";
            this.columnHeader13.Width = 160;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Detail";
            this.columnHeader11.Width = 400;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(553, 45);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(76, 20);
            this.button4.TabIndex = 31;
            this.button4.Text = "Add var";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBoxEtlDescr
            // 
            this.textBoxEtlDescr.Enabled = false;
            this.textBoxEtlDescr.Location = new System.Drawing.Point(400, 6);
            this.textBoxEtlDescr.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEtlDescr.Name = "textBoxEtlDescr";
            this.textBoxEtlDescr.Size = new System.Drawing.Size(232, 23);
            this.textBoxEtlDescr.TabIndex = 32;
            this.textBoxEtlDescr.Text = "not defined!!!";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(358, 6);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(38, 20);
            this.button5.TabIndex = 33;
            this.button5.Text = "Set";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBoxPackage
            // 
            this.comboBoxPackage.FormattingEnabled = true;
            this.comboBoxPackage.Location = new System.Drawing.Point(400, 26);
            this.comboBoxPackage.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxPackage.Name = "comboBoxPackage";
            this.comboBoxPackage.Size = new System.Drawing.Size(233, 23);
            this.comboBoxPackage.TabIndex = 34;
            this.comboBoxPackage.SelectedIndexChanged += new System.EventHandler(this.comboBoxPackage_SelectedIndexChanged);
            // 
            // buttonEditCondition
            // 
            this.buttonEditCondition.Enabled = false;
            this.buttonEditCondition.Location = new System.Drawing.Point(348, 325);
            this.buttonEditCondition.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEditCondition.Name = "buttonEditCondition";
            this.buttonEditCondition.Size = new System.Drawing.Size(40, 20);
            this.buttonEditCondition.TabIndex = 35;
            this.buttonEditCondition.Text = "Mod";
            this.buttonEditCondition.UseVisualStyleBackColor = true;
            this.buttonEditCondition.Click += new System.EventHandler(this.buttonEditCondition_Click);
            // 
            // buttonEditField
            // 
            this.buttonEditField.Enabled = false;
            this.buttonEditField.Location = new System.Drawing.Point(95, 98);
            this.buttonEditField.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEditField.Name = "buttonEditField";
            this.buttonEditField.Size = new System.Drawing.Size(76, 20);
            this.buttonEditField.TabIndex = 36;
            this.buttonEditField.Text = "Изменить";
            this.buttonEditField.UseVisualStyleBackColor = true;
            this.buttonEditField.Click += new System.EventHandler(this.buttonEditField_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(633, 45);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(76, 20);
            this.button6.TabIndex = 37;
            this.button6.Text = "Mod var";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(358, 25);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(38, 20);
            this.buttonDelete.TabIndex = 38;
            this.buttonDelete.Text = "Del";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(634, 24);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 39;
            this.button7.Text = "Explore";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(243, 27);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 40;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // comboBoxDestTable
            // 
            this.comboBoxDestTable.FormattingEnabled = true;
            this.comboBoxDestTable.Location = new System.Drawing.Point(201, 99);
            this.comboBoxDestTable.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.comboBoxDestTable.Name = "comboBoxDestTable";
            this.comboBoxDestTable.Size = new System.Drawing.Size(104, 23);
            this.comboBoxDestTable.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 129);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 15);
            this.label5.TabIndex = 44;
            this.label5.Text = "fromSrc";
            // 
            // textBoxFromSrc
            // 
            this.textBoxFromSrc.Location = new System.Drawing.Point(256, 125);
            this.textBoxFromSrc.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBoxFromSrc.Name = "textBoxFromSrc";
            this.textBoxFromSrc.Size = new System.Drawing.Size(49, 23);
            this.textBoxFromSrc.TabIndex = 45;
            this.textBoxFromSrc.Text = "1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 706);
            this.Controls.Add(this.tableViewControl1);
            this.Controls.Add(this.listViewAddCondition);
            this.Controls.Add(this.textBoxFromSrc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxDestTable);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.buttonEditField);
            this.Controls.Add(this.buttonEditCondition);
            this.Controls.Add(this.comboBoxPackage);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBoxEtlDescr);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.listViewVariableList);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxTableAlias);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxTableName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxColumnAlias);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxFieldName);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxTableAdditional);
            this.Controls.Add(this.buttonAddCondition);
            this.Controls.Add(this.textBoxCondition);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewLinks);
            this.Controls.Add(this.listViewSelectedField);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "FormEtlCreator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private ComboBox comboBox1;
        private Button button2;
        private ListView listViewSelectedField;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ListView listViewLinks;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBoxCondition;
        private Button buttonAddCondition;
        private ListView listViewAddCondition;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private TableViewControl tableViewControl1;
        private TextBox textBoxTableAdditional;
        private Button button3;
        private ColumnHeader columnHeader9;
        private TextBox textBoxFieldName;
        private Label label7;
        private Label label8;
        private TextBox textBoxColumnAlias;
        private Label label9;
        private TextBox textBoxTableName;
        private TextBox textBoxTableAlias;
        private Label label10;
        private Label label11;
        private ListView listViewVariableList;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private Button button4;
        private TextBox textBoxEtlDescr;
        private Button button5;
        private ComboBox comboBoxPackage;
        private Button buttonEditCondition;
        private Button buttonEditField;
        private Button button6;
        private Button buttonDelete;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader13;
        private Button button7;
        private Button button8;
        private ColumnHeader columnHeader14;
        private ComboBox comboBoxDestTable;
        private Label label5;
        private TextBox textBoxFromSrc;
    }
}