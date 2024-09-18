namespace TestJsonRazbor
{
    partial class FormSwaggerFromXML
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
            listViewInput = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            columnHeader9 = new ColumnHeader();
            columnHeader10 = new ColumnHeader();
            columnHeader14 = new ColumnHeader();
            columnHeader15 = new ColumnHeader();
            columnHeader17 = new ColumnHeader();
            columnHeader19 = new ColumnHeader();
            label1 = new Label();
            button1 = new Button();
            label2 = new Label();
            textBoxAPIPath = new TextBox();
            label3 = new Label();
            buttonFormJson = new Button();
            button2 = new Button();
            label4 = new Label();
            listViewOutput = new ListView();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            columnHeader11 = new ColumnHeader();
            columnHeader12 = new ColumnHeader();
            columnHeader13 = new ColumnHeader();
            columnHeader16 = new ColumnHeader();
            columnHeader18 = new ColumnHeader();
            columnHeader20 = new ColumnHeader();
            textBoxNewNameInput = new TextBox();
            label5 = new Label();
            checkBox1 = new CheckBox();
            label6 = new Label();
            textBoxNewNameOutput = new TextBox();
            comboBoxCommand = new ComboBox();
            label7 = new Label();
            label8 = new Label();
            comboBoxMethod = new ComboBox();
            label9 = new Label();
            textBoxDescription = new TextBox();
            label10 = new Label();
            textBoxSummary = new TextBox();
            listBoxTags = new ListBox();
            comboBoxTag = new ComboBox();
            label11 = new Label();
            textBoxInputType = new TextBox();
            textBoxInputFormat = new TextBox();
            label12 = new Label();
            textBoxInputExample = new TextBox();
            label13 = new Label();
            textBoxOutputExample = new TextBox();
            label14 = new Label();
            textBoxOutputFormat = new TextBox();
            label15 = new Label();
            textBoxOutputType = new TextBox();
            label16 = new Label();
            checkBoxInputRequired = new CheckBox();
            buttonAddRequest = new Button();
            textBoxDescrReq = new TextBox();
            buttonRemove = new Button();
            checkBoxAddToExist = new CheckBox();
            comboBoxAllPaths = new ComboBox();
            label17 = new Label();
            checkBoxSavePipeline = new CheckBox();
            checkBoxOnlyCurrent = new CheckBox();
            buttonCheckContent = new Button();
            checkBoxRepeatable = new CheckBox();
            SuspendLayout();
            // 
            // listViewInput
            // 
            listViewInput.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader5, columnHeader6, columnHeader8, columnHeader9, columnHeader10, columnHeader14, columnHeader15, columnHeader17, columnHeader19 });
            listViewInput.FullRowSelect = true;
            listViewInput.Location = new Point(0, 136);
            listViewInput.Margin = new Padding(2, 1, 2, 1);
            listViewInput.Name = "listViewInput";
            listViewInput.Size = new Size(1061, 236);
            listViewInput.TabIndex = 0;
            listViewInput.UseCompatibleStateImageBehavior = false;
            listViewInput.View = View.Details;
            listViewInput.SelectedIndexChanged += listViewInput_SelectedIndexChanged;
            listViewInput.KeyDown += listViewInput_KeyDown;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Key";
            columnHeader1.Width = 560;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Description";
            columnHeader2.Width = 400;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "NewName";
            columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Type";
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Format";
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "Example";
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "Req";
            // 
            // columnHeader14
            // 
            columnHeader14.Text = "Param";
            // 
            // columnHeader15
            // 
            columnHeader15.Text = "req";
            // 
            // columnHeader17
            // 
            columnHeader17.Text = "Source";
            // 
            // columnHeader19
            // 
            columnHeader19.Text = "out path";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 120);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 1;
            label1.Text = "Request";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top;
            button1.Location = new Point(1083, 155);
            button1.Margin = new Padding(2, 1, 2, 1);
            button1.Name = "button1";
            button1.Size = new Size(81, 22);
            button1.TabIndex = 2;
            button1.Text = "Select";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 4);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(91, 15);
            label2.TabIndex = 3;
            label2.Text = "XML Command";
            label2.Click += label2_Click;
            // 
            // textBoxAPIPath
            // 
            textBoxAPIPath.Location = new Point(592, 2);
            textBoxAPIPath.Margin = new Padding(2, 1, 2, 1);
            textBoxAPIPath.Name = "textBoxAPIPath";
            textBoxAPIPath.Size = new Size(110, 23);
            textBoxAPIPath.TabIndex = 6;
            textBoxAPIPath.TextChanged += textBoxAPIPath_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(528, 2);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 5;
            label3.Text = "API Path";
            // 
            // buttonFormJson
            // 
            buttonFormJson.Anchor = AnchorStyles.Top;
            buttonFormJson.Location = new Point(1077, 2);
            buttonFormJson.Margin = new Padding(2, 1, 2, 1);
            buttonFormJson.Name = "buttonFormJson";
            buttonFormJson.Size = new Size(81, 22);
            buttonFormJson.TabIndex = 7;
            buttonFormJson.Text = "Form JSON";
            buttonFormJson.UseVisualStyleBackColor = true;
            buttonFormJson.Click += buttonFormJson_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top;
            button2.Location = new Point(1083, 402);
            button2.Margin = new Padding(2, 1, 2, 1);
            button2.Name = "button2";
            button2.Size = new Size(81, 22);
            button2.TabIndex = 10;
            button2.Text = "Select";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(0, 378);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 9;
            label4.Text = "Response";
            // 
            // listViewOutput
            // 
            listViewOutput.AllowColumnReorder = true;
            listViewOutput.Columns.AddRange(new ColumnHeader[] { columnHeader3, columnHeader4, columnHeader7, columnHeader11, columnHeader12, columnHeader13, columnHeader16, columnHeader18, columnHeader20 });
            listViewOutput.FullRowSelect = true;
            listViewOutput.Location = new Point(0, 401);
            listViewOutput.Margin = new Padding(2, 1, 2, 1);
            listViewOutput.Name = "listViewOutput";
            listViewOutput.Size = new Size(1061, 175);
            listViewOutput.TabIndex = 8;
            listViewOutput.UseCompatibleStateImageBehavior = false;
            listViewOutput.View = View.Details;
            listViewOutput.SelectedIndexChanged += listViewOutput_SelectedIndexChanged;
            listViewOutput.KeyDown += listViewOutput_KeyDown;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Key";
            columnHeader3.Width = 560;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Description";
            columnHeader4.Width = 400;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "NewName";
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "Type";
            // 
            // columnHeader12
            // 
            columnHeader12.Text = "Format";
            // 
            // columnHeader13
            // 
            columnHeader13.Text = "Example";
            // 
            // columnHeader16
            // 
            columnHeader16.Text = "req";
            // 
            // columnHeader18
            // 
            columnHeader18.Text = "Source";
            // 
            // columnHeader20
            // 
            columnHeader20.Text = "out path";
            // 
            // textBoxNewNameInput
            // 
            textBoxNewNameInput.Anchor = AnchorStyles.Top;
            textBoxNewNameInput.Location = new Point(1083, 196);
            textBoxNewNameInput.Margin = new Padding(2, 1, 2, 1);
            textBoxNewNameInput.Name = "textBoxNewNameInput";
            textBoxNewNameInput.Size = new Size(77, 23);
            textBoxNewNameInput.TabIndex = 11;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Location = new Point(1083, 179);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(66, 15);
            label5.TabIndex = 12;
            label5.Text = "New Name";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Top;
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(1083, 219);
            checkBox1.Margin = new Padding(2, 1, 2, 1);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(60, 19);
            checkBox1.TabIndex = 13;
            checkBox1.Text = "Param";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top;
            label6.AutoSize = true;
            label6.Location = new Point(1083, 430);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(66, 15);
            label6.TabIndex = 16;
            label6.Text = "New Name";
            // 
            // textBoxNewNameOutput
            // 
            textBoxNewNameOutput.Anchor = AnchorStyles.Top;
            textBoxNewNameOutput.Location = new Point(1083, 446);
            textBoxNewNameOutput.Margin = new Padding(2, 1, 2, 1);
            textBoxNewNameOutput.Name = "textBoxNewNameOutput";
            textBoxNewNameOutput.Size = new Size(77, 23);
            textBoxNewNameOutput.TabIndex = 15;
            // 
            // comboBoxCommand
            // 
            comboBoxCommand.FormattingEnabled = true;
            comboBoxCommand.Location = new Point(106, 3);
            comboBoxCommand.Margin = new Padding(2, 1, 2, 1);
            comboBoxCommand.Name = "comboBoxCommand";
            comboBoxCommand.Size = new Size(117, 23);
            comboBoxCommand.TabIndex = 17;
            comboBoxCommand.SelectedIndexChanged += comboBoxCommand_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(342, 42);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(0, 15);
            label7.TabIndex = 18;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(11, 25);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(49, 15);
            label8.TabIndex = 19;
            label8.Text = "Method";
            // 
            // comboBoxMethod
            // 
            comboBoxMethod.FormattingEnabled = true;
            comboBoxMethod.Location = new Point(68, 25);
            comboBoxMethod.Margin = new Padding(2, 1, 2, 1);
            comboBoxMethod.Name = "comboBoxMethod";
            comboBoxMethod.Size = new Size(72, 23);
            comboBoxMethod.TabIndex = 20;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(149, 26);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(67, 15);
            label9.TabIndex = 21;
            label9.Text = "Description";
            label9.Click += label9_Click;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Location = new Point(232, 26);
            textBoxDescription.Margin = new Padding(2, 1, 2, 1);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.Size = new Size(829, 45);
            textBoxDescription.TabIndex = 22;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 76);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(58, 15);
            label10.TabIndex = 23;
            label10.Text = "Summary";
            label10.Click += label10_Click;
            // 
            // textBoxSummary
            // 
            textBoxSummary.Location = new Point(68, 75);
            textBoxSummary.Margin = new Padding(2, 1, 2, 1);
            textBoxSummary.Multiline = true;
            textBoxSummary.Name = "textBoxSummary";
            textBoxSummary.Size = new Size(335, 39);
            textBoxSummary.TabIndex = 24;
            // 
            // listBoxTags
            // 
            listBoxTags.FormattingEnabled = true;
            listBoxTags.ItemHeight = 15;
            listBoxTags.Location = new Point(409, 73);
            listBoxTags.Margin = new Padding(2, 1, 2, 1);
            listBoxTags.Name = "listBoxTags";
            listBoxTags.Size = new Size(131, 34);
            listBoxTags.TabIndex = 25;
            // 
            // comboBoxTag
            // 
            comboBoxTag.FormattingEnabled = true;
            comboBoxTag.Location = new Point(561, 72);
            comboBoxTag.Margin = new Padding(2, 1, 2, 1);
            comboBoxTag.Name = "comboBoxTag";
            comboBoxTag.Size = new Size(383, 23);
            comboBoxTag.TabIndex = 26;
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top;
            label11.AutoSize = true;
            label11.Location = new Point(1083, 237);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(31, 15);
            label11.TabIndex = 27;
            label11.Text = "Type";
            label11.Click += label11_Click;
            // 
            // textBoxInputType
            // 
            textBoxInputType.Anchor = AnchorStyles.Top;
            textBoxInputType.Location = new Point(1083, 254);
            textBoxInputType.Margin = new Padding(2, 1, 2, 1);
            textBoxInputType.Name = "textBoxInputType";
            textBoxInputType.Size = new Size(83, 23);
            textBoxInputType.TabIndex = 28;
            // 
            // textBoxInputFormat
            // 
            textBoxInputFormat.Anchor = AnchorStyles.Top;
            textBoxInputFormat.Location = new Point(1083, 289);
            textBoxInputFormat.Margin = new Padding(2, 1, 2, 1);
            textBoxInputFormat.Name = "textBoxInputFormat";
            textBoxInputFormat.Size = new Size(83, 23);
            textBoxInputFormat.TabIndex = 30;
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.Top;
            label12.AutoSize = true;
            label12.Location = new Point(1083, 273);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(45, 15);
            label12.TabIndex = 29;
            label12.Text = "Format";
            // 
            // textBoxInputExample
            // 
            textBoxInputExample.Anchor = AnchorStyles.Top;
            textBoxInputExample.Location = new Point(1083, 327);
            textBoxInputExample.Margin = new Padding(2, 1, 2, 1);
            textBoxInputExample.Name = "textBoxInputExample";
            textBoxInputExample.Size = new Size(83, 23);
            textBoxInputExample.TabIndex = 32;
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.Top;
            label13.AutoSize = true;
            label13.Location = new Point(1083, 311);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.Size = new Size(52, 15);
            label13.TabIndex = 31;
            label13.Text = "Example";
            // 
            // textBoxOutputExample
            // 
            textBoxOutputExample.Anchor = AnchorStyles.Top;
            textBoxOutputExample.Location = new Point(1083, 558);
            textBoxOutputExample.Margin = new Padding(2, 1, 2, 1);
            textBoxOutputExample.Name = "textBoxOutputExample";
            textBoxOutputExample.Size = new Size(83, 23);
            textBoxOutputExample.TabIndex = 38;
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.Top;
            label14.AutoSize = true;
            label14.Location = new Point(1083, 542);
            label14.Margin = new Padding(2, 0, 2, 0);
            label14.Name = "label14";
            label14.Size = new Size(52, 15);
            label14.TabIndex = 37;
            label14.Text = "Example";
            // 
            // textBoxOutputFormat
            // 
            textBoxOutputFormat.Anchor = AnchorStyles.Top;
            textBoxOutputFormat.Location = new Point(1083, 520);
            textBoxOutputFormat.Margin = new Padding(2, 1, 2, 1);
            textBoxOutputFormat.Name = "textBoxOutputFormat";
            textBoxOutputFormat.Size = new Size(83, 23);
            textBoxOutputFormat.TabIndex = 36;
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Top;
            label15.AutoSize = true;
            label15.Location = new Point(1083, 504);
            label15.Margin = new Padding(2, 0, 2, 0);
            label15.Name = "label15";
            label15.Size = new Size(45, 15);
            label15.TabIndex = 35;
            label15.Text = "Format";
            // 
            // textBoxOutputType
            // 
            textBoxOutputType.Anchor = AnchorStyles.Top;
            textBoxOutputType.Location = new Point(1083, 485);
            textBoxOutputType.Margin = new Padding(2, 1, 2, 1);
            textBoxOutputType.Name = "textBoxOutputType";
            textBoxOutputType.Size = new Size(83, 23);
            textBoxOutputType.TabIndex = 34;
            // 
            // label16
            // 
            label16.Anchor = AnchorStyles.Top;
            label16.AutoSize = true;
            label16.Location = new Point(1083, 468);
            label16.Margin = new Padding(2, 0, 2, 0);
            label16.Name = "label16";
            label16.Size = new Size(31, 15);
            label16.TabIndex = 33;
            label16.Text = "Type";
            // 
            // checkBoxInputRequired
            // 
            checkBoxInputRequired.Anchor = AnchorStyles.Top;
            checkBoxInputRequired.AutoSize = true;
            checkBoxInputRequired.Location = new Point(1087, 348);
            checkBoxInputRequired.Margin = new Padding(2, 1, 2, 1);
            checkBoxInputRequired.Name = "checkBoxInputRequired";
            checkBoxInputRequired.Size = new Size(70, 19);
            checkBoxInputRequired.TabIndex = 40;
            checkBoxInputRequired.Text = "required";
            checkBoxInputRequired.UseVisualStyleBackColor = true;
            // 
            // buttonAddRequest
            // 
            buttonAddRequest.Anchor = AnchorStyles.Top;
            buttonAddRequest.Location = new Point(1083, 107);
            buttonAddRequest.Margin = new Padding(2, 1, 2, 1);
            buttonAddRequest.Name = "buttonAddRequest";
            buttonAddRequest.Size = new Size(81, 22);
            buttonAddRequest.TabIndex = 41;
            buttonAddRequest.Text = "Add ";
            buttonAddRequest.UseVisualStyleBackColor = true;
            buttonAddRequest.Click += buttonAddRequest_Click;
            // 
            // textBoxDescrReq
            // 
            textBoxDescrReq.Location = new Point(344, 115);
            textBoxDescrReq.Margin = new Padding(2, 1, 2, 1);
            textBoxDescrReq.Name = "textBoxDescrReq";
            textBoxDescrReq.Size = new Size(717, 23);
            textBoxDescrReq.TabIndex = 42;
            textBoxDescrReq.TextChanged += textBoxDescrReq_TextChanged;
            // 
            // buttonRemove
            // 
            buttonRemove.Anchor = AnchorStyles.Top;
            buttonRemove.Location = new Point(1083, 131);
            buttonRemove.Margin = new Padding(2, 1, 2, 1);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(81, 22);
            buttonRemove.TabIndex = 43;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddToExist
            // 
            checkBoxAddToExist.AutoSize = true;
            checkBoxAddToExist.Location = new Point(232, 2);
            checkBoxAddToExist.Margin = new Padding(2, 1, 2, 1);
            checkBoxAddToExist.Name = "checkBoxAddToExist";
            checkBoxAddToExist.Size = new Size(89, 19);
            checkBoxAddToExist.TabIndex = 44;
            checkBoxAddToExist.Text = "Add to exist";
            checkBoxAddToExist.UseVisualStyleBackColor = true;
            // 
            // comboBoxAllPaths
            // 
            comboBoxAllPaths.FormattingEnabled = true;
            comboBoxAllPaths.Location = new Point(411, 1);
            comboBoxAllPaths.Margin = new Padding(2, 1, 2, 1);
            comboBoxAllPaths.Name = "comboBoxAllPaths";
            comboBoxAllPaths.Size = new Size(117, 23);
            comboBoxAllPaths.TabIndex = 46;
            comboBoxAllPaths.SelectedIndexChanged += comboBoxAllPaths_SelectedIndexChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(342, 3);
            label17.Margin = new Padding(2, 0, 2, 0);
            label17.Name = "label17";
            label17.Size = new Size(53, 15);
            label17.TabIndex = 45;
            label17.Text = "All Paths";
            // 
            // checkBoxSavePipeline
            // 
            checkBoxSavePipeline.AutoSize = true;
            checkBoxSavePipeline.Location = new Point(800, 3);
            checkBoxSavePipeline.Margin = new Padding(2, 1, 2, 1);
            checkBoxSavePipeline.Name = "checkBoxSavePipeline";
            checkBoxSavePipeline.Size = new Size(95, 19);
            checkBoxSavePipeline.TabIndex = 47;
            checkBoxSavePipeline.Text = "Save pipeline";
            checkBoxSavePipeline.UseVisualStyleBackColor = true;
            // 
            // checkBoxOnlyCurrent
            // 
            checkBoxOnlyCurrent.AutoSize = true;
            checkBoxOnlyCurrent.Checked = true;
            checkBoxOnlyCurrent.CheckState = CheckState.Checked;
            checkBoxOnlyCurrent.Location = new Point(705, 3);
            checkBoxOnlyCurrent.Margin = new Padding(2, 1, 2, 1);
            checkBoxOnlyCurrent.Name = "checkBoxOnlyCurrent";
            checkBoxOnlyCurrent.Size = new Size(73, 19);
            checkBoxOnlyCurrent.TabIndex = 48;
            checkBoxOnlyCurrent.TabStop = false;
            checkBoxOnlyCurrent.Text = "only curr";
            checkBoxOnlyCurrent.UseVisualStyleBackColor = true;
            // 
            // buttonCheckContent
            // 
            buttonCheckContent.Location = new Point(6, 48);
            buttonCheckContent.Margin = new Padding(2, 1, 2, 1);
            buttonCheckContent.Name = "buttonCheckContent";
            buttonCheckContent.Size = new Size(107, 22);
            buttonCheckContent.TabIndex = 49;
            buttonCheckContent.Text = "CheckContent";
            buttonCheckContent.UseVisualStyleBackColor = true;
            buttonCheckContent.Click += buttonCheckContent_Click;
            // 
            // checkBoxRepeatable
            // 
            checkBoxRepeatable.Anchor = AnchorStyles.Top;
            checkBoxRepeatable.AutoSize = true;
            checkBoxRepeatable.Location = new Point(1089, 368);
            checkBoxRepeatable.Margin = new Padding(2, 1, 2, 1);
            checkBoxRepeatable.Name = "checkBoxRepeatable";
            checkBoxRepeatable.Size = new Size(81, 19);
            checkBoxRepeatable.TabIndex = 50;
            checkBoxRepeatable.Text = "repeteable";
            checkBoxRepeatable.UseVisualStyleBackColor = true;
            // 
            // FormSwaggerFromXML
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1173, 591);
            Controls.Add(checkBoxRepeatable);
            Controls.Add(buttonCheckContent);
            Controls.Add(checkBoxOnlyCurrent);
            Controls.Add(checkBoxSavePipeline);
            Controls.Add(comboBoxAllPaths);
            Controls.Add(label17);
            Controls.Add(checkBoxAddToExist);
            Controls.Add(buttonRemove);
            Controls.Add(textBoxDescrReq);
            Controls.Add(buttonAddRequest);
            Controls.Add(checkBoxInputRequired);
            Controls.Add(textBoxOutputExample);
            Controls.Add(label14);
            Controls.Add(textBoxOutputFormat);
            Controls.Add(label15);
            Controls.Add(textBoxOutputType);
            Controls.Add(label16);
            Controls.Add(textBoxInputExample);
            Controls.Add(label13);
            Controls.Add(textBoxInputFormat);
            Controls.Add(label12);
            Controls.Add(textBoxInputType);
            Controls.Add(label11);
            Controls.Add(comboBoxTag);
            Controls.Add(listBoxTags);
            Controls.Add(textBoxSummary);
            Controls.Add(label10);
            Controls.Add(textBoxDescription);
            Controls.Add(label9);
            Controls.Add(comboBoxMethod);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(comboBoxCommand);
            Controls.Add(label6);
            Controls.Add(textBoxNewNameOutput);
            Controls.Add(checkBox1);
            Controls.Add(label5);
            Controls.Add(textBoxNewNameInput);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(listViewOutput);
            Controls.Add(buttonFormJson);
            Controls.Add(textBoxAPIPath);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(listViewInput);
            Margin = new Padding(2, 1, 2, 1);
            Name = "FormSwaggerFromXML";
            Text = "FormSwaggerFromXML";
            Load += FormSwaggerFromXML_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listViewInput;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAPIPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonFormJson;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listViewOutput;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox textBoxNewNameInput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxNewNameOutput;
        private System.Windows.Forms.ComboBox comboBoxCommand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxSummary;
        private System.Windows.Forms.ListBox listBoxTags;
        private System.Windows.Forms.ComboBox comboBoxTag;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxInputType;
        private System.Windows.Forms.TextBox textBoxInputFormat;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxInputExample;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.TextBox textBoxOutputExample;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxOutputFormat;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxOutputType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.CheckBox checkBoxInputRequired;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.Button buttonAddRequest;
        private System.Windows.Forms.TextBox textBoxDescrReq;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.CheckBox checkBoxAddToExist;
        private System.Windows.Forms.ComboBox comboBoxAllPaths;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.CheckBox checkBoxSavePipeline;
        private System.Windows.Forms.CheckBox checkBoxOnlyCurrent;
        private System.Windows.Forms.Button buttonCheckContent;
        private System.Windows.Forms.CheckBox checkBoxRepeatable;
        private ColumnHeader columnHeader19;
        private ColumnHeader columnHeader20;
    }
}