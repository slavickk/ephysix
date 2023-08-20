namespace WinFormsETLPackagedCreator
{
    partial class FormConnectFimi
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
            this.comboBoxFimiCommand = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.listBoxTableColumns = new System.Windows.Forms.ListBox();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.buttonLink = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.listViewFIMIInputParams = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSQL = new System.Windows.Forms.TextBox();
            this.comboBoxSqlColumn = new System.Windows.Forms.ComboBox();
            this.buttonAddSQLColumn = new System.Windows.Forms.Button();
            this.textBoxConstant = new System.Windows.Forms.TextBox();
            this.buttonAddConst = new System.Windows.Forms.Button();
            this.textBoxCommandPrefix = new System.Windows.Forms.TextBox();
            this.listViewFimiOutputParam = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // comboBoxFimiCommand
            // 
            this.comboBoxFimiCommand.FormattingEnabled = true;
            this.comboBoxFimiCommand.Location = new System.Drawing.Point(274, 144);
            this.comboBoxFimiCommand.Name = "comboBoxFimiCommand";
            this.comboBoxFimiCommand.Size = new System.Drawing.Size(473, 40);
            this.comboBoxFimiCommand.TabIndex = 0;
            this.comboBoxFimiCommand.SelectedIndexChanged += new System.EventHandler(this.comboBoxFimiCommand_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "FIMI Command";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(887, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select table";
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(887, 170);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(645, 40);
            this.comboBoxTable.TabIndex = 4;
            this.comboBoxTable.SelectedIndexChanged += new System.EventHandler(this.comboBoxTable_SelectedIndexChanged);
            // 
            // listBoxTableColumns
            // 
            this.listBoxTableColumns.FormattingEnabled = true;
            this.listBoxTableColumns.ItemHeight = 32;
            this.listBoxTableColumns.Location = new System.Drawing.Point(889, 242);
            this.listBoxTableColumns.Name = "listBoxTableColumns";
            this.listBoxTableColumns.Size = new System.Drawing.Size(643, 836);
            this.listBoxTableColumns.TabIndex = 5;
            this.listBoxTableColumns.SelectedIndexChanged += new System.EventHandler(this.listBoxTableColumns_SelectedIndexChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Location = new System.Drawing.Point(899, 93);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(200, 39);
            this.textBoxPrefix.TabIndex = 6;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.textBoxPrefix_TextChanged);
            this.textBoxPrefix.Leave += new System.EventHandler(this.textBoxPrefix_Leave);
            // 
            // buttonLink
            // 
            this.buttonLink.Location = new System.Drawing.Point(1382, 93);
            this.buttonLink.Name = "buttonLink";
            this.buttonLink.Size = new System.Drawing.Size(150, 46);
            this.buttonLink.TabIndex = 7;
            this.buttonLink.Text = "Link";
            this.buttonLink.UseVisualStyleBackColor = true;
            this.buttonLink.Click += new System.EventHandler(this.buttonLink_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(12, 1013);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(150, 46);
            this.buttonTest.TabIndex = 8;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // listViewFIMIInputParams
            // 
            this.listViewFIMIInputParams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewFIMIInputParams.FullRowSelect = true;
            this.listViewFIMIInputParams.Location = new System.Drawing.Point(21, 222);
            this.listViewFIMIInputParams.Name = "listViewFIMIInputParams";
            this.listViewFIMIInputParams.Size = new System.Drawing.Size(355, 458);
            this.listViewFIMIInputParams.TabIndex = 9;
            this.listViewFIMIInputParams.UseCompatibleStateImageBehavior = false;
            this.listViewFIMIInputParams.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Param";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Column";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Constant";
            this.columnHeader3.Width = 110;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 32);
            this.label3.TabIndex = 10;
            this.label3.Text = "SQL query";
            // 
            // textBoxSQL
            // 
            this.textBoxSQL.Location = new System.Drawing.Point(149, 0);
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.Size = new System.Drawing.Size(1097, 39);
            this.textBoxSQL.TabIndex = 11;
            this.textBoxSQL.Leave += new System.EventHandler(this.textBoxSQL_Leave);
            // 
            // comboBoxSqlColumn
            // 
            this.comboBoxSqlColumn.FormattingEnabled = true;
            this.comboBoxSqlColumn.Location = new System.Drawing.Point(12, 65);
            this.comboBoxSqlColumn.Name = "comboBoxSqlColumn";
            this.comboBoxSqlColumn.Size = new System.Drawing.Size(242, 40);
            this.comboBoxSqlColumn.TabIndex = 12;
            // 
            // buttonAddSQLColumn
            // 
            this.buttonAddSQLColumn.Location = new System.Drawing.Point(260, 65);
            this.buttonAddSQLColumn.Name = "buttonAddSQLColumn";
            this.buttonAddSQLColumn.Size = new System.Drawing.Size(116, 46);
            this.buttonAddSQLColumn.TabIndex = 13;
            this.buttonAddSQLColumn.Text = "Add Col";
            this.buttonAddSQLColumn.UseVisualStyleBackColor = true;
            this.buttonAddSQLColumn.Click += new System.EventHandler(this.buttonAddSQLColumn_Click);
            // 
            // textBoxConstant
            // 
            this.textBoxConstant.Location = new System.Drawing.Point(391, 72);
            this.textBoxConstant.Name = "textBoxConstant";
            this.textBoxConstant.Size = new System.Drawing.Size(200, 39);
            this.textBoxConstant.TabIndex = 14;
            // 
            // buttonAddConst
            // 
            this.buttonAddConst.Location = new System.Drawing.Point(597, 68);
            this.buttonAddConst.Name = "buttonAddConst";
            this.buttonAddConst.Size = new System.Drawing.Size(150, 46);
            this.buttonAddConst.TabIndex = 15;
            this.buttonAddConst.Text = "Add Const";
            this.buttonAddConst.UseVisualStyleBackColor = true;
            this.buttonAddConst.Click += new System.EventHandler(this.buttonAddConst_Click);
            // 
            // textBoxCommandPrefix
            // 
            this.textBoxCommandPrefix.Location = new System.Drawing.Point(34, 145);
            this.textBoxCommandPrefix.Name = "textBoxCommandPrefix";
            this.textBoxCommandPrefix.Size = new System.Drawing.Size(220, 39);
            this.textBoxCommandPrefix.TabIndex = 16;
            this.textBoxCommandPrefix.Leave += new System.EventHandler(this.textBoxCommandPrefix_Leave);
            // 
            // listViewFimiOutputParam
            // 
            this.listViewFimiOutputParam.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.listViewFimiOutputParam.FullRowSelect = true;
            this.listViewFimiOutputParam.Location = new System.Drawing.Point(392, 222);
            this.listViewFimiOutputParam.Name = "listViewFimiOutputParam";
            this.listViewFimiOutputParam.Size = new System.Drawing.Size(459, 458);
            this.listViewFimiOutputParam.TabIndex = 17;
            this.listViewFimiOutputParam.UseCompatibleStateImageBehavior = false;
            this.listViewFimiOutputParam.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Path";
            this.columnHeader4.Width = 350;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Value";
            this.columnHeader5.Width = 100;
            // 
            // FormConnectFimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1544, 1099);
            this.Controls.Add(this.listViewFimiOutputParam);
            this.Controls.Add(this.textBoxCommandPrefix);
            this.Controls.Add(this.buttonAddConst);
            this.Controls.Add(this.textBoxConstant);
            this.Controls.Add(this.buttonAddSQLColumn);
            this.Controls.Add(this.comboBoxSqlColumn);
            this.Controls.Add(this.textBoxSQL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewFIMIInputParams);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonLink);
            this.Controls.Add(this.textBoxPrefix);
            this.Controls.Add(this.listBoxTableColumns);
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxFimiCommand);
            this.Name = "FormConnectFimi";
            this.Text = "FormConnectFimi";
            this.Load += new System.EventHandler(this.FormConnectFimi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox comboBoxFimiCommand;
        private Label label1;
        private Label label2;
        private ComboBox comboBoxTable;
        private ListBox listBoxTableColumns;
        private TextBox textBoxPrefix;
        private Button buttonLink;
        private Button buttonTest;
        private ListView listViewFIMIInputParams;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Label label3;
        private TextBox textBoxSQL;
        private ComboBox comboBoxSqlColumn;
        private Button buttonAddSQLColumn;
        private TextBox textBoxConstant;
        private Button buttonAddConst;
        private TextBox textBoxCommandPrefix;
        private ListView listViewFimiOutputParam;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
    }
}