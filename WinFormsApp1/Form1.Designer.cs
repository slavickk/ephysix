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
            this.panel1 = new System.Windows.Forms.Panel();
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.comboBoxDestTable = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFromSrc = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя поля(подстрока)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(256, 9);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(190, 39);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(451, 9);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 43);
            this.button1.TabIndex = 2;
            this.button1.Text = "Искать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(19, 64);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(426, 40);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 209);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 43);
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
            this.listViewSelectedField.Location = new System.Drawing.Point(19, 299);
            this.listViewSelectedField.Margin = new System.Windows.Forms.Padding(4);
            this.listViewSelectedField.MultiSelect = false;
            this.listViewSelectedField.Name = "listViewSelectedField";
            this.listViewSelectedField.Size = new System.Drawing.Size(574, 181);
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
            this.listViewLinks.Location = new System.Drawing.Point(615, 299);
            this.listViewLinks.Margin = new System.Windows.Forms.Padding(4);
            this.listViewLinks.MultiSelect = false;
            this.listViewLinks.Name = "listViewLinks";
            this.listViewLinks.Size = new System.Drawing.Size(712, 181);
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
            this.label2.Location = new System.Drawing.Point(19, 252);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 32);
            this.label2.TabIndex = 7;
            this.label2.Text = "Поля";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(615, 252);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 32);
            this.label3.TabIndex = 8;
            this.label3.Text = "Связи";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 491);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(357, 32);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дополнительные ограничения";
            // 
            // textBoxCondition
            // 
            this.textBoxCondition.Location = new System.Drawing.Point(191, 527);
            this.textBoxCondition.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCondition.Name = "textBoxCondition";
            this.textBoxCondition.Size = new System.Drawing.Size(364, 39);
            this.textBoxCondition.TabIndex = 11;
            // 
            // buttonAddCondition
            // 
            this.buttonAddCondition.Enabled = false;
            this.buttonAddCondition.Location = new System.Drawing.Point(565, 521);
            this.buttonAddCondition.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddCondition.Name = "buttonAddCondition";
            this.buttonAddCondition.Size = new System.Drawing.Size(74, 43);
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
            this.listViewAddCondition.Location = new System.Drawing.Point(19, 580);
            this.listViewAddCondition.Margin = new System.Windows.Forms.Padding(4);
            this.listViewAddCondition.MultiSelect = false;
            this.listViewAddCondition.Name = "listViewAddCondition";
            this.listViewAddCondition.Size = new System.Drawing.Size(736, 264);
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
            this.tableViewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableViewControl1.AutoSize = true;
            this.tableViewControl1.Location = new System.Drawing.Point(722, 491);
            this.tableViewControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tableViewControl1.Name = "tableViewControl1";
            this.tableViewControl1.Size = new System.Drawing.Size(583, 489);
            this.tableViewControl1.TabIndex = 14;
            // 
            // textBoxTableAdditional
            // 
            this.textBoxTableAdditional.Enabled = false;
            this.textBoxTableAdditional.Location = new System.Drawing.Point(19, 527);
            this.textBoxTableAdditional.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTableAdditional.Name = "textBoxTableAdditional";
            this.textBoxTableAdditional.Size = new System.Drawing.Size(164, 39);
            this.textBoxTableAdditional.TabIndex = 15;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1177, 9);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(141, 43);
            this.button3.TabIndex = 18;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxFieldName
            // 
            this.textBoxFieldName.Location = new System.Drawing.Point(93, 113);
            this.textBoxFieldName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFieldName.Name = "textBoxFieldName";
            this.textBoxFieldName.Size = new System.Drawing.Size(190, 39);
            this.textBoxFieldName.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 115);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 32);
            this.label7.TabIndex = 22;
            this.label7.Text = "Поле";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(290, 113);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 32);
            this.label8.TabIndex = 23;
            this.label8.Text = "Алиас";
            // 
            // textBoxColumnAlias
            // 
            this.textBoxColumnAlias.Location = new System.Drawing.Point(373, 113);
            this.textBoxColumnAlias.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxColumnAlias.Name = "textBoxColumnAlias";
            this.textBoxColumnAlias.Size = new System.Drawing.Size(190, 39);
            this.textBoxColumnAlias.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 171);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 32);
            this.label9.TabIndex = 25;
            this.label9.Text = "Таблица";
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Enabled = false;
            this.textBoxTableName.Location = new System.Drawing.Point(128, 164);
            this.textBoxTableName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.Size = new System.Drawing.Size(154, 39);
            this.textBoxTableName.TabIndex = 26;
            // 
            // textBoxTableAlias
            // 
            this.textBoxTableAlias.Location = new System.Drawing.Point(373, 166);
            this.textBoxTableAlias.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTableAlias.Name = "textBoxTableAlias";
            this.textBoxTableAlias.Size = new System.Drawing.Size(190, 39);
            this.textBoxTableAlias.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(290, 166);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 32);
            this.label10.TabIndex = 27;
            this.label10.Text = "Алиас";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(615, 105);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(159, 32);
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
            this.listViewVariableList.Location = new System.Drawing.Point(615, 141);
            this.listViewVariableList.Margin = new System.Windows.Forms.Padding(4);
            this.listViewVariableList.MultiSelect = false;
            this.listViewVariableList.Name = "listViewVariableList";
            this.listViewVariableList.Size = new System.Drawing.Size(704, 162);
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
            this.button4.Location = new System.Drawing.Point(1027, 96);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(141, 43);
            this.button4.TabIndex = 31;
            this.button4.Text = "Add var";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBoxEtlDescr
            // 
            this.textBoxEtlDescr.Enabled = false;
            this.textBoxEtlDescr.Location = new System.Drawing.Point(743, 13);
            this.textBoxEtlDescr.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEtlDescr.Name = "textBoxEtlDescr";
            this.textBoxEtlDescr.Size = new System.Drawing.Size(427, 39);
            this.textBoxEtlDescr.TabIndex = 32;
            this.textBoxEtlDescr.Text = "not defined!!!";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(665, 13);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(71, 43);
            this.button5.TabIndex = 33;
            this.button5.Text = "Set";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBoxPackage
            // 
            this.comboBoxPackage.FormattingEnabled = true;
            this.comboBoxPackage.Location = new System.Drawing.Point(743, 55);
            this.comboBoxPackage.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxPackage.Name = "comboBoxPackage";
            this.comboBoxPackage.Size = new System.Drawing.Size(429, 40);
            this.comboBoxPackage.TabIndex = 34;
            this.comboBoxPackage.SelectedIndexChanged += new System.EventHandler(this.comboBoxPackage_SelectedIndexChanged);
            // 
            // buttonEditCondition
            // 
            this.buttonEditCondition.Enabled = false;
            this.buttonEditCondition.Location = new System.Drawing.Point(646, 521);
            this.buttonEditCondition.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEditCondition.Name = "buttonEditCondition";
            this.buttonEditCondition.Size = new System.Drawing.Size(74, 43);
            this.buttonEditCondition.TabIndex = 35;
            this.buttonEditCondition.Text = "Mod";
            this.buttonEditCondition.UseVisualStyleBackColor = true;
            this.buttonEditCondition.Click += new System.EventHandler(this.buttonEditCondition_Click);
            // 
            // buttonEditField
            // 
            this.buttonEditField.Enabled = false;
            this.buttonEditField.Location = new System.Drawing.Point(176, 209);
            this.buttonEditField.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEditField.Name = "buttonEditField";
            this.buttonEditField.Size = new System.Drawing.Size(141, 43);
            this.buttonEditField.TabIndex = 36;
            this.buttonEditField.Text = "Изменить";
            this.buttonEditField.UseVisualStyleBackColor = true;
            this.buttonEditField.Click += new System.EventHandler(this.buttonEditField_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1176, 96);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(141, 43);
            this.button6.TabIndex = 37;
            this.button6.Text = "Mod var";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(665, 53);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(71, 43);
            this.buttonDelete.TabIndex = 38;
            this.buttonDelete.Text = "Del";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1177, 51);
            this.button7.Margin = new System.Windows.Forms.Padding(6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(139, 49);
            this.button7.TabIndex = 39;
            this.button7.Text = "Explore";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(451, 58);
            this.button8.Margin = new System.Windows.Forms.Padding(6);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(139, 49);
            this.button8.TabIndex = 40;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.webView21);
            this.panel1.Cursor = System.Windows.Forms.Cursors.NoMoveVert;
            this.panel1.Location = new System.Drawing.Point(19, 860);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1311, 499);
            this.panel1.TabIndex = 42;
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView21.Location = new System.Drawing.Point(0, 0);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(1311, 499);
            this.webView21.TabIndex = 0;
            this.webView21.ZoomFactor = 1D;
            // 
            // comboBoxDestTable
            // 
            this.comboBoxDestTable.FormattingEnabled = true;
            this.comboBoxDestTable.Location = new System.Drawing.Point(373, 212);
            this.comboBoxDestTable.Name = "comboBoxDestTable";
            this.comboBoxDestTable.Size = new System.Drawing.Size(190, 40);
            this.comboBoxDestTable.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(373, 252);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 32);
            this.label5.TabIndex = 44;
            this.label5.Text = "fromSrc";
            // 
            // textBoxFromSrc
            // 
            this.textBoxFromSrc.Location = new System.Drawing.Point(476, 252);
            this.textBoxFromSrc.Name = "textBoxFromSrc";
            this.textBoxFromSrc.Size = new System.Drawing.Size(87, 39);
            this.textBoxFromSrc.TabIndex = 45;
            this.textBoxFromSrc.Text = "1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 1361);
            this.Controls.Add(this.textBoxFromSrc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxDestTable);
            this.Controls.Add(this.panel1);
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
            this.Controls.Add(this.tableViewControl1);
            this.Controls.Add(this.listViewAddCondition);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "FormEtlCreator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
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
        private Panel panel1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private ColumnHeader columnHeader14;
        private ComboBox comboBoxDestTable;
        private Label label5;
        private TextBox textBoxFromSrc;
    }
}