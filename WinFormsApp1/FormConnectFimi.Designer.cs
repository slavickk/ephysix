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
            this.buttonAddColumnToTable = new System.Windows.Forms.Button();
            this.buttonAddConstantToTable = new System.Windows.Forms.Button();
            this.listViewTableColumns = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.buttonFullTest = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxAlternatives = new System.Windows.Forms.ComboBox();
            this.buttonMXGraph = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxFimiCommand
            // 
            this.comboBoxFimiCommand.FormattingEnabled = true;
            this.comboBoxFimiCommand.Location = new System.Drawing.Point(273, 188);
            this.comboBoxFimiCommand.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.comboBoxFimiCommand.Name = "comboBoxFimiCommand";
            this.comboBoxFimiCommand.Size = new System.Drawing.Size(474, 40);
            this.comboBoxFimiCommand.TabIndex = 0;
            this.comboBoxFimiCommand.SelectedIndexChanged += new System.EventHandler(this.comboBoxFimiCommand_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 151);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "API Command";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1309, 117);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select table";
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(1307, 151);
            this.comboBoxTable.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(645, 40);
            this.comboBoxTable.TabIndex = 4;
            this.comboBoxTable.SelectedIndexChanged += new System.EventHandler(this.comboBoxTable_SelectedIndexChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Location = new System.Drawing.Point(1307, 66);
            this.textBoxPrefix.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(201, 39);
            this.textBoxPrefix.TabIndex = 6;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.textBoxPrefix_TextChanged);
            this.textBoxPrefix.Leave += new System.EventHandler(this.textBoxPrefix_Leave);
            // 
            // buttonLink
            // 
            this.buttonLink.Location = new System.Drawing.Point(1805, 205);
            this.buttonLink.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonLink.Name = "buttonLink";
            this.buttonLink.Size = new System.Drawing.Size(150, 47);
            this.buttonLink.TabIndex = 7;
            this.buttonLink.Text = "Link";
            this.buttonLink.UseVisualStyleBackColor = true;
            this.buttonLink.Click += new System.EventHandler(this.buttonLink_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(19, 751);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(150, 47);
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
            this.listViewFIMIInputParams.Location = new System.Drawing.Point(19, 265);
            this.listViewFIMIInputParams.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.listViewFIMIInputParams.Name = "listViewFIMIInputParams";
            this.listViewFIMIInputParams.Size = new System.Drawing.Size(479, 458);
            this.listViewFIMIInputParams.TabIndex = 9;
            this.listViewFIMIInputParams.UseCompatibleStateImageBehavior = false;
            this.listViewFIMIInputParams.View = System.Windows.Forms.View.Details;
            this.listViewFIMIInputParams.SelectedIndexChanged += new System.EventHandler(this.listViewFIMIInputParams_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Param";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Column";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Constant";
            this.columnHeader3.Width = 110;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 32);
            this.label3.TabIndex = 10;
            this.label3.Text = "SQL query";
            // 
            // textBoxSQL
            // 
            this.textBoxSQL.Location = new System.Drawing.Point(149, 0);
            this.textBoxSQL.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.Size = new System.Drawing.Size(1098, 39);
            this.textBoxSQL.TabIndex = 11;
            this.textBoxSQL.Leave += new System.EventHandler(this.textBoxSQL_Leave);
            // 
            // comboBoxSqlColumn
            // 
            this.comboBoxSqlColumn.FormattingEnabled = true;
            this.comboBoxSqlColumn.Location = new System.Drawing.Point(19, 66);
            this.comboBoxSqlColumn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.comboBoxSqlColumn.Name = "comboBoxSqlColumn";
            this.comboBoxSqlColumn.Size = new System.Drawing.Size(242, 40);
            this.comboBoxSqlColumn.TabIndex = 12;
            // 
            // buttonAddSQLColumn
            // 
            this.buttonAddSQLColumn.Location = new System.Drawing.Point(264, 66);
            this.buttonAddSQLColumn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonAddSQLColumn.Name = "buttonAddSQLColumn";
            this.buttonAddSQLColumn.Size = new System.Drawing.Size(186, 47);
            this.buttonAddSQLColumn.TabIndex = 13;
            this.buttonAddSQLColumn.Text = "Add To API";
            this.buttonAddSQLColumn.UseVisualStyleBackColor = true;
            this.buttonAddSQLColumn.Click += new System.EventHandler(this.buttonAddSQLColumn_Click);
            // 
            // textBoxConstant
            // 
            this.textBoxConstant.Location = new System.Drawing.Point(477, 62);
            this.textBoxConstant.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.textBoxConstant.Name = "textBoxConstant";
            this.textBoxConstant.Size = new System.Drawing.Size(201, 39);
            this.textBoxConstant.TabIndex = 14;
            // 
            // buttonAddConst
            // 
            this.buttonAddConst.Location = new System.Drawing.Point(706, 62);
            this.buttonAddConst.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonAddConst.Name = "buttonAddConst";
            this.buttonAddConst.Size = new System.Drawing.Size(227, 47);
            this.buttonAddConst.TabIndex = 15;
            this.buttonAddConst.Text = "Add Const To API";
            this.buttonAddConst.UseVisualStyleBackColor = true;
            this.buttonAddConst.Click += new System.EventHandler(this.buttonAddConst_Click);
            // 
            // textBoxCommandPrefix
            // 
            this.textBoxCommandPrefix.Location = new System.Drawing.Point(32, 188);
            this.textBoxCommandPrefix.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.textBoxCommandPrefix.Name = "textBoxCommandPrefix";
            this.textBoxCommandPrefix.Size = new System.Drawing.Size(219, 39);
            this.textBoxCommandPrefix.TabIndex = 16;
            this.textBoxCommandPrefix.Leave += new System.EventHandler(this.textBoxCommandPrefix_Leave);
            // 
            // listViewFimiOutputParam
            // 
            this.listViewFimiOutputParam.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.listViewFimiOutputParam.FullRowSelect = true;
            this.listViewFimiOutputParam.Location = new System.Drawing.Point(509, 265);
            this.listViewFimiOutputParam.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.listViewFimiOutputParam.Name = "listViewFimiOutputParam";
            this.listViewFimiOutputParam.Size = new System.Drawing.Size(790, 458);
            this.listViewFimiOutputParam.TabIndex = 17;
            this.listViewFimiOutputParam.UseCompatibleStateImageBehavior = false;
            this.listViewFimiOutputParam.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Path";
            this.columnHeader4.Width = 300;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Value";
            this.columnHeader5.Width = 150;
            // 
            // buttonAddColumnToTable
            // 
            this.buttonAddColumnToTable.Location = new System.Drawing.Point(264, 117);
            this.buttonAddColumnToTable.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonAddColumnToTable.Name = "buttonAddColumnToTable";
            this.buttonAddColumnToTable.Size = new System.Drawing.Size(186, 47);
            this.buttonAddColumnToTable.TabIndex = 18;
            this.buttonAddColumnToTable.Text = "Add To Table";
            this.buttonAddColumnToTable.UseVisualStyleBackColor = true;
            this.buttonAddColumnToTable.Click += new System.EventHandler(this.buttonAddColumnToTable_Click);
            // 
            // buttonAddConstantToTable
            // 
            this.buttonAddConstantToTable.Location = new System.Drawing.Point(706, 109);
            this.buttonAddConstantToTable.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonAddConstantToTable.Name = "buttonAddConstantToTable";
            this.buttonAddConstantToTable.Size = new System.Drawing.Size(227, 47);
            this.buttonAddConstantToTable.TabIndex = 19;
            this.buttonAddConstantToTable.Text = "Add Const To Table";
            this.buttonAddConstantToTable.UseVisualStyleBackColor = true;
            this.buttonAddConstantToTable.Click += new System.EventHandler(this.buttonAddConstantToTable_Click);
            // 
            // listViewTableColumns
            // 
            this.listViewTableColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.listViewTableColumns.FullRowSelect = true;
            this.listViewTableColumns.Location = new System.Drawing.Point(1307, 265);
            this.listViewTableColumns.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.listViewTableColumns.Name = "listViewTableColumns";
            this.listViewTableColumns.Size = new System.Drawing.Size(663, 1066);
            this.listViewTableColumns.TabIndex = 20;
            this.listViewTableColumns.UseCompatibleStateImageBehavior = false;
            this.listViewTableColumns.View = System.Windows.Forms.View.Details;
            this.listViewTableColumns.SelectedIndexChanged += new System.EventHandler(this.listViewTableColumns_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "path";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Cnst";
            this.columnHeader7.Width = 50;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Var";
            this.columnHeader8.Width = 50;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Column";
            this.columnHeader9.Width = 65;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Table";
            this.columnHeader10.Width = 65;
            // 
            // buttonFullTest
            // 
            this.buttonFullTest.Location = new System.Drawing.Point(22, 806);
            this.buttonFullTest.Margin = new System.Windows.Forms.Padding(6);
            this.buttonFullTest.Name = "buttonFullTest";
            this.buttonFullTest.Size = new System.Drawing.Size(147, 49);
            this.buttonFullTest.TabIndex = 21;
            this.buttonFullTest.Text = "Test full";
            this.buttonFullTest.UseVisualStyleBackColor = true;
            this.buttonFullTest.Click += new System.EventHandler(this.buttonFullTest_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(813, 194);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 32);
            this.label4.TabIndex = 22;
            this.label4.Text = "Alt";
            // 
            // comboBoxAlternatives
            // 
            this.comboBoxAlternatives.FormattingEnabled = true;
            this.comboBoxAlternatives.Location = new System.Drawing.Point(865, 188);
            this.comboBoxAlternatives.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxAlternatives.Name = "comboBoxAlternatives";
            this.comboBoxAlternatives.Size = new System.Drawing.Size(429, 40);
            this.comboBoxAlternatives.TabIndex = 23;
            this.comboBoxAlternatives.SelectedIndexChanged += new System.EventHandler(this.comboBoxAlternatives_SelectedIndexChanged);
            // 
            // buttonMXGraph
            // 
            this.buttonMXGraph.Location = new System.Drawing.Point(22, 864);
            this.buttonMXGraph.Name = "buttonMXGraph";
            this.buttonMXGraph.Size = new System.Drawing.Size(150, 46);
            this.buttonMXGraph.TabIndex = 24;
            this.buttonMXGraph.Text = "ToMXGraph";
            this.buttonMXGraph.UseVisualStyleBackColor = true;
            this.buttonMXGraph.Click += new System.EventHandler(this.buttonMXGraph_Click);
            // 
            // FormConnectFimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1976, 1340);
            this.Controls.Add(this.buttonMXGraph);
            this.Controls.Add(this.comboBoxAlternatives);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonFullTest);
            this.Controls.Add(this.listViewTableColumns);
            this.Controls.Add(this.buttonAddConstantToTable);
            this.Controls.Add(this.buttonAddColumnToTable);
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
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxFimiCommand);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "FormConnectFimi";
            this.Text = "FormConnectApi";
            this.Load += new System.EventHandler(this.FormConnectFimi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox comboBoxFimiCommand;
        private Label label1;
        private Label label2;
        private ComboBox comboBoxTable;
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
        private Button buttonAddColumnToTable;
        private Button buttonAddConstantToTable;
        private ListView listViewTableColumns;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private Button buttonFullTest;
        private Label label4;
        private ComboBox comboBoxAlternatives;
        private Button buttonMXGraph;
    }
}