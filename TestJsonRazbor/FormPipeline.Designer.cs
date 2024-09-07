/******************************************************************
 * File: FormPipeline.Designer.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/


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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Steps");
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeView1 = new System.Windows.Forms.TreeView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            addStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            button6 = new System.Windows.Forms.Button();
            buttonOpen = new System.Windows.Forms.Button();
            buttonPlantUml = new System.Windows.Forms.Button();
            buttonNew = new System.Windows.Forms.Button();
            buttonSavePipeline = new System.Windows.Forms.Button();
            buttonYaml = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            button8 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            buttonPaste = new System.Windows.Forms.Button();
            buttonCopy = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            textBoxRestorePath = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            checkBoxHandleSendError = new System.Windows.Forms.CheckBox();
            checkBox1 = new System.Windows.Forms.CheckBox();
            textBoxResponceStep = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            buttonUp = new System.Windows.Forms.Button();
            buttonDown = new System.Windows.Forms.Button();
            buttonSenderMoc = new System.Windows.Forms.Button();
            buttonReceiverMoc = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            listBox1 = new System.Windows.Forms.ListBox();
            contextMenuStripFilters = new System.Windows.Forms.ContextMenuStrip(components);
            addFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            buttonTestServer = new System.Windows.Forms.Button();
            buttonTestReceiver = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            buttonSetupSender = new System.Windows.Forms.Button();
            buttonSetupReceiver = new System.Windows.Forms.Button();
            textBoxStepDescription = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            textBoxIDPrevStep = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            textBoxIDStep = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            textBoxPipelineDescription = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(components);
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            textBoxIDFamilyPrevious = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            textBoxFamilyStep = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            contextMenuStripFilters.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button6);
            splitContainer1.Panel2.Controls.Add(buttonOpen);
            splitContainer1.Panel2.Controls.Add(buttonPlantUml);
            splitContainer1.Panel2.Controls.Add(buttonNew);
            splitContainer1.Panel2.Controls.Add(buttonSavePipeline);
            splitContainer1.Panel2.Controls.Add(buttonYaml);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Panel2.Controls.Add(groupBox1);
            splitContainer1.Panel2.Controls.Add(textBoxPipelineDescription);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new System.Drawing.Size(1573, 929);
            splitContainer1.SplitterDistance = 520;
            splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView1.Location = new System.Drawing.Point(0, 0);
            treeView1.Margin = new System.Windows.Forms.Padding(4);
            treeView1.Name = "treeView1";
            treeNode2.Name = "Steps";
            treeNode2.Text = "Steps";
            treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode2 });
            treeView1.Size = new System.Drawing.Size(520, 929);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            treeView1.Click += treeView1_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addStepToolStripMenuItem, removeStepToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(227, 80);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // addStepToolStripMenuItem
            // 
            addStepToolStripMenuItem.Name = "addStepToolStripMenuItem";
            addStepToolStripMenuItem.Size = new System.Drawing.Size(226, 38);
            addStepToolStripMenuItem.Text = "Add step";
            addStepToolStripMenuItem.Click += AddNode;
            // 
            // removeStepToolStripMenuItem
            // 
            removeStepToolStripMenuItem.Name = "removeStepToolStripMenuItem";
            removeStepToolStripMenuItem.Size = new System.Drawing.Size(226, 38);
            removeStepToolStripMenuItem.Text = "Remove step";
            removeStepToolStripMenuItem.Click += removeStepToolStripMenuItem_Click;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(22, 32);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(184, 46);
            button6.TabIndex = 34;
            button6.Text = "button6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new System.Drawing.Point(342, 41);
            buttonOpen.Margin = new System.Windows.Forms.Padding(4);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new System.Drawing.Size(89, 47);
            buttonOpen.TabIndex = 33;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // buttonPlantUml
            // 
            buttonPlantUml.Location = new System.Drawing.Point(702, 41);
            buttonPlantUml.Margin = new System.Windows.Forms.Padding(4);
            buttonPlantUml.Name = "buttonPlantUml";
            buttonPlantUml.Size = new System.Drawing.Size(89, 47);
            buttonPlantUml.TabIndex = 33;
            buttonPlantUml.Text = "ToMd";
            buttonPlantUml.UseVisualStyleBackColor = true;
            buttonPlantUml.Click += buttonPlantUml_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new System.Drawing.Point(230, 41);
            buttonNew.Margin = new System.Windows.Forms.Padding(4);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new System.Drawing.Size(93, 47);
            buttonNew.TabIndex = 6;
            buttonNew.Text = "New";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonSavePipeline
            // 
            buttonSavePipeline.Location = new System.Drawing.Point(423, 41);
            buttonSavePipeline.Margin = new System.Windows.Forms.Padding(4);
            buttonSavePipeline.Name = "buttonSavePipeline";
            buttonSavePipeline.Size = new System.Drawing.Size(80, 47);
            buttonSavePipeline.TabIndex = 33;
            buttonSavePipeline.Text = "Save";
            buttonSavePipeline.UseVisualStyleBackColor = true;
            buttonSavePipeline.Click += buttonSavePipeline_Click;
            // 
            // buttonYaml
            // 
            buttonYaml.Location = new System.Drawing.Point(511, 41);
            buttonYaml.Margin = new System.Windows.Forms.Padding(4);
            buttonYaml.Name = "buttonYaml";
            buttonYaml.Size = new System.Drawing.Size(74, 47);
            buttonYaml.TabIndex = 4;
            buttonYaml.Text = "Yaml";
            buttonYaml.UseVisualStyleBackColor = true;
            buttonYaml.Click += buttonYaml_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(602, 41);
            button1.Margin = new System.Windows.Forms.Padding(4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(67, 47);
            button1.TabIndex = 5;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_2;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(textBoxIDFamilyPrevious);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(textBoxFamilyStep);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(button8);
            groupBox1.Controls.Add(button7);
            groupBox1.Controls.Add(buttonPaste);
            groupBox1.Controls.Add(buttonCopy);
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(textBoxRestorePath);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(checkBoxHandleSendError);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(textBoxResponceStep);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(buttonUp);
            groupBox1.Controls.Add(buttonDown);
            groupBox1.Controls.Add(buttonSenderMoc);
            groupBox1.Controls.Add(buttonReceiverMoc);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(listBox1);
            groupBox1.Controls.Add(buttonTestServer);
            groupBox1.Controls.Add(buttonTestReceiver);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(buttonSetupSender);
            groupBox1.Controls.Add(buttonSetupReceiver);
            groupBox1.Controls.Add(textBoxStepDescription);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(textBoxIDPrevStep);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textBoxIDStep);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new System.Drawing.Point(11, 68);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(1033, 854);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Step detail";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(744, 227);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(82, 46);
            button8.TabIndex = 45;
            button8.Text = "button8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button7.Location = new System.Drawing.Point(949, 227);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(89, 46);
            button7.TabIndex = 44;
            button7.Text = "Swag";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // buttonPaste
            // 
            buttonPaste.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonPaste.Location = new System.Drawing.Point(989, 533);
            buttonPaste.Name = "buttonPaste";
            buttonPaste.Size = new System.Drawing.Size(45, 46);
            buttonPaste.TabIndex = 43;
            buttonPaste.Text = "p";
            buttonPaste.UseVisualStyleBackColor = true;
            buttonPaste.Click += buttonPaste_Click;
            // 
            // buttonCopy
            // 
            buttonCopy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonCopy.Location = new System.Drawing.Point(989, 484);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new System.Drawing.Size(45, 46);
            buttonCopy.TabIndex = 42;
            buttonCopy.Text = "c";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // button5
            // 
            button5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button5.Location = new System.Drawing.Point(449, 802);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(253, 46);
            button5.TabIndex = 41;
            button5.Text = "Pipeline metrics";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBoxRestorePath
            // 
            textBoxRestorePath.Location = new System.Drawing.Point(435, 140);
            textBoxRestorePath.Name = "textBoxRestorePath";
            textBoxRestorePath.Size = new System.Drawing.Size(200, 39);
            textBoxRestorePath.TabIndex = 40;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(288, 144);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(139, 32);
            label7.TabIndex = 39;
            label7.Text = "RestorePath";
            // 
            // checkBoxHandleSendError
            // 
            checkBoxHandleSendError.AutoSize = true;
            checkBoxHandleSendError.Location = new System.Drawing.Point(17, 143);
            checkBoxHandleSendError.Name = "checkBoxHandleSendError";
            checkBoxHandleSendError.Size = new System.Drawing.Size(240, 36);
            checkBoxHandleSendError.TabIndex = 38;
            checkBoxHandleSendError.Text = "Handle Send Error";
            checkBoxHandleSendError.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(648, 32);
            checkBox1.Margin = new System.Windows.Forms.Padding(6);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(115, 36);
            checkBox1.TabIndex = 37;
            checkBox1.Text = "bridge";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            // 
            // textBoxResponceStep
            // 
            textBoxResponceStep.Location = new System.Drawing.Point(511, 26);
            textBoxResponceStep.Margin = new System.Windows.Forms.Padding(4);
            textBoxResponceStep.Name = "textBoxResponceStep";
            textBoxResponceStep.Size = new System.Drawing.Size(127, 39);
            textBoxResponceStep.TabIndex = 36;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(409, 30);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(111, 32);
            label6.TabIndex = 35;
            label6.Text = "RespStep";
            // 
            // button3
            // 
            button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button3.Location = new System.Drawing.Point(709, 802);
            button3.Margin = new System.Windows.Forms.Padding(4);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(74, 43);
            button3.TabIndex = 34;
            button3.Text = "dmn";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(592, 226);
            button2.Margin = new System.Windows.Forms.Padding(4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(124, 47);
            button2.TabIndex = 33;
            button2.Text = "Moc Text";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // buttonUp
            // 
            buttonUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonUp.Location = new System.Drawing.Point(988, 319);
            buttonUp.Margin = new System.Windows.Forms.Padding(4);
            buttonUp.Name = "buttonUp";
            buttonUp.Size = new System.Drawing.Size(41, 47);
            buttonUp.TabIndex = 31;
            buttonUp.Text = "^";
            buttonUp.UseVisualStyleBackColor = true;
            buttonUp.Click += buttonUp_Click;
            // 
            // buttonDown
            // 
            buttonDown.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonDown.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            buttonDown.Location = new System.Drawing.Point(988, 371);
            buttonDown.Margin = new System.Windows.Forms.Padding(4);
            buttonDown.Name = "buttonDown";
            buttonDown.Size = new System.Drawing.Size(43, 47);
            buttonDown.TabIndex = 32;
            buttonDown.Text = "V";
            buttonDown.UseVisualStyleBackColor = true;
            // 
            // buttonSenderMoc
            // 
            buttonSenderMoc.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonSenderMoc.Location = new System.Drawing.Point(605, 739);
            buttonSenderMoc.Margin = new System.Windows.Forms.Padding(4);
            buttonSenderMoc.Name = "buttonSenderMoc";
            buttonSenderMoc.Size = new System.Drawing.Size(97, 47);
            buttonSenderMoc.TabIndex = 15;
            buttonSenderMoc.Text = "Moc";
            buttonSenderMoc.UseVisualStyleBackColor = true;
            buttonSenderMoc.Click += button1_Click_1;
            // 
            // buttonReceiverMoc
            // 
            buttonReceiverMoc.Location = new System.Drawing.Point(472, 226);
            buttonReceiverMoc.Margin = new System.Windows.Forms.Padding(4);
            buttonReceiverMoc.Name = "buttonReceiverMoc";
            buttonReceiverMoc.Size = new System.Drawing.Size(115, 47);
            buttonReceiverMoc.TabIndex = 14;
            buttonReceiverMoc.Text = "Moc File";
            buttonReceiverMoc.UseVisualStyleBackColor = true;
            buttonReceiverMoc.Click += buttonReceiverMoc_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(17, 279);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(77, 32);
            label5.TabIndex = 13;
            label5.Text = "Filters";
            // 
            // listBox1
            // 
            listBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listBox1.ContextMenuStrip = contextMenuStripFilters;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 32;
            listBox1.Location = new System.Drawing.Point(15, 311);
            listBox1.Margin = new System.Windows.Forms.Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(968, 388);
            listBox1.TabIndex = 12;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // contextMenuStripFilters
            // 
            contextMenuStripFilters.ImageScalingSize = new System.Drawing.Size(32, 32);
            contextMenuStripFilters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addFilterToolStripMenuItem, removeFilterToolStripMenuItem });
            contextMenuStripFilters.Name = "contextMenuStripFilters";
            contextMenuStripFilters.Size = new System.Drawing.Size(231, 80);
            // 
            // addFilterToolStripMenuItem
            // 
            addFilterToolStripMenuItem.Name = "addFilterToolStripMenuItem";
            addFilterToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            addFilterToolStripMenuItem.Text = "Add filter";
            addFilterToolStripMenuItem.Click += toolStripMenuItem3_Click;
            // 
            // removeFilterToolStripMenuItem
            // 
            removeFilterToolStripMenuItem.Name = "removeFilterToolStripMenuItem";
            removeFilterToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            removeFilterToolStripMenuItem.Text = "Remove filter";
            removeFilterToolStripMenuItem.Click += toolStripMenuItem4_Click;
            // 
            // buttonTestServer
            // 
            buttonTestServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonTestServer.Enabled = false;
            buttonTestServer.Location = new System.Drawing.Point(448, 739);
            buttonTestServer.Margin = new System.Windows.Forms.Padding(4);
            buttonTestServer.Name = "buttonTestServer";
            buttonTestServer.Size = new System.Drawing.Size(149, 47);
            buttonTestServer.TabIndex = 11;
            buttonTestServer.Text = "Test";
            buttonTestServer.UseVisualStyleBackColor = true;
            buttonTestServer.Click += buttonTestServer_Click;
            // 
            // buttonTestReceiver
            // 
            buttonTestReceiver.Enabled = false;
            buttonTestReceiver.Location = new System.Drawing.Point(383, 226);
            buttonTestReceiver.Margin = new System.Windows.Forms.Padding(4);
            buttonTestReceiver.Name = "buttonTestReceiver";
            buttonTestReceiver.Size = new System.Drawing.Size(82, 47);
            buttonTestReceiver.TabIndex = 10;
            buttonTestReceiver.Text = "Test";
            buttonTestReceiver.UseVisualStyleBackColor = true;
            buttonTestReceiver.Click += buttonTestReceiver_Click;
            // 
            // button4
            // 
            button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            button4.Location = new System.Drawing.Point(15, 801);
            button4.Margin = new System.Windows.Forms.Padding(4);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(427, 47);
            button4.TabIndex = 9;
            button4.Text = "Save step";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // buttonSetupSender
            // 
            buttonSetupSender.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonSetupSender.Location = new System.Drawing.Point(15, 739);
            buttonSetupSender.Margin = new System.Windows.Forms.Padding(4);
            buttonSetupSender.Name = "buttonSetupSender";
            buttonSetupSender.Size = new System.Drawing.Size(427, 47);
            buttonSetupSender.TabIndex = 8;
            buttonSetupSender.Text = "Sender";
            buttonSetupSender.UseVisualStyleBackColor = true;
            buttonSetupSender.Click += button3_Click;
            // 
            // buttonSetupReceiver
            // 
            buttonSetupReceiver.Location = new System.Drawing.Point(15, 226);
            buttonSetupReceiver.Margin = new System.Windows.Forms.Padding(4);
            buttonSetupReceiver.Name = "buttonSetupReceiver";
            buttonSetupReceiver.Size = new System.Drawing.Size(362, 47);
            buttonSetupReceiver.TabIndex = 6;
            buttonSetupReceiver.Text = "Receiver";
            buttonSetupReceiver.UseVisualStyleBackColor = true;
            buttonSetupReceiver.Click += button1_Click;
            // 
            // textBoxStepDescription
            // 
            textBoxStepDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxStepDescription.Location = new System.Drawing.Point(178, 181);
            textBoxStepDescription.Margin = new System.Windows.Forms.Padding(4);
            textBoxStepDescription.Name = "textBoxStepDescription";
            textBoxStepDescription.Size = new System.Drawing.Size(840, 39);
            textBoxStepDescription.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 181);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(132, 32);
            label4.TabIndex = 4;
            label4.Text = "description";
            // 
            // textBoxIDPrevStep
            // 
            textBoxIDPrevStep.Location = new System.Drawing.Point(292, 30);
            textBoxIDPrevStep.Margin = new System.Windows.Forms.Padding(4);
            textBoxIDPrevStep.Name = "textBoxIDPrevStep";
            textBoxIDPrevStep.Size = new System.Drawing.Size(114, 39);
            textBoxIDPrevStep.TabIndex = 3;
            textBoxIDPrevStep.TextChanged += textBoxIDPrevStep_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(171, 32);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(129, 32);
            label3.TabIndex = 2;
            label3.Text = "IDAncestor";
            // 
            // textBoxIDStep
            // 
            textBoxIDStep.Location = new System.Drawing.Point(45, 32);
            textBoxIDStep.Margin = new System.Windows.Forms.Padding(4);
            textBoxIDStep.Name = "textBoxIDStep";
            textBoxIDStep.Size = new System.Drawing.Size(123, 39);
            textBoxIDStep.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(7, 34);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(37, 32);
            label2.TabIndex = 0;
            label2.Text = "ID";
            // 
            // textBoxPipelineDescription
            // 
            textBoxPipelineDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxPipelineDescription.Location = new System.Drawing.Point(241, 0);
            textBoxPipelineDescription.Margin = new System.Windows.Forms.Padding(4);
            textBoxPipelineDescription.Name = "textBoxPipelineDescription";
            textBoxPipelineDescription.Size = new System.Drawing.Size(788, 39);
            textBoxPipelineDescription.TabIndex = 1;
            textBoxPipelineDescription.TextChanged += textBoxPipelineDescription_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(224, 32);
            label1.TabIndex = 0;
            label1.Text = "Pipeline description";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "yml";
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxIDFamilyPrevious
            // 
            textBoxIDFamilyPrevious.Location = new System.Drawing.Point(472, 77);
            textBoxIDFamilyPrevious.Margin = new System.Windows.Forms.Padding(4);
            textBoxIDFamilyPrevious.Name = "textBoxIDFamilyPrevious";
            textBoxIDFamilyPrevious.Size = new System.Drawing.Size(114, 39);
            textBoxIDFamilyPrevious.TabIndex = 49;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(305, 79);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(151, 32);
            label8.TabIndex = 48;
            label8.Text = "IDFamilyPrev";
            // 
            // textBoxFamilyStep
            // 
            textBoxFamilyStep.Location = new System.Drawing.Point(174, 79);
            textBoxFamilyStep.Margin = new System.Windows.Forms.Padding(4);
            textBoxFamilyStep.Name = "textBoxFamilyStep";
            textBoxFamilyStep.Size = new System.Drawing.Size(123, 39);
            textBoxFamilyStep.TabIndex = 47;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(7, 81);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(152, 32);
            label9.TabIndex = 46;
            label9.Text = "IDFamilyStep";
            // 
            // FormPipeline
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1573, 929);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "FormPipeline";
            Text = "FormPipeline";
            FormClosed += FormPipeline_FormClosed;
            Load += FormPipeline_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            contextMenuStripFilters.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.TextBox textBoxRestorePath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button buttonPlantUml;
        private System.Windows.Forms.Button buttonPaste;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBoxIDFamilyPrevious;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxFamilyStep;
        private System.Windows.Forms.Label label9;
    }
}