
namespace TestJsonRazbor
{
    partial class FormPipeline
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Steps");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonSavePipeline = new System.Windows.Forms.Button();
            this.buttonYaml = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxResponceStep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonSenderMoc = new System.Windows.Forms.Button();
            this.buttonReceiverMoc = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStripFilters = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTestServer = new System.Windows.Forms.Button();
            this.buttonTestReceiver = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.buttonSetupSender = new System.Windows.Forms.Button();
            this.buttonSetupReceiver = new System.Windows.Forms.Button();
            this.textBoxStepDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxIDPrevStep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxIDStep = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPipelineDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxHandleSendError = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStripFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonOpen);
            this.splitContainer1.Panel2.Controls.Add(this.buttonNew);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSavePipeline);
            this.splitContainer1.Panel2.Controls.Add(this.buttonYaml);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxPipelineDescription);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1194, 729);
            this.splitContainer1.SplitterDistance = 396;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Steps";
            treeNode1.Text = "Steps";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.Size = new System.Drawing.Size(396, 729);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addStepToolStripMenuItem,
            this.removeStepToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(227, 80);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addStepToolStripMenuItem
            // 
            this.addStepToolStripMenuItem.Name = "addStepToolStripMenuItem";
            this.addStepToolStripMenuItem.Size = new System.Drawing.Size(226, 38);
            this.addStepToolStripMenuItem.Text = "Add step";
            this.addStepToolStripMenuItem.Click += new System.EventHandler(this.AddNode);
            // 
            // removeStepToolStripMenuItem
            // 
            this.removeStepToolStripMenuItem.Name = "removeStepToolStripMenuItem";
            this.removeStepToolStripMenuItem.Size = new System.Drawing.Size(226, 38);
            this.removeStepToolStripMenuItem.Text = "Remove step";
            this.removeStepToolStripMenuItem.Click += new System.EventHandler(this.removeStepToolStripMenuItem_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(342, 41);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(89, 47);
            this.buttonOpen.TabIndex = 33;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(230, 41);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(93, 47);
            this.buttonNew.TabIndex = 6;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonSavePipeline
            // 
            this.buttonSavePipeline.Location = new System.Drawing.Point(423, 41);
            this.buttonSavePipeline.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSavePipeline.Name = "buttonSavePipeline";
            this.buttonSavePipeline.Size = new System.Drawing.Size(80, 47);
            this.buttonSavePipeline.TabIndex = 33;
            this.buttonSavePipeline.Text = "Save";
            this.buttonSavePipeline.UseVisualStyleBackColor = true;
            this.buttonSavePipeline.Click += new System.EventHandler(this.buttonSavePipeline_Click);
            // 
            // buttonYaml
            // 
            this.buttonYaml.Location = new System.Drawing.Point(511, 41);
            this.buttonYaml.Margin = new System.Windows.Forms.Padding(4);
            this.buttonYaml.Name = "buttonYaml";
            this.buttonYaml.Size = new System.Drawing.Size(74, 47);
            this.buttonYaml.TabIndex = 4;
            this.buttonYaml.Text = "Yaml";
            this.buttonYaml.UseVisualStyleBackColor = true;
            this.buttonYaml.Click += new System.EventHandler(this.buttonYaml_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(602, 41);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 47);
            this.button1.TabIndex = 5;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxHandleSendError);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.textBoxResponceStep);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonSenderMoc);
            this.groupBox1.Controls.Add(this.buttonReceiverMoc);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.buttonTestServer);
            this.groupBox1.Controls.Add(this.buttonTestReceiver);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.buttonSetupSender);
            this.groupBox1.Controls.Add(this.buttonSetupReceiver);
            this.groupBox1.Controls.Add(this.textBoxStepDescription);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxIDPrevStep);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxIDStep);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(11, 68);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(778, 654);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step detail";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(648, 32);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(115, 36);
            this.checkBox1.TabIndex = 37;
            this.checkBox1.Text = "bridge";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBoxResponceStep
            // 
            this.textBoxResponceStep.Location = new System.Drawing.Point(511, 26);
            this.textBoxResponceStep.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxResponceStep.Name = "textBoxResponceStep";
            this.textBoxResponceStep.Size = new System.Drawing.Size(127, 39);
            this.textBoxResponceStep.TabIndex = 36;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(409, 30);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 32);
            this.label6.TabIndex = 35;
            this.label6.Text = "RespStep";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(594, 606);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(141, 43);
            this.button3.TabIndex = 34;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(592, 154);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 47);
            this.button2.TabIndex = 33;
            this.button2.Text = "Moc Text";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUp.Location = new System.Drawing.Point(733, 247);
            this.buttonUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(41, 47);
            this.buttonUp.TabIndex = 31;
            this.buttonUp.Text = "^";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDown.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonDown.Location = new System.Drawing.Point(733, 299);
            this.buttonDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(43, 47);
            this.buttonDown.TabIndex = 32;
            this.buttonDown.Text = "V";
            this.buttonDown.UseVisualStyleBackColor = true;
            // 
            // buttonSenderMoc
            // 
            this.buttonSenderMoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSenderMoc.Location = new System.Drawing.Point(605, 539);
            this.buttonSenderMoc.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSenderMoc.Name = "buttonSenderMoc";
            this.buttonSenderMoc.Size = new System.Drawing.Size(97, 47);
            this.buttonSenderMoc.TabIndex = 15;
            this.buttonSenderMoc.Text = "Moc";
            this.buttonSenderMoc.UseVisualStyleBackColor = true;
            this.buttonSenderMoc.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // buttonReceiverMoc
            // 
            this.buttonReceiverMoc.Location = new System.Drawing.Point(472, 154);
            this.buttonReceiverMoc.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReceiverMoc.Name = "buttonReceiverMoc";
            this.buttonReceiverMoc.Size = new System.Drawing.Size(115, 47);
            this.buttonReceiverMoc.TabIndex = 14;
            this.buttonReceiverMoc.Text = "Moc File";
            this.buttonReceiverMoc.UseVisualStyleBackColor = true;
            this.buttonReceiverMoc.Click += new System.EventHandler(this.buttonReceiverMoc_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 207);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 32);
            this.label5.TabIndex = 13;
            this.label5.Text = "Filters";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.ContextMenuStrip = this.contextMenuStripFilters;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 32;
            this.listBox1.Location = new System.Drawing.Point(15, 247);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(713, 260);
            this.listBox1.TabIndex = 12;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // contextMenuStripFilters
            // 
            this.contextMenuStripFilters.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStripFilters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilterToolStripMenuItem,
            this.removeFilterToolStripMenuItem});
            this.contextMenuStripFilters.Name = "contextMenuStripFilters";
            this.contextMenuStripFilters.Size = new System.Drawing.Size(231, 80);
            // 
            // addFilterToolStripMenuItem
            // 
            this.addFilterToolStripMenuItem.Name = "addFilterToolStripMenuItem";
            this.addFilterToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            this.addFilterToolStripMenuItem.Text = "Add filter";
            this.addFilterToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // removeFilterToolStripMenuItem
            // 
            this.removeFilterToolStripMenuItem.Name = "removeFilterToolStripMenuItem";
            this.removeFilterToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            this.removeFilterToolStripMenuItem.Text = "Remove filter";
            this.removeFilterToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // buttonTestServer
            // 
            this.buttonTestServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonTestServer.Enabled = false;
            this.buttonTestServer.Location = new System.Drawing.Point(448, 539);
            this.buttonTestServer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTestServer.Name = "buttonTestServer";
            this.buttonTestServer.Size = new System.Drawing.Size(149, 47);
            this.buttonTestServer.TabIndex = 11;
            this.buttonTestServer.Text = "Test";
            this.buttonTestServer.UseVisualStyleBackColor = true;
            this.buttonTestServer.Click += new System.EventHandler(this.buttonTestServer_Click);
            // 
            // buttonTestReceiver
            // 
            this.buttonTestReceiver.Enabled = false;
            this.buttonTestReceiver.Location = new System.Drawing.Point(383, 154);
            this.buttonTestReceiver.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTestReceiver.Name = "buttonTestReceiver";
            this.buttonTestReceiver.Size = new System.Drawing.Size(82, 47);
            this.buttonTestReceiver.TabIndex = 10;
            this.buttonTestReceiver.Text = "Test";
            this.buttonTestReceiver.UseVisualStyleBackColor = true;
            this.buttonTestReceiver.Click += new System.EventHandler(this.buttonTestReceiver_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(15, 601);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(427, 47);
            this.button4.TabIndex = 9;
            this.button4.Text = "Save step";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // buttonSetupSender
            // 
            this.buttonSetupSender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSetupSender.Location = new System.Drawing.Point(15, 539);
            this.buttonSetupSender.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSetupSender.Name = "buttonSetupSender";
            this.buttonSetupSender.Size = new System.Drawing.Size(427, 47);
            this.buttonSetupSender.TabIndex = 8;
            this.buttonSetupSender.Text = "Sender";
            this.buttonSetupSender.UseVisualStyleBackColor = true;
            this.buttonSetupSender.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonSetupReceiver
            // 
            this.buttonSetupReceiver.Location = new System.Drawing.Point(15, 154);
            this.buttonSetupReceiver.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSetupReceiver.Name = "buttonSetupReceiver";
            this.buttonSetupReceiver.Size = new System.Drawing.Size(362, 47);
            this.buttonSetupReceiver.TabIndex = 6;
            this.buttonSetupReceiver.Text = "Receiver";
            this.buttonSetupReceiver.UseVisualStyleBackColor = true;
            this.buttonSetupReceiver.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxStepDescription
            // 
            this.textBoxStepDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStepDescription.Location = new System.Drawing.Point(178, 106);
            this.textBoxStepDescription.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxStepDescription.Name = "textBoxStepDescription";
            this.textBoxStepDescription.Size = new System.Drawing.Size(585, 39);
            this.textBoxStepDescription.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 32);
            this.label4.TabIndex = 4;
            this.label4.Text = "description";
            // 
            // textBoxIDPrevStep
            // 
            this.textBoxIDPrevStep.Location = new System.Drawing.Point(292, 30);
            this.textBoxIDPrevStep.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxIDPrevStep.Name = "textBoxIDPrevStep";
            this.textBoxIDPrevStep.Size = new System.Drawing.Size(114, 39);
            this.textBoxIDPrevStep.TabIndex = 3;
            this.textBoxIDPrevStep.TextChanged += new System.EventHandler(this.textBoxIDPrevStep_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "IDAncestor";
            // 
            // textBoxIDStep
            // 
            this.textBoxIDStep.Location = new System.Drawing.Point(45, 32);
            this.textBoxIDStep.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxIDStep.Name = "textBoxIDStep";
            this.textBoxIDStep.Size = new System.Drawing.Size(123, 39);
            this.textBoxIDStep.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 32);
            this.label2.TabIndex = 0;
            this.label2.Text = "ID";
            // 
            // textBoxPipelineDescription
            // 
            this.textBoxPipelineDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPipelineDescription.Location = new System.Drawing.Point(241, 0);
            this.textBoxPipelineDescription.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPipelineDescription.Name = "textBoxPipelineDescription";
            this.textBoxPipelineDescription.Size = new System.Drawing.Size(533, 39);
            this.textBoxPipelineDescription.TabIndex = 1;
            this.textBoxPipelineDescription.TextChanged += new System.EventHandler(this.textBoxPipelineDescription_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pipeline description";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "yml";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBoxHandleSendError
            // 
            this.checkBoxHandleSendError.AutoSize = true;
            this.checkBoxHandleSendError.Location = new System.Drawing.Point(511, 65);
            this.checkBoxHandleSendError.Name = "checkBoxHandleSendError";
            this.checkBoxHandleSendError.Size = new System.Drawing.Size(240, 36);
            this.checkBoxHandleSendError.TabIndex = 38;
            this.checkBoxHandleSendError.Text = "Handle Send Error";
            this.checkBoxHandleSendError.UseVisualStyleBackColor = true;
            // 
            // FormPipeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 729);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormPipeline";
            this.Text = "FormPipeline";
            this.Load += new System.EventHandler(this.FormPipeline_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStripFilters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxIDPrevStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxIDStep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPipelineDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxStepDescription;
        private System.Windows.Forms.Button buttonSetupSender;
        private System.Windows.Forms.Button buttonSetupReceiver;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buttonTestReceiver;
        private System.Windows.Forms.Button buttonTestServer;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilters;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.Button buttonYaml;
        private System.Windows.Forms.Button buttonSavePipeline;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFilterToolStripMenuItem;
        private System.Windows.Forms.Button buttonReceiverMoc;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonSenderMoc;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxResponceStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBoxHandleSendError;
    }
}