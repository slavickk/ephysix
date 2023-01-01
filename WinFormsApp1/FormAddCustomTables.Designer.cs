using Microsoft.VisualBasic;
using static NpgsqlTypes.NpgsqlTsQuery;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System;

namespace WinFormsApp1
{
    partial class FormAddCustomTables
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
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExecUrl = new System.Windows.Forms.Button();
            this.textBoxUrlResult = new System.Windows.Forms.TextBox();
            this.textBoxSql = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExecSql = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTableName = new System.Windows.Forms.TextBox();
            this.textBoxTableCaption = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFieldName = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonSaveField = new System.Windows.Forms.Button();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.Location = new System.Drawing.Point(36, 2);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(533, 23);
            this.textBoxUrl.TabIndex = 0;
            this.textBoxUrl.Text = "https://pkgstore.datahub.io/core/currency-codes/codes-all_json/data/029be9faf6547aba93d64384f7444774/codes-all_json.json";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "url";
            // 
            // buttonExecUrl
            // 
            this.buttonExecUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecUrl.Location = new System.Drawing.Point(575, 2);
            this.buttonExecUrl.Name = "buttonExecUrl";
            this.buttonExecUrl.Size = new System.Drawing.Size(52, 23);
            this.buttonExecUrl.TabIndex = 2;
            this.buttonExecUrl.Text = "Exec";
            this.buttonExecUrl.UseVisualStyleBackColor = true;
            this.buttonExecUrl.Click += new System.EventHandler(this.buttonExecUrl_Click);
            // 
            // textBoxUrlResult
            // 
            this.textBoxUrlResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrlResult.Location = new System.Drawing.Point(34, 32);
            this.textBoxUrlResult.Multiline = true;
            this.textBoxUrlResult.Name = "textBoxUrlResult";
            this.textBoxUrlResult.Size = new System.Drawing.Size(535, 123);
            this.textBoxUrlResult.TabIndex = 3;
           
            // 
            // textBoxSql
            // 
            this.textBoxSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSql.Location = new System.Drawing.Point(34, 179);
            this.textBoxSql.Multiline = true;
            this.textBoxSql.Name = "textBoxSql";
            this.textBoxSql.Size = new System.Drawing.Size(535, 123);
            this.textBoxSql.TabIndex = 4;
            this.textBoxSql.Text = "select *\r\nfrom json_to_recordset(@body::json)\r\nas x(\"AlphabeticCode\" text,\"Currency\" text, \"NumericCode\" float,\"MinorUnit\" text);\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sql statement";
            // 
            // buttonExecSql
            // 
            this.buttonExecSql.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecSql.Location = new System.Drawing.Point(575, 179);
            this.buttonExecSql.Name = "buttonExecSql";
            this.buttonExecSql.Size = new System.Drawing.Size(52, 23);
            this.buttonExecSql.TabIndex = 6;
            this.buttonExecSql.Text = "Exec";
            this.buttonExecSql.UseVisualStyleBackColor = true;
            this.buttonExecSql.Click += new System.EventHandler(this.buttonExecSql_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(36, 306);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(448, 171);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(492, 511);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(95, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save Table";
            this.buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click  ;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 486);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Table Name";
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Location = new System.Drawing.Point(84, 482);
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.Size = new System.Drawing.Size(184, 23);
            this.textBoxTableName.TabIndex = 10;
            // 
            // textBoxTableCaption
            // 
            this.textBoxTableCaption.Location = new System.Drawing.Point(84, 511);
            this.textBoxTableCaption.Name = "textBoxTableCaption";
            this.textBoxTableCaption.Size = new System.Drawing.Size(400, 23);
            this.textBoxTableCaption.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 515);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Caption";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(490, 306);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Name";
            // 
            // textBoxFieldName
            // 
            this.textBoxFieldName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFieldName.Location = new System.Drawing.Point(490, 321);
            this.textBoxFieldName.Name = "textBoxFieldName";
            this.textBoxFieldName.Size = new System.Drawing.Size(137, 23);
            this.textBoxFieldName.TabIndex = 15;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(490, 363);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(137, 23);
            this.textBox1.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(490, 347);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Caption";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(490, 407);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(137, 23);
            this.textBox2.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(490, 391);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Type";
            // 
            // buttonSaveField
            // 
            this.buttonSaveField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveField.Location = new System.Drawing.Point(490, 436);
            this.buttonSaveField.Name = "buttonSaveField";
            this.buttonSaveField.Size = new System.Drawing.Size(97, 23);
            this.buttonSaveField.TabIndex = 20;
            this.buttonSaveField.Text = "Save Field";
            this.buttonSaveField.UseVisualStyleBackColor = true;
            this.buttonSaveField.Click += new System.EventHandler(this.buttonSaveField_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Caption";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 90;
            // 
            // FormAddCustomTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 546);
            this.Controls.Add(this.buttonSaveField);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxFieldName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxTableCaption);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxTableName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonExecSql);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSql);
            this.Controls.Add(this.textBoxUrlResult);
            this.Controls.Add(this.buttonExecUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUrl);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "FormAddCustomTables";
            this.Text = "Table from external url";
            this.Load += new System.EventHandler(this.FormAddCustomTables_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private TextBox textBoxUrl;
        private Label label1;
        private Button buttonExecUrl;
        private TextBox textBoxUrlResult;
        private TextBox textBoxSql;
        private Label label2;
        private Button buttonExecSql;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Button buttonSave;
        private Label label3;
        private TextBox textBoxTableName;
        private TextBox textBoxTableCaption;
        private Label label5;
        private Label label4;
        private TextBox textBoxFieldName;
        private TextBox textBox1;
        private Label label6;
        private TextBox textBox2;
        private Label label7;
        private Button buttonSaveField;
    }
}