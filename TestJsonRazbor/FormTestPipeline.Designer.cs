
namespace TestJsonRazbor
{
    partial class FormTestPipeline
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
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.listView1 = new System.Windows.Forms.ListView();
            this.checkBoxMockReceiver = new System.Windows.Forms.CheckBox();
            this.checkBoxSender = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBoxSetMoc = new System.Windows.Forms.CheckBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Step";
            this.columnHeader1.Width = 220;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Receiver";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Sender";
            this.columnHeader3.Width = 120;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(3, 11);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(826, 187);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // checkBoxMockReceiver
            // 
            this.checkBoxMockReceiver.AutoSize = true;
            this.checkBoxMockReceiver.Location = new System.Drawing.Point(3, 207);
            this.checkBoxMockReceiver.Name = "checkBoxMockReceiver";
            this.checkBoxMockReceiver.Size = new System.Drawing.Size(158, 34);
            this.checkBoxMockReceiver.TabIndex = 1;
            this.checkBoxMockReceiver.Text = "Moc receiver";
            this.checkBoxMockReceiver.UseVisualStyleBackColor = true;
            this.checkBoxMockReceiver.CheckedChanged += new System.EventHandler(this.checkBoxMockReceiver_CheckedChanged);
            // 
            // checkBoxSender
            // 
            this.checkBoxSender.AutoSize = true;
            this.checkBoxSender.Location = new System.Drawing.Point(3, 247);
            this.checkBoxSender.Name = "checkBoxSender";
            this.checkBoxSender.Size = new System.Drawing.Size(148, 34);
            this.checkBoxSender.TabIndex = 2;
            this.checkBoxSender.Text = "Moc sender";
            this.checkBoxSender.UseVisualStyleBackColor = true;
            this.checkBoxSender.CheckedChanged += new System.EventHandler(this.checkBoxSender_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 300);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 43);
            this.button1.TabIndex = 3;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(178, 204);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 43);
            this.button2.TabIndex = 4;
            this.button2.Text = "Moc file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(178, 247);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(138, 43);
            this.button3.TabIndex = 5;
            this.button3.Text = "Moc file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBoxSetMoc
            // 
            this.checkBoxSetMoc.AutoSize = true;
            this.checkBoxSetMoc.Location = new System.Drawing.Point(179, 300);
            this.checkBoxSetMoc.Name = "checkBoxSetMoc";
            this.checkBoxSetMoc.Size = new System.Drawing.Size(206, 34);
            this.checkBoxSetMoc.TabIndex = 6;
            this.checkBoxSetMoc.Text = "Set results as moc";
            this.checkBoxSetMoc.UseVisualStyleBackColor = true;
            // 
            // logTextBox
            // 
            this.logTextBox.AcceptsReturn = true;
            this.logTextBox.AcceptsTab = true;
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(3, 349);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(837, 247);
            this.logTextBox.TabIndex = 7;
            // 
            // FormTestPipeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 599);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.checkBoxSetMoc);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxSender);
            this.Controls.Add(this.checkBoxMockReceiver);
            this.Controls.Add(this.listView1);
            this.Name = "FormTestPipeline";
            this.Text = "FormTestPipeline";
            this.Load += new System.EventHandler(this.FormTestPipeline_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.CheckBox checkBoxMockReceiver;
        private System.Windows.Forms.CheckBox checkBoxSender;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBoxSetMoc;
        private System.Windows.Forms.TextBox logTextBox;
    }
}