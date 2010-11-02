namespace Logging
{
    partial class EditLogForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOpenLogDialog = new System.Windows.Forms.Button();
            this.txtLogFileName = new System.Windows.Forms.TextBox();
            this.btnLogOptApply = new System.Windows.Forms.Button();
            this.lstOptions = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLogName = new System.Windows.Forms.TextBox();
            this.chkEnableLog = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOpenLogDialog);
            this.groupBox2.Controls.Add(this.txtLogFileName);
            this.groupBox2.Location = new System.Drawing.Point(12, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Name:";
            // 
            // btnOpenLogDialog
            // 
            this.btnOpenLogDialog.Location = new System.Drawing.Point(191, 19);
            this.btnOpenLogDialog.Name = "btnOpenLogDialog";
            this.btnOpenLogDialog.Size = new System.Drawing.Size(67, 24);
            this.btnOpenLogDialog.TabIndex = 52;
            this.btnOpenLogDialog.Text = "Save As";
            this.btnOpenLogDialog.UseVisualStyleBackColor = true;
            this.btnOpenLogDialog.Click += new System.EventHandler(this.btnOpenLogDialog_Click);
            // 
            // txtLogFileName
            // 
            this.txtLogFileName.Location = new System.Drawing.Point(6, 19);
            this.txtLogFileName.Name = "txtLogFileName";
            this.txtLogFileName.Size = new System.Drawing.Size(179, 20);
            this.txtLogFileName.TabIndex = 50;
            // 
            // btnLogOptApply
            // 
            this.btnLogOptApply.Location = new System.Drawing.Point(67, 349);
            this.btnLogOptApply.Name = "btnLogOptApply";
            this.btnLogOptApply.Size = new System.Drawing.Size(75, 23);
            this.btnLogOptApply.TabIndex = 2;
            this.btnLogOptApply.Text = "OK";
            this.btnLogOptApply.UseVisualStyleBackColor = true;
            this.btnLogOptApply.Click += new System.EventHandler(this.btnLogOptApply_Click);
            // 
            // lstOptions
            // 
            this.lstOptions.FormattingEnabled = true;
            this.lstOptions.Location = new System.Drawing.Point(55, 170);
            this.lstOptions.Name = "lstOptions";
            this.lstOptions.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstOptions.Size = new System.Drawing.Size(183, 173);
            this.lstOptions.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(148, 349);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Select The Things You Want To Log:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLogName);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 54);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log Name:";
            // 
            // txtLogName
            // 
            this.txtLogName.Location = new System.Drawing.Point(6, 19);
            this.txtLogName.Name = "txtLogName";
            this.txtLogName.Size = new System.Drawing.Size(252, 20);
            this.txtLogName.TabIndex = 50;
            // 
            // chkEnableLog
            // 
            this.chkEnableLog.AutoSize = true;
            this.chkEnableLog.Location = new System.Drawing.Point(98, 12);
            this.chkEnableLog.Name = "chkEnableLog";
            this.chkEnableLog.Size = new System.Drawing.Size(80, 17);
            this.chkEnableLog.TabIndex = 51;
            this.chkEnableLog.Text = "Enable Log";
            this.chkEnableLog.UseVisualStyleBackColor = true;
            // 
            // EditLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 384);
            this.Controls.Add(this.chkEnableLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lstOptions);
            this.Controls.Add(this.btnLogOptApply);
            this.Controls.Add(this.groupBox2);
            this.Name = "EditLogForm";
            this.Text = "LoggingEditOptions";
            this.Load += new System.EventHandler(this.EditLogForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOpenLogDialog;
        private System.Windows.Forms.TextBox txtLogFileName;
        private System.Windows.Forms.Button btnLogOptApply;
        private System.Windows.Forms.ListBox lstOptions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLogName;
        private System.Windows.Forms.CheckBox chkEnableLog;
    }
}