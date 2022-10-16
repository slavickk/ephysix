namespace WinFormsETLPackagedCreator
{
    partial class FormLinkExplorer
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
            this.tableViewControl1 = new WinFormsApp1.TableViewControl();
            this.SuspendLayout();
            // 
            // tableViewControl1
            // 
            this.tableViewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableViewControl1.AutoSize = true;
            this.tableViewControl1.Location = new System.Drawing.Point(166, 142);
            this.tableViewControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tableViewControl1.Name = "tableViewControl1";
            this.tableViewControl1.Size = new System.Drawing.Size(381, 342);
            this.tableViewControl1.TabIndex = 15;
            // 
            // FormLinkExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableViewControl1);
            this.Name = "FormLinkExplorer";
            this.Text = "FormLinkExplorer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WinFormsApp1.TableViewControl tableViewControl1;
    }
}