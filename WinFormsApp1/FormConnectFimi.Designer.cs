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
            this.listBoxFimiParam = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTable = new System.Windows.Forms.ComboBox();
            this.listBoxTableColumns = new System.Windows.Forms.ListBox();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.buttonLink = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxFimiCommand
            // 
            this.comboBoxFimiCommand.FormattingEnabled = true;
            this.comboBoxFimiCommand.Location = new System.Drawing.Point(12, 142);
            this.comboBoxFimiCommand.Name = "comboBoxFimiCommand";
            this.comboBoxFimiCommand.Size = new System.Drawing.Size(379, 40);
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
            // listBoxFimiParam
            // 
            this.listBoxFimiParam.FormattingEnabled = true;
            this.listBoxFimiParam.ItemHeight = 32;
            this.listBoxFimiParam.Location = new System.Drawing.Point(12, 203);
            this.listBoxFimiParam.Name = "listBoxFimiParam";
            this.listBoxFimiParam.Size = new System.Drawing.Size(379, 452);
            this.listBoxFimiParam.TabIndex = 2;
            this.listBoxFimiParam.SelectedIndexChanged += new System.EventHandler(this.listBoxFimiParam_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(452, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select table";
            // 
            // comboBoxTable
            // 
            this.comboBoxTable.FormattingEnabled = true;
            this.comboBoxTable.Location = new System.Drawing.Point(452, 142);
            this.comboBoxTable.Name = "comboBoxTable";
            this.comboBoxTable.Size = new System.Drawing.Size(645, 40);
            this.comboBoxTable.TabIndex = 4;
            this.comboBoxTable.SelectedIndexChanged += new System.EventHandler(this.comboBoxTable_SelectedIndexChanged);
            // 
            // listBoxTableColumns
            // 
            this.listBoxTableColumns.FormattingEnabled = true;
            this.listBoxTableColumns.ItemHeight = 32;
            this.listBoxTableColumns.Location = new System.Drawing.Point(454, 214);
            this.listBoxTableColumns.Name = "listBoxTableColumns";
            this.listBoxTableColumns.Size = new System.Drawing.Size(643, 836);
            this.listBoxTableColumns.TabIndex = 5;
            this.listBoxTableColumns.SelectedIndexChanged += new System.EventHandler(this.listBoxTableColumns_SelectedIndexChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Location = new System.Drawing.Point(464, 65);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(200, 39);
            this.textBoxPrefix.TabIndex = 6;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.textBoxPrefix_TextChanged);
            // 
            // buttonLink
            // 
            this.buttonLink.Location = new System.Drawing.Point(947, 65);
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
            // FormConnectFimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 1071);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonLink);
            this.Controls.Add(this.textBoxPrefix);
            this.Controls.Add(this.listBoxTableColumns);
            this.Controls.Add(this.comboBoxTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxFimiParam);
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
        private ListBox listBoxFimiParam;
        private Label label2;
        private ComboBox comboBoxTable;
        private ListBox listBoxTableColumns;
        private TextBox textBoxPrefix;
        private Button buttonLink;
        private Button buttonTest;
    }
}