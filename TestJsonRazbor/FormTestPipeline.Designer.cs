
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
            this.components = new System.ComponentModel.Container();
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
            this.checkBoxDebug = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelCount = new System.Windows.Forms.Label();
            this.labelOpened = new System.Windows.Forms.Label();
            this.labelRexRequest = new System.Windows.Forms.Label();
            this.labelPerf = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBoxSaveScen = new System.Windows.Forms.CheckBox();
            this.buttonScenario = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.listView1.Location = new System.Drawing.Point(2, 32);
            this.listView1.Margin = new System.Windows.Forms.Padding(2);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(493, 96);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // checkBoxMockReceiver
            // 
            this.checkBoxMockReceiver.AutoSize = true;
            this.checkBoxMockReceiver.Location = new System.Drawing.Point(2, 130);
            this.checkBoxMockReceiver.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxMockReceiver.Name = "checkBoxMockReceiver";
            this.checkBoxMockReceiver.Size = new System.Drawing.Size(94, 19);
            this.checkBoxMockReceiver.TabIndex = 1;
            this.checkBoxMockReceiver.Text = "Moc receiver";
            this.checkBoxMockReceiver.UseVisualStyleBackColor = true;
            this.checkBoxMockReceiver.CheckedChanged += new System.EventHandler(this.checkBoxMockReceiver_CheckedChanged);
            // 
            // checkBoxSender
            // 
            this.checkBoxSender.AutoSize = true;
            this.checkBoxSender.Location = new System.Drawing.Point(2, 150);
            this.checkBoxSender.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSender.Name = "checkBoxSender";
            this.checkBoxSender.Size = new System.Drawing.Size(88, 19);
            this.checkBoxSender.TabIndex = 2;
            this.checkBoxSender.Text = "Moc sender";
            this.checkBoxSender.UseVisualStyleBackColor = true;
            this.checkBoxSender.CheckedChanged += new System.EventHandler(this.checkBoxSender_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 176);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 22);
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
            this.button2.Location = new System.Drawing.Point(104, 128);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 22);
            this.button2.TabIndex = 4;
            this.button2.Text = "Moc file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(104, 150);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 22);
            this.button3.TabIndex = 5;
            this.button3.Text = "Moc file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBoxSetMoc
            // 
            this.checkBoxSetMoc.AutoSize = true;
            this.checkBoxSetMoc.Location = new System.Drawing.Point(104, 176);
            this.checkBoxSetMoc.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSetMoc.Name = "checkBoxSetMoc";
            this.checkBoxSetMoc.Size = new System.Drawing.Size(120, 19);
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
            this.logTextBox.Location = new System.Drawing.Point(104, 32);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(327, 57);
            this.logTextBox.TabIndex = 7;
            this.logTextBox.Visible = false;
            // 
            // checkBoxDebug
            // 
            this.checkBoxDebug.AutoSize = true;
            this.checkBoxDebug.Checked = true;
            this.checkBoxDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDebug.Location = new System.Drawing.Point(190, 131);
            this.checkBoxDebug.Name = "checkBoxDebug";
            this.checkBoxDebug.Size = new System.Drawing.Size(95, 19);
            this.checkBoxDebug.TabIndex = 8;
            this.checkBoxDebug.Text = "Debug Mode";
            this.checkBoxDebug.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(382, 150);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(70, 15);
            this.labelCount.TabIndex = 9;
            this.labelCount.Text = "                     ";
            // 
            // labelOpened
            // 
            this.labelOpened.AutoSize = true;
            this.labelOpened.Location = new System.Drawing.Point(382, 128);
            this.labelOpened.Name = "labelOpened";
            this.labelOpened.Size = new System.Drawing.Size(70, 15);
            this.labelOpened.TabIndex = 10;
            this.labelOpened.Text = "                     ";
            // 
            // labelRexRequest
            // 
            this.labelRexRequest.AutoSize = true;
            this.labelRexRequest.Location = new System.Drawing.Point(382, 105);
            this.labelRexRequest.Name = "labelRexRequest";
            this.labelRexRequest.Size = new System.Drawing.Size(70, 15);
            this.labelRexRequest.TabIndex = 11;
            this.labelRexRequest.Text = "                     ";
            // 
            // labelPerf
            // 
            this.labelPerf.AutoSize = true;
            this.labelPerf.Location = new System.Drawing.Point(216, 129);
            this.labelPerf.Name = "labelPerf";
            this.labelPerf.Size = new System.Drawing.Size(70, 15);
            this.labelPerf.TabIndex = 12;
            this.labelPerf.Text = "                     ";
            this.labelPerf.Click += new System.EventHandler(this.labelPerf_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(2, 203);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(493, 124);
            this.listBox1.TabIndex = 13;
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(132, 336);
            this.listBox2.TabIndex = 14;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonScenario);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxSaveScen);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxMockReceiver);
            this.splitContainer1.Panel1.Controls.Add(this.labelPerf);
            this.splitContainer1.Panel1.Controls.Add(this.logTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxSender);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxDebug);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxSetMoc);
            this.splitContainer1.Panel1.Controls.Add(this.labelRexRequest);
            this.splitContainer1.Panel1.Controls.Add(this.labelOpened);
            this.splitContainer1.Panel1.Controls.Add(this.labelCount);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox2);
            this.splitContainer1.Size = new System.Drawing.Size(634, 336);
            this.splitContainer1.SplitterDistance = 498;
            this.splitContainer1.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Scenario";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(61, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(434, 23);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // checkBoxSaveScen
            // 
            this.checkBoxSaveScen.AutoSize = true;
            this.checkBoxSaveScen.Location = new System.Drawing.Point(189, 152);
            this.checkBoxSaveScen.Name = "checkBoxSaveScen";
            this.checkBoxSaveScen.Size = new System.Drawing.Size(110, 19);
            this.checkBoxSaveScen.TabIndex = 16;
            this.checkBoxSaveScen.Text = "save to scenario";
            this.checkBoxSaveScen.UseVisualStyleBackColor = true;
            // 
            // buttonScenario
            // 
            this.buttonScenario.Location = new System.Drawing.Point(224, 174);
            this.buttonScenario.Name = "buttonScenario";
            this.buttonScenario.Size = new System.Drawing.Size(75, 23);
            this.buttonScenario.TabIndex = 17;
            this.buttonScenario.Text = "Scenarious";
            this.buttonScenario.UseVisualStyleBackColor = true;
            this.buttonScenario.Click += new System.EventHandler(this.buttonScenario_Click);
            // 
            // FormTestPipeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 336);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormTestPipeline";
            this.Text = "FormTestPipeline";
            this.Load += new System.EventHandler(this.FormTestPipeline_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.CheckBox checkBoxDebug;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelOpened;
        private System.Windows.Forms.Label labelRexRequest;
        private System.Windows.Forms.Label labelPerf;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button buttonScenario;
        private System.Windows.Forms.CheckBox checkBoxSaveScen;
    }
}