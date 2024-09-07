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
            listViewInput = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader5 = new System.Windows.Forms.ColumnHeader();
            columnHeader6 = new System.Windows.Forms.ColumnHeader();
            columnHeader8 = new System.Windows.Forms.ColumnHeader();
            columnHeader9 = new System.Windows.Forms.ColumnHeader();
            columnHeader10 = new System.Windows.Forms.ColumnHeader();
            columnHeader14 = new System.Windows.Forms.ColumnHeader();
            columnHeader15 = new System.Windows.Forms.ColumnHeader();
            columnHeader17 = new System.Windows.Forms.ColumnHeader();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            textBoxAPIPath = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            buttonFormJson = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            listViewOutput = new System.Windows.Forms.ListView();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader7 = new System.Windows.Forms.ColumnHeader();
            columnHeader11 = new System.Windows.Forms.ColumnHeader();
            columnHeader12 = new System.Windows.Forms.ColumnHeader();
            columnHeader13 = new System.Windows.Forms.ColumnHeader();
            columnHeader16 = new System.Windows.Forms.ColumnHeader();
            columnHeader18 = new System.Windows.Forms.ColumnHeader();
            textBoxNewNameInput = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            checkBox1 = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            textBoxNewNameOutput = new System.Windows.Forms.TextBox();
            comboBoxCommand = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            comboBoxMethod = new System.Windows.Forms.ComboBox();
            label9 = new System.Windows.Forms.Label();
            textBoxDescription = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            textBoxSummary = new System.Windows.Forms.TextBox();
            listBoxTags = new System.Windows.Forms.ListBox();
            comboBoxTag = new System.Windows.Forms.ComboBox();
            label11 = new System.Windows.Forms.Label();
            textBoxInputType = new System.Windows.Forms.TextBox();
            textBoxInputFormat = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            textBoxInputExample = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            textBoxOutputExample = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            textBoxOutputFormat = new System.Windows.Forms.TextBox();
            label15 = new System.Windows.Forms.Label();
            textBoxOutputType = new System.Windows.Forms.TextBox();
            label16 = new System.Windows.Forms.Label();
            checkBoxInputRequired = new System.Windows.Forms.CheckBox();
            buttonAddRequest = new System.Windows.Forms.Button();
            textBoxDescrReq = new System.Windows.Forms.TextBox();
            buttonRemove = new System.Windows.Forms.Button();
            checkBoxAddToExist = new System.Windows.Forms.CheckBox();
            comboBoxAllPaths = new System.Windows.Forms.ComboBox();
            label17 = new System.Windows.Forms.Label();
            checkBoxSavePipeline = new System.Windows.Forms.CheckBox();
            checkBoxOnlyCurrent = new System.Windows.Forms.CheckBox();
            buttonCheckContent = new System.Windows.Forms.Button();
            checkBoxRepeatable = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // listViewInput
            // 
            listViewInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listViewInput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader5, columnHeader6, columnHeader8, columnHeader9, columnHeader10, columnHeader14, columnHeader15, columnHeader17 });
            listViewInput.FullRowSelect = true;
            listViewInput.Location = new System.Drawing.Point(0, 290);
            listViewInput.Name = "listViewInput";
            listViewInput.Size = new System.Drawing.Size(1680, 500);
            listViewInput.TabIndex = 0;
            listViewInput.UseCompatibleStateImageBehavior = false;
            listViewInput.View = System.Windows.Forms.View.Details;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(0, 255);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(99, 32);
            label1.TabIndex = 1;
            label1.Text = "Request";
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new System.Drawing.Point(1691, 331);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(151, 46);
            button1.TabIndex = 2;
            button1.Text = "Select";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(179, 32);
            label2.TabIndex = 3;
            label2.Text = "XML Command";
            label2.Click += label2_Click;
            // 
            // textBoxAPIPath
            // 
            textBoxAPIPath.Location = new System.Drawing.Point(1100, 4);
            textBoxAPIPath.Name = "textBoxAPIPath";
            textBoxAPIPath.Size = new System.Drawing.Size(200, 39);
            textBoxAPIPath.TabIndex = 6;
            textBoxAPIPath.TextChanged += textBoxAPIPath_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(981, 4);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(101, 32);
            label3.TabIndex = 5;
            label3.Text = "API Path";
            // 
            // buttonFormJson
            // 
            buttonFormJson.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonFormJson.Location = new System.Drawing.Point(1680, 5);
            buttonFormJson.Name = "buttonFormJson";
            buttonFormJson.Size = new System.Drawing.Size(150, 46);
            buttonFormJson.TabIndex = 7;
            buttonFormJson.Text = "Form JSON";
            buttonFormJson.UseVisualStyleBackColor = true;
            buttonFormJson.Click += buttonFormJson_Click;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button2.Location = new System.Drawing.Point(1691, 857);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(151, 46);
            button2.TabIndex = 10;
            button2.Text = "Select";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(0, 807);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(115, 32);
            label4.TabIndex = 9;
            label4.Text = "Response";
            // 
            // listViewOutput
            // 
            listViewOutput.AllowColumnReorder = true;
            listViewOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listViewOutput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader3, columnHeader4, columnHeader7, columnHeader11, columnHeader12, columnHeader13, columnHeader16, columnHeader18 });
            listViewOutput.FullRowSelect = true;
            listViewOutput.Location = new System.Drawing.Point(0, 855);
            listViewOutput.Name = "listViewOutput";
            listViewOutput.Size = new System.Drawing.Size(1680, 368);
            listViewOutput.TabIndex = 8;
            listViewOutput.UseCompatibleStateImageBehavior = false;
            listViewOutput.View = System.Windows.Forms.View.Details;
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
            // textBoxNewNameInput
            // 
            textBoxNewNameInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxNewNameInput.Location = new System.Drawing.Point(1691, 419);
            textBoxNewNameInput.Name = "textBoxNewNameInput";
            textBoxNewNameInput.Size = new System.Drawing.Size(139, 39);
            textBoxNewNameInput.TabIndex = 11;
            // 
            // label5
            // 
            label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(1691, 382);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(133, 32);
            label5.TabIndex = 12;
            label5.Text = "New Name";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(1691, 467);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(111, 36);
            checkBox1.TabIndex = 13;
            checkBox1.Text = "Param";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(1691, 917);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(133, 32);
            label6.TabIndex = 16;
            label6.Text = "New Name";
            // 
            // textBoxNewNameOutput
            // 
            textBoxNewNameOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxNewNameOutput.Location = new System.Drawing.Point(1691, 952);
            textBoxNewNameOutput.Name = "textBoxNewNameOutput";
            textBoxNewNameOutput.Size = new System.Drawing.Size(139, 39);
            textBoxNewNameOutput.TabIndex = 15;
            // 
            // comboBoxCommand
            // 
            comboBoxCommand.FormattingEnabled = true;
            comboBoxCommand.Location = new System.Drawing.Point(197, 6);
            comboBoxCommand.Name = "comboBoxCommand";
            comboBoxCommand.Size = new System.Drawing.Size(213, 40);
            comboBoxCommand.TabIndex = 17;
            comboBoxCommand.SelectedIndexChanged += comboBoxCommand_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(636, 89);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(0, 32);
            label7.TabIndex = 18;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(21, 53);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(99, 32);
            label8.TabIndex = 19;
            label8.Text = "Method";
            // 
            // comboBoxMethod
            // 
            comboBoxMethod.FormattingEnabled = true;
            comboBoxMethod.Location = new System.Drawing.Point(126, 53);
            comboBoxMethod.Name = "comboBoxMethod";
            comboBoxMethod.Size = new System.Drawing.Size(130, 40);
            comboBoxMethod.TabIndex = 20;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(276, 56);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(135, 32);
            label9.TabIndex = 21;
            label9.Text = "Description";
            label9.Click += label9_Click;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxDescription.Location = new System.Drawing.Point(430, 56);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.Size = new System.Drawing.Size(1400, 92);
            textBoxDescription.TabIndex = 22;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(5, 162);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(115, 32);
            label10.TabIndex = 23;
            label10.Text = "Summary";
            label10.Click += label10_Click;
            // 
            // textBoxSummary
            // 
            textBoxSummary.Location = new System.Drawing.Point(126, 159);
            textBoxSummary.Multiline = true;
            textBoxSummary.Name = "textBoxSummary";
            textBoxSummary.Size = new System.Drawing.Size(618, 78);
            textBoxSummary.TabIndex = 24;
            // 
            // listBoxTags
            // 
            listBoxTags.FormattingEnabled = true;
            listBoxTags.ItemHeight = 32;
            listBoxTags.Location = new System.Drawing.Point(759, 155);
            listBoxTags.Name = "listBoxTags";
            listBoxTags.Size = new System.Drawing.Size(240, 68);
            listBoxTags.TabIndex = 25;
            // 
            // comboBoxTag
            // 
            comboBoxTag.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxTag.FormattingEnabled = true;
            comboBoxTag.Location = new System.Drawing.Point(1041, 154);
            comboBoxTag.Name = "comboBoxTag";
            comboBoxTag.Size = new System.Drawing.Size(789, 40);
            comboBoxTag.TabIndex = 26;
            // 
            // label11
            // 
            label11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(1691, 506);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(65, 32);
            label11.TabIndex = 27;
            label11.Text = "Type";
            label11.Click += label11_Click;
            // 
            // textBoxInputType
            // 
            textBoxInputType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxInputType.Location = new System.Drawing.Point(1691, 541);
            textBoxInputType.Name = "textBoxInputType";
            textBoxInputType.Size = new System.Drawing.Size(151, 39);
            textBoxInputType.TabIndex = 28;
            // 
            // textBoxInputFormat
            // 
            textBoxInputFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxInputFormat.Location = new System.Drawing.Point(1691, 617);
            textBoxInputFormat.Name = "textBoxInputFormat";
            textBoxInputFormat.Size = new System.Drawing.Size(151, 39);
            textBoxInputFormat.TabIndex = 30;
            // 
            // label12
            // 
            label12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(1691, 582);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(89, 32);
            label12.TabIndex = 29;
            label12.Text = "Format";
            // 
            // textBoxInputExample
            // 
            textBoxInputExample.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxInputExample.Location = new System.Drawing.Point(1691, 698);
            textBoxInputExample.Name = "textBoxInputExample";
            textBoxInputExample.Size = new System.Drawing.Size(151, 39);
            textBoxInputExample.TabIndex = 32;
            // 
            // label13
            // 
            label13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(1691, 663);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(103, 32);
            label13.TabIndex = 31;
            label13.Text = "Example";
            // 
            // textBoxOutputExample
            // 
            textBoxOutputExample.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxOutputExample.Location = new System.Drawing.Point(1691, 1191);
            textBoxOutputExample.Name = "textBoxOutputExample";
            textBoxOutputExample.Size = new System.Drawing.Size(151, 39);
            textBoxOutputExample.TabIndex = 38;
            // 
            // label14
            // 
            label14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(1691, 1156);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(103, 32);
            label14.TabIndex = 37;
            label14.Text = "Example";
            // 
            // textBoxOutputFormat
            // 
            textBoxOutputFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxOutputFormat.Location = new System.Drawing.Point(1691, 1110);
            textBoxOutputFormat.Name = "textBoxOutputFormat";
            textBoxOutputFormat.Size = new System.Drawing.Size(151, 39);
            textBoxOutputFormat.TabIndex = 36;
            // 
            // label15
            // 
            label15.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(1691, 1075);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(89, 32);
            label15.TabIndex = 35;
            label15.Text = "Format";
            // 
            // textBoxOutputType
            // 
            textBoxOutputType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            textBoxOutputType.Location = new System.Drawing.Point(1691, 1034);
            textBoxOutputType.Name = "textBoxOutputType";
            textBoxOutputType.Size = new System.Drawing.Size(151, 39);
            textBoxOutputType.TabIndex = 34;
            // 
            // label16
            // 
            label16.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(1691, 999);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(65, 32);
            label16.TabIndex = 33;
            label16.Text = "Type";
            // 
            // checkBoxInputRequired
            // 
            checkBoxInputRequired.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            checkBoxInputRequired.AutoSize = true;
            checkBoxInputRequired.Location = new System.Drawing.Point(1694, 743);
            checkBoxInputRequired.Name = "checkBoxInputRequired";
            checkBoxInputRequired.Size = new System.Drawing.Size(136, 36);
            checkBoxInputRequired.TabIndex = 40;
            checkBoxInputRequired.Text = "required";
            checkBoxInputRequired.UseVisualStyleBackColor = true;
            // 
            // buttonAddRequest
            // 
            buttonAddRequest.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonAddRequest.Location = new System.Drawing.Point(1691, 228);
            buttonAddRequest.Name = "buttonAddRequest";
            buttonAddRequest.Size = new System.Drawing.Size(151, 46);
            buttonAddRequest.TabIndex = 41;
            buttonAddRequest.Text = "Add ";
            buttonAddRequest.UseVisualStyleBackColor = true;
            buttonAddRequest.Click += buttonAddRequest_Click;
            // 
            // textBoxDescrReq
            // 
            textBoxDescrReq.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxDescrReq.Location = new System.Drawing.Point(639, 245);
            textBoxDescrReq.Name = "textBoxDescrReq";
            textBoxDescrReq.Size = new System.Drawing.Size(1041, 39);
            textBoxDescrReq.TabIndex = 42;
            // 
            // buttonRemove
            // 
            buttonRemove.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonRemove.Location = new System.Drawing.Point(1691, 279);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new System.Drawing.Size(151, 46);
            buttonRemove.TabIndex = 43;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddToExist
            // 
            checkBoxAddToExist.AutoSize = true;
            checkBoxAddToExist.Location = new System.Drawing.Point(430, 4);
            checkBoxAddToExist.Name = "checkBoxAddToExist";
            checkBoxAddToExist.Size = new System.Drawing.Size(173, 36);
            checkBoxAddToExist.TabIndex = 44;
            checkBoxAddToExist.Text = "Add to exist";
            checkBoxAddToExist.UseVisualStyleBackColor = true;
            // 
            // comboBoxAllPaths
            // 
            comboBoxAllPaths.FormattingEnabled = true;
            comboBoxAllPaths.Location = new System.Drawing.Point(763, 3);
            comboBoxAllPaths.Name = "comboBoxAllPaths";
            comboBoxAllPaths.Size = new System.Drawing.Size(213, 40);
            comboBoxAllPaths.TabIndex = 46;
            comboBoxAllPaths.SelectedIndexChanged += comboBoxAllPaths_SelectedIndexChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(635, 6);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(104, 32);
            label17.TabIndex = 45;
            label17.Text = "All Paths";
            // 
            // checkBoxSavePipeline
            // 
            checkBoxSavePipeline.AutoSize = true;
            checkBoxSavePipeline.Location = new System.Drawing.Point(1485, 7);
            checkBoxSavePipeline.Name = "checkBoxSavePipeline";
            checkBoxSavePipeline.Size = new System.Drawing.Size(189, 36);
            checkBoxSavePipeline.TabIndex = 47;
            checkBoxSavePipeline.Text = "Save pipeline";
            checkBoxSavePipeline.UseVisualStyleBackColor = true;
            // 
            // checkBoxOnlyCurrent
            // 
            checkBoxOnlyCurrent.AutoSize = true;
            checkBoxOnlyCurrent.Checked = true;
            checkBoxOnlyCurrent.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxOnlyCurrent.Location = new System.Drawing.Point(1309, 7);
            checkBoxOnlyCurrent.Name = "checkBoxOnlyCurrent";
            checkBoxOnlyCurrent.Size = new System.Drawing.Size(140, 36);
            checkBoxOnlyCurrent.TabIndex = 48;
            checkBoxOnlyCurrent.TabStop = false;
            checkBoxOnlyCurrent.Text = "only curr";
            checkBoxOnlyCurrent.UseVisualStyleBackColor = true;
            // 
            // buttonCheckContent
            // 
            buttonCheckContent.Location = new System.Drawing.Point(12, 102);
            buttonCheckContent.Name = "buttonCheckContent";
            buttonCheckContent.Size = new System.Drawing.Size(199, 46);
            buttonCheckContent.TabIndex = 49;
            buttonCheckContent.Text = "CheckContent";
            buttonCheckContent.UseVisualStyleBackColor = true;
            buttonCheckContent.Click += buttonCheckContent_Click;
            // 
            // checkBoxRepeatable
            // 
            checkBoxRepeatable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            checkBoxRepeatable.AutoSize = true;
            checkBoxRepeatable.Location = new System.Drawing.Point(1694, 785);
            checkBoxRepeatable.Name = "checkBoxRepeatable";
            checkBoxRepeatable.Size = new System.Drawing.Size(160, 36);
            checkBoxRepeatable.TabIndex = 50;
            checkBoxRepeatable.Text = "repeteable";
            checkBoxRepeatable.UseVisualStyleBackColor = true;
            // 
            // FormSwaggerFromXML
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1854, 1243);
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
    }
}