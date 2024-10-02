namespace WinFormsSettingSender
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            comboBoxChooseSender = new ComboBox();
            buttonNewSender = new Button();
            buttonDelSender = new Button();
            buttonSaveSender = new Button();
            label2 = new Label();
            textBoxUrl = new TextBox();
            label3 = new Label();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            textBoxHeaderName = new TextBox();
            textBoxHeaderValue = new TextBox();
            buttonSaveHeader = new Button();
            buttonDelHeader = new Button();
            buttonNewHeader = new Button();
            label4 = new Label();
            textBoxBody = new TextBox();
            textBoxAnswer = new TextBox();
            button7 = new Button();
            label5 = new Label();
            textBoxContentType = new TextBox();
            label6 = new Label();
            textBoxDescription = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(2, 9);
            label1.Name = "label1";
            label1.Size = new Size(130, 25);
            label1.TabIndex = 0;
            label1.Text = "Choose sender";
            // 
            // comboBoxChooseSender
            // 
            comboBoxChooseSender.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxChooseSender.FormattingEnabled = true;
            comboBoxChooseSender.Location = new Point(138, 6);
            comboBoxChooseSender.Name = "comboBoxChooseSender";
            comboBoxChooseSender.Size = new Size(1185, 33);
            comboBoxChooseSender.TabIndex = 1;
            comboBoxChooseSender.SelectedIndexChanged += comboBoxChooseSender_SelectedIndexChanged_2;
            // 
            // buttonNewSender
            // 
            buttonNewSender.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonNewSender.Location = new Point(1329, 6);
            buttonNewSender.Name = "buttonNewSender";
            buttonNewSender.Size = new Size(64, 34);
            buttonNewSender.TabIndex = 2;
            buttonNewSender.Text = "New";
            buttonNewSender.UseVisualStyleBackColor = true;
            buttonNewSender.Click += buttonNewSender_Click;
            // 
            // buttonDelSender
            // 
            buttonDelSender.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDelSender.Location = new Point(1398, 6);
            buttonDelSender.Name = "buttonDelSender";
            buttonDelSender.Size = new Size(64, 34);
            buttonDelSender.TabIndex = 3;
            buttonDelSender.Text = "Del";
            buttonDelSender.UseVisualStyleBackColor = true;
            buttonDelSender.Click += buttonDelSender_Click;
            // 
            // buttonSaveSender
            // 
            buttonSaveSender.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSaveSender.Location = new Point(1467, 6);
            buttonSaveSender.Name = "buttonSaveSender";
            buttonSaveSender.Size = new Size(64, 34);
            buttonSaveSender.TabIndex = 4;
            buttonSaveSender.Text = "Save";
            buttonSaveSender.UseVisualStyleBackColor = true;
            buttonSaveSender.Click += buttonSaveSender_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 115);
            label2.Name = "label2";
            label2.Size = new Size(34, 25);
            label2.TabIndex = 5;
            label2.Text = "Url";
            // 
            // textBoxUrl
            // 
            textBoxUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxUrl.Location = new Point(138, 115);
            textBoxUrl.Name = "textBoxUrl";
            textBoxUrl.Size = new Size(919, 31);
            textBoxUrl.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 162);
            label3.Name = "label3";
            label3.Size = new Size(77, 25);
            label3.TabIndex = 7;
            label3.Text = "Headers";
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.FullRowSelect = true;
            listView1.Location = new Point(138, 183);
            listView1.Name = "listView1";
            listView1.Size = new Size(1240, 104);
            listView1.TabIndex = 8;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 260;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Value";
            columnHeader2.Width = 560;
            // 
            // textBoxHeaderName
            // 
            textBoxHeaderName.Location = new Point(138, 152);
            textBoxHeaderName.Name = "textBoxHeaderName";
            textBoxHeaderName.Size = new Size(262, 31);
            textBoxHeaderName.TabIndex = 9;
            // 
            // textBoxHeaderValue
            // 
            textBoxHeaderValue.Location = new Point(396, 152);
            textBoxHeaderValue.Name = "textBoxHeaderValue";
            textBoxHeaderValue.Size = new Size(778, 31);
            textBoxHeaderValue.TabIndex = 10;
            // 
            // buttonSaveHeader
            // 
            buttonSaveHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSaveHeader.Location = new Point(1314, 149);
            buttonSaveHeader.Name = "buttonSaveHeader";
            buttonSaveHeader.Size = new Size(64, 34);
            buttonSaveHeader.TabIndex = 13;
            buttonSaveHeader.Text = "Save";
            buttonSaveHeader.UseVisualStyleBackColor = true;
            buttonSaveHeader.Click += buttonSaveHeader_Click;
            // 
            // buttonDelHeader
            // 
            buttonDelHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDelHeader.Location = new Point(1245, 149);
            buttonDelHeader.Name = "buttonDelHeader";
            buttonDelHeader.Size = new Size(64, 34);
            buttonDelHeader.TabIndex = 12;
            buttonDelHeader.Text = "Del";
            buttonDelHeader.UseVisualStyleBackColor = true;
            buttonDelHeader.Click += buttonDelHeader_Click;
            // 
            // buttonNewHeader
            // 
            buttonNewHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonNewHeader.Location = new Point(1176, 149);
            buttonNewHeader.Name = "buttonNewHeader";
            buttonNewHeader.Size = new Size(64, 34);
            buttonNewHeader.TabIndex = 11;
            buttonNewHeader.Text = "New";
            buttonNewHeader.UseVisualStyleBackColor = true;
            buttonNewHeader.Click += buttonNewHeader_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 339);
            label4.Name = "label4";
            label4.Size = new Size(49, 25);
            label4.TabIndex = 14;
            label4.Text = "Data";
            // 
            // textBoxBody
            // 
            textBoxBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxBody.Location = new Point(138, 332);
            textBoxBody.Multiline = true;
            textBoxBody.Name = "textBoxBody";
            textBoxBody.ScrollBars = ScrollBars.Vertical;
            textBoxBody.Size = new Size(1387, 276);
            textBoxBody.TabIndex = 15;
            // 
            // textBoxAnswer
            // 
            textBoxAnswer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            textBoxAnswer.Location = new Point(138, 609);
            textBoxAnswer.Multiline = true;
            textBoxAnswer.Name = "textBoxAnswer";
            textBoxAnswer.ScrollBars = ScrollBars.Vertical;
            textBoxAnswer.Size = new Size(1387, 133);
            textBoxAnswer.TabIndex = 16;
            // 
            // button7
            // 
            button7.Location = new Point(12, 367);
            button7.Name = "button7";
            button7.Size = new Size(112, 34);
            button7.TabIndex = 17;
            button7.Text = "Test";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 292);
            label5.Name = "label5";
            label5.Size = new Size(112, 25);
            label5.TabIndex = 18;
            label5.Text = "ContentType";
            // 
            // textBoxContentType
            // 
            textBoxContentType.Location = new Point(138, 292);
            textBoxContentType.Name = "textBoxContentType";
            textBoxContentType.Size = new Size(176, 31);
            textBoxContentType.TabIndex = 19;
            textBoxContentType.Text = "application/json";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(4, 48);
            label6.Name = "label6";
            label6.Size = new Size(102, 25);
            label6.TabIndex = 20;
            label6.Text = "Description";
            // 
            // textBoxDescription
            // 
            textBoxDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxDescription.Location = new Point(138, 41);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.Size = new Size(1387, 68);
            textBoxDescription.TabIndex = 21;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1537, 743);
            Controls.Add(textBoxDescription);
            Controls.Add(label6);
            Controls.Add(textBoxContentType);
            Controls.Add(label5);
            Controls.Add(button7);
            Controls.Add(textBoxAnswer);
            Controls.Add(textBoxBody);
            Controls.Add(label4);
            Controls.Add(buttonSaveHeader);
            Controls.Add(buttonDelHeader);
            Controls.Add(buttonNewHeader);
            Controls.Add(textBoxHeaderValue);
            Controls.Add(textBoxHeaderName);
            Controls.Add(listView1);
            Controls.Add(label3);
            Controls.Add(textBoxUrl);
            Controls.Add(label2);
            Controls.Add(buttonSaveSender);
            Controls.Add(buttonDelSender);
            Controls.Add(buttonNewSender);
            Controls.Add(comboBoxChooseSender);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Senders collection ";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox comboBoxChooseSender;
        private Button buttonNewSender;
        private Button buttonDelSender;
        private Button buttonSaveSender;
        private Label label2;
        private TextBox textBoxUrl;
        private Label label3;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TextBox textBoxHeaderName;
        private TextBox textBoxHeaderValue;
        private Button buttonSaveHeader;
        private Button buttonDelHeader;
        private Button buttonNewHeader;
        private Label label4;
        private TextBox textBoxBody;
        private TextBox textBoxAnswer;
        private Button button7;
        private Label label5;
        private TextBox textBoxContentType;
        private Label label6;
        private TextBox textBoxDescription;
    }
}
