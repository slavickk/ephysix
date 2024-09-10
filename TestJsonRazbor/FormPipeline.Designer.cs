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
            TreeNode treeNode1 = new TreeNode("Steps");
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            addStepToolStripMenuItem = new ToolStripMenuItem();
            removeStepToolStripMenuItem = new ToolStripMenuItem();
            buttonCorrectOccurences = new Button();
            button6 = new Button();
            buttonOpen = new Button();
            buttonPlantUml = new Button();
            buttonNew = new Button();
            buttonSavePipeline = new Button();
            buttonYaml = new Button();
            button1 = new Button();
            groupBox1 = new GroupBox();
            textBoxIDFamilyPrevious = new TextBox();
            label8 = new Label();
            textBoxFamilyStep = new TextBox();
            label9 = new Label();
            button8 = new Button();
            buttonSwagger = new Button();
            buttonPaste = new Button();
            buttonCopy = new Button();
            button5 = new Button();
            textBoxRestorePath = new TextBox();
            label7 = new Label();
            checkBoxHandleSendError = new CheckBox();
            checkBox1 = new CheckBox();
            textBoxResponceStep = new TextBox();
            label6 = new Label();
            button3 = new Button();
            button2 = new Button();
            buttonUp = new Button();
            buttonDown = new Button();
            buttonSenderMoc = new Button();
            buttonReceiverMoc = new Button();
            label5 = new Label();
            listBox1 = new ListBox();
            contextMenuStripFilters = new ContextMenuStrip(components);
            addFilterToolStripMenuItem = new ToolStripMenuItem();
            removeFilterToolStripMenuItem = new ToolStripMenuItem();
            buttonTestServer = new Button();
            buttonTestReceiver = new Button();
            button4 = new Button();
            buttonSetupSender = new Button();
            buttonSetupReceiver = new Button();
            textBoxStepDescription = new TextBox();
            label4 = new Label();
            textBoxIDPrevStep = new TextBox();
            label3 = new Label();
            textBoxIDStep = new TextBox();
            label2 = new Label();
            textBoxPipelineDescription = new TextBox();
            label1 = new Label();
            saveFileDialog1 = new SaveFileDialog();
            contextMenuStrip2 = new ContextMenuStrip(components);
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            iNSERToolStripMenuItem = new ToolStripMenuItem();
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
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2, 2, 2, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(buttonCorrectOccurences);
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
            splitContainer1.Size = new Size(847, 436);
            splitContainer1.SplitterDistance = 280;
            splitContainer1.SplitterWidth = 2;
            splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Margin = new Padding(2, 2, 2, 2);
            treeView1.Name = "treeView1";
            treeNode1.Name = "Steps";
            treeNode1.Text = "Steps";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode1 });
            treeView1.Size = new Size(280, 436);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            treeView1.Click += treeView1_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(32, 32);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { addStepToolStripMenuItem, removeStepToolStripMenuItem, iNSERToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 92);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // addStepToolStripMenuItem
            // 
            addStepToolStripMenuItem.Name = "addStepToolStripMenuItem";
            addStepToolStripMenuItem.Size = new Size(180, 22);
            addStepToolStripMenuItem.Text = "Add step";
            addStepToolStripMenuItem.Click += AddNode;
            // 
            // removeStepToolStripMenuItem
            // 
            removeStepToolStripMenuItem.Name = "removeStepToolStripMenuItem";
            removeStepToolStripMenuItem.Size = new Size(180, 22);
            removeStepToolStripMenuItem.Text = "Remove step";
            removeStepToolStripMenuItem.Click += removeStepToolStripMenuItem_Click;
            // 
            // buttonCorrectOccurences
            // 
            buttonCorrectOccurences.Location = new Point(442, 21);
            buttonCorrectOccurences.Margin = new Padding(1, 1, 1, 1);
            buttonCorrectOccurences.Name = "buttonCorrectOccurences";
            buttonCorrectOccurences.Size = new Size(76, 20);
            buttonCorrectOccurences.TabIndex = 35;
            buttonCorrectOccurences.Text = "Correct Occ";
            buttonCorrectOccurences.UseVisualStyleBackColor = true;
            buttonCorrectOccurences.Click += buttonCorrectOccurences_Click;
            // 
            // button6
            // 
            button6.Location = new Point(12, 15);
            button6.Margin = new Padding(1, 1, 1, 1);
            button6.Name = "button6";
            button6.Size = new Size(99, 22);
            button6.TabIndex = 34;
            button6.Text = "button6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(184, 19);
            buttonOpen.Margin = new Padding(2, 2, 2, 2);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(48, 22);
            buttonOpen.TabIndex = 33;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // buttonPlantUml
            // 
            buttonPlantUml.Location = new Point(378, 19);
            buttonPlantUml.Margin = new Padding(2, 2, 2, 2);
            buttonPlantUml.Name = "buttonPlantUml";
            buttonPlantUml.Size = new Size(48, 22);
            buttonPlantUml.TabIndex = 33;
            buttonPlantUml.Text = "ToMd";
            buttonPlantUml.UseVisualStyleBackColor = true;
            buttonPlantUml.Click += buttonPlantUml_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new Point(124, 19);
            buttonNew.Margin = new Padding(2, 2, 2, 2);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(50, 22);
            buttonNew.TabIndex = 6;
            buttonNew.Text = "New";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonSavePipeline
            // 
            buttonSavePipeline.Location = new Point(228, 19);
            buttonSavePipeline.Margin = new Padding(2, 2, 2, 2);
            buttonSavePipeline.Name = "buttonSavePipeline";
            buttonSavePipeline.Size = new Size(43, 22);
            buttonSavePipeline.TabIndex = 33;
            buttonSavePipeline.Text = "Save";
            buttonSavePipeline.UseVisualStyleBackColor = true;
            buttonSavePipeline.Click += buttonSavePipeline_Click;
            // 
            // buttonYaml
            // 
            buttonYaml.Location = new Point(275, 19);
            buttonYaml.Margin = new Padding(2, 2, 2, 2);
            buttonYaml.Name = "buttonYaml";
            buttonYaml.Size = new Size(40, 22);
            buttonYaml.TabIndex = 4;
            buttonYaml.Text = "Yaml";
            buttonYaml.UseVisualStyleBackColor = true;
            buttonYaml.Click += buttonYaml_Click;
            // 
            // button1
            // 
            button1.Location = new Point(324, 19);
            button1.Margin = new Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new Size(36, 22);
            button1.TabIndex = 5;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_2;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(textBoxIDFamilyPrevious);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(textBoxFamilyStep);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(button8);
            groupBox1.Controls.Add(buttonSwagger);
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
            groupBox1.Location = new Point(6, 32);
            groupBox1.Margin = new Padding(2, 2, 2, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2, 2, 2, 2);
            groupBox1.Size = new Size(557, 401);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Step detail";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // textBoxIDFamilyPrevious
            // 
            textBoxIDFamilyPrevious.Location = new Point(254, 36);
            textBoxIDFamilyPrevious.Margin = new Padding(2, 2, 2, 2);
            textBoxIDFamilyPrevious.Name = "textBoxIDFamilyPrevious";
            textBoxIDFamilyPrevious.Size = new Size(64, 23);
            textBoxIDFamilyPrevious.TabIndex = 49;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(164, 37);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(76, 15);
            label8.TabIndex = 48;
            label8.Text = "IDFamilyPrev";
            // 
            // textBoxFamilyStep
            // 
            textBoxFamilyStep.Location = new Point(94, 37);
            textBoxFamilyStep.Margin = new Padding(2, 2, 2, 2);
            textBoxFamilyStep.Name = "textBoxFamilyStep";
            textBoxFamilyStep.Size = new Size(68, 23);
            textBoxFamilyStep.TabIndex = 47;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(4, 38);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(76, 15);
            label9.TabIndex = 46;
            label9.Text = "IDFamilyStep";
            // 
            // button8
            // 
            button8.Location = new Point(400, 107);
            button8.Margin = new Padding(1, 1, 1, 1);
            button8.Name = "button8";
            button8.Size = new Size(44, 22);
            button8.TabIndex = 45;
            button8.Text = "button8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // buttonSwagger
            // 
            buttonSwagger.Location = new Point(456, 107);
            buttonSwagger.Margin = new Padding(1, 1, 1, 1);
            buttonSwagger.Name = "buttonSwagger";
            buttonSwagger.Size = new Size(48, 22);
            buttonSwagger.TabIndex = 44;
            buttonSwagger.Text = "Swag";
            buttonSwagger.UseVisualStyleBackColor = true;
            buttonSwagger.Click += button7_Click;
            // 
            // buttonPaste
            // 
            buttonPaste.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPaste.Location = new Point(533, 250);
            buttonPaste.Margin = new Padding(1, 1, 1, 1);
            buttonPaste.Name = "buttonPaste";
            buttonPaste.Size = new Size(24, 22);
            buttonPaste.TabIndex = 43;
            buttonPaste.Text = "p";
            buttonPaste.UseVisualStyleBackColor = true;
            buttonPaste.Click += buttonPaste_Click;
            // 
            // buttonCopy
            // 
            buttonCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonCopy.Location = new Point(533, 227);
            buttonCopy.Margin = new Padding(1, 1, 1, 1);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new Size(24, 22);
            buttonCopy.TabIndex = 42;
            buttonCopy.Text = "c";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button5.Location = new Point(242, 376);
            button5.Margin = new Padding(1, 1, 1, 1);
            button5.Name = "button5";
            button5.Size = new Size(136, 22);
            button5.TabIndex = 41;
            button5.Text = "Pipeline metrics";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBoxRestorePath
            // 
            textBoxRestorePath.Location = new Point(234, 65);
            textBoxRestorePath.Margin = new Padding(1, 1, 1, 1);
            textBoxRestorePath.Name = "textBoxRestorePath";
            textBoxRestorePath.Size = new Size(110, 23);
            textBoxRestorePath.TabIndex = 40;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(155, 67);
            label7.Margin = new Padding(1, 0, 1, 0);
            label7.Name = "label7";
            label7.Size = new Size(70, 15);
            label7.TabIndex = 39;
            label7.Text = "RestorePath";
            // 
            // checkBoxHandleSendError
            // 
            checkBoxHandleSendError.AutoSize = true;
            checkBoxHandleSendError.Location = new Point(9, 67);
            checkBoxHandleSendError.Margin = new Padding(1, 1, 1, 1);
            checkBoxHandleSendError.Name = "checkBoxHandleSendError";
            checkBoxHandleSendError.Size = new Size(121, 19);
            checkBoxHandleSendError.TabIndex = 38;
            checkBoxHandleSendError.Text = "Handle Send Error";
            checkBoxHandleSendError.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(349, 15);
            checkBox1.Margin = new Padding(4, 3, 4, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(60, 19);
            checkBox1.TabIndex = 37;
            checkBox1.Text = "bridge";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            // 
            // textBoxResponceStep
            // 
            textBoxResponceStep.Location = new Point(275, 12);
            textBoxResponceStep.Margin = new Padding(2, 2, 2, 2);
            textBoxResponceStep.Name = "textBoxResponceStep";
            textBoxResponceStep.Size = new Size(70, 23);
            textBoxResponceStep.TabIndex = 36;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(220, 14);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 35;
            label6.Text = "RespStep";
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button3.Location = new Point(382, 376);
            button3.Margin = new Padding(2, 2, 2, 2);
            button3.Name = "button3";
            button3.Size = new Size(40, 20);
            button3.TabIndex = 34;
            button3.Text = "dmn";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // button2
            // 
            button2.Location = new Point(318, 106);
            button2.Margin = new Padding(2, 2, 2, 2);
            button2.Name = "button2";
            button2.Size = new Size(66, 22);
            button2.TabIndex = 33;
            button2.Text = "Moc Text";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // buttonUp
            // 
            buttonUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonUp.Location = new Point(533, 149);
            buttonUp.Margin = new Padding(2, 2, 2, 2);
            buttonUp.Name = "buttonUp";
            buttonUp.Size = new Size(22, 22);
            buttonUp.TabIndex = 31;
            buttonUp.Text = "^";
            buttonUp.UseVisualStyleBackColor = true;
            buttonUp.Click += buttonUp_Click;
            // 
            // buttonDown
            // 
            buttonDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDown.Font = new Font("Segoe UI", 6F, FontStyle.Regular, GraphicsUnit.Point);
            buttonDown.Location = new Point(533, 174);
            buttonDown.Margin = new Padding(2, 2, 2, 2);
            buttonDown.Name = "buttonDown";
            buttonDown.Size = new Size(23, 22);
            buttonDown.TabIndex = 32;
            buttonDown.Text = "V";
            buttonDown.UseVisualStyleBackColor = true;
            // 
            // buttonSenderMoc
            // 
            buttonSenderMoc.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonSenderMoc.Location = new Point(326, 347);
            buttonSenderMoc.Margin = new Padding(2, 2, 2, 2);
            buttonSenderMoc.Name = "buttonSenderMoc";
            buttonSenderMoc.Size = new Size(52, 22);
            buttonSenderMoc.TabIndex = 15;
            buttonSenderMoc.Text = "Moc";
            buttonSenderMoc.UseVisualStyleBackColor = true;
            buttonSenderMoc.Click += button1_Click_1;
            // 
            // buttonReceiverMoc
            // 
            buttonReceiverMoc.Location = new Point(254, 106);
            buttonReceiverMoc.Margin = new Padding(2, 2, 2, 2);
            buttonReceiverMoc.Name = "buttonReceiverMoc";
            buttonReceiverMoc.Size = new Size(62, 22);
            buttonReceiverMoc.TabIndex = 14;
            buttonReceiverMoc.Text = "Moc File";
            buttonReceiverMoc.UseVisualStyleBackColor = true;
            buttonReceiverMoc.Click += buttonReceiverMoc_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(9, 131);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(38, 15);
            label5.TabIndex = 13;
            label5.Text = "Filters";
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.ContextMenuStrip = contextMenuStripFilters;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(8, 146);
            listBox1.Margin = new Padding(2, 2, 2, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(524, 184);
            listBox1.TabIndex = 12;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // contextMenuStripFilters
            // 
            contextMenuStripFilters.ImageScalingSize = new Size(32, 32);
            contextMenuStripFilters.Items.AddRange(new ToolStripItem[] { addFilterToolStripMenuItem, removeFilterToolStripMenuItem });
            contextMenuStripFilters.Name = "contextMenuStripFilters";
            contextMenuStripFilters.Size = new Size(145, 48);
            // 
            // addFilterToolStripMenuItem
            // 
            addFilterToolStripMenuItem.Name = "addFilterToolStripMenuItem";
            addFilterToolStripMenuItem.Size = new Size(144, 22);
            addFilterToolStripMenuItem.Text = "Add filter";
            addFilterToolStripMenuItem.Click += toolStripMenuItem3_Click;
            // 
            // removeFilterToolStripMenuItem
            // 
            removeFilterToolStripMenuItem.Name = "removeFilterToolStripMenuItem";
            removeFilterToolStripMenuItem.Size = new Size(144, 22);
            removeFilterToolStripMenuItem.Text = "Remove filter";
            removeFilterToolStripMenuItem.Click += toolStripMenuItem4_Click;
            // 
            // buttonTestServer
            // 
            buttonTestServer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonTestServer.Enabled = false;
            buttonTestServer.Location = new Point(242, 347);
            buttonTestServer.Margin = new Padding(2, 2, 2, 2);
            buttonTestServer.Name = "buttonTestServer";
            buttonTestServer.Size = new Size(80, 22);
            buttonTestServer.TabIndex = 11;
            buttonTestServer.Text = "Test";
            buttonTestServer.UseVisualStyleBackColor = true;
            buttonTestServer.Click += buttonTestServer_Click;
            // 
            // buttonTestReceiver
            // 
            buttonTestReceiver.Enabled = false;
            buttonTestReceiver.Location = new Point(206, 106);
            buttonTestReceiver.Margin = new Padding(2, 2, 2, 2);
            buttonTestReceiver.Name = "buttonTestReceiver";
            buttonTestReceiver.Size = new Size(44, 22);
            buttonTestReceiver.TabIndex = 10;
            buttonTestReceiver.Text = "Test";
            buttonTestReceiver.UseVisualStyleBackColor = true;
            buttonTestReceiver.Click += buttonTestReceiver_Click;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button4.Location = new Point(8, 376);
            button4.Margin = new Padding(2, 2, 2, 2);
            button4.Name = "button4";
            button4.Size = new Size(230, 22);
            button4.TabIndex = 9;
            button4.Text = "Save step";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // buttonSetupSender
            // 
            buttonSetupSender.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonSetupSender.Location = new Point(8, 347);
            buttonSetupSender.Margin = new Padding(2, 2, 2, 2);
            buttonSetupSender.Name = "buttonSetupSender";
            buttonSetupSender.Size = new Size(230, 22);
            buttonSetupSender.TabIndex = 8;
            buttonSetupSender.Text = "Sender";
            buttonSetupSender.UseVisualStyleBackColor = true;
            buttonSetupSender.Click += button3_Click;
            // 
            // buttonSetupReceiver
            // 
            buttonSetupReceiver.Location = new Point(8, 106);
            buttonSetupReceiver.Margin = new Padding(2, 2, 2, 2);
            buttonSetupReceiver.Name = "buttonSetupReceiver";
            buttonSetupReceiver.Size = new Size(195, 22);
            buttonSetupReceiver.TabIndex = 6;
            buttonSetupReceiver.Text = "Receiver";
            buttonSetupReceiver.UseVisualStyleBackColor = true;
            buttonSetupReceiver.Click += button1_Click;
            // 
            // textBoxStepDescription
            // 
            textBoxStepDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxStepDescription.Location = new Point(96, 85);
            textBoxStepDescription.Margin = new Padding(2, 2, 2, 2);
            textBoxStepDescription.Name = "textBoxStepDescription";
            textBoxStepDescription.Size = new Size(455, 23);
            textBoxStepDescription.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(4, 85);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 4;
            label4.Text = "description";
            // 
            // textBoxIDPrevStep
            // 
            textBoxIDPrevStep.Location = new Point(158, 14);
            textBoxIDPrevStep.Margin = new Padding(2, 2, 2, 2);
            textBoxIDPrevStep.Name = "textBoxIDPrevStep";
            textBoxIDPrevStep.Size = new Size(64, 23);
            textBoxIDPrevStep.TabIndex = 3;
            textBoxIDPrevStep.TextChanged += textBoxIDPrevStep_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(92, 15);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(65, 15);
            label3.TabIndex = 2;
            label3.Text = "IDAncestor";
            // 
            // textBoxIDStep
            // 
            textBoxIDStep.Location = new Point(24, 15);
            textBoxIDStep.Margin = new Padding(2, 2, 2, 2);
            textBoxIDStep.Name = "textBoxIDStep";
            textBoxIDStep.Size = new Size(68, 23);
            textBoxIDStep.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 16);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 0;
            label2.Text = "ID";
            // 
            // textBoxPipelineDescription
            // 
            textBoxPipelineDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPipelineDescription.Location = new Point(130, 0);
            textBoxPipelineDescription.Margin = new Padding(2, 2, 2, 2);
            textBoxPipelineDescription.Name = "textBoxPipelineDescription";
            textBoxPipelineDescription.Size = new Size(427, 23);
            textBoxPipelineDescription.TabIndex = 1;
            textBoxPipelineDescription.TextChanged += textBoxPipelineDescription_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(111, 15);
            label1.TabIndex = 0;
            label1.Text = "Pipeline description";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "yml";
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new Size(32, 32);
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(61, 4);
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // iNSERToolStripMenuItem
            // 
            iNSERToolStripMenuItem.Name = "iNSERToolStripMenuItem";
            iNSERToolStripMenuItem.Size = new Size(180, 22);
            iNSERToolStripMenuItem.Text = "Insert between";
            iNSERToolStripMenuItem.Click += iNSERToolStripMenuItem_Click;
            // 
            // FormPipeline
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 436);
            Controls.Add(splitContainer1);
            Margin = new Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.Button buttonSwagger;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBoxIDFamilyPrevious;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxFamilyStep;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonCorrectOccurences;
        private ToolStripMenuItem iNSERToolStripMenuItem;
    }
}