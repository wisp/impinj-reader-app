namespace Logging
{
    partial class LoggingForm
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
            this.btnLogApply = new System.Windows.Forms.Button();
            this.cmbLog = new System.Windows.Forms.ComboBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogApply
            // 
            this.btnLogApply.Location = new System.Drawing.Point(119, 64);
            this.btnLogApply.Name = "btnLogApply";
            this.btnLogApply.Size = new System.Drawing.Size(77, 26);
            this.btnLogApply.TabIndex = 66;
            this.btnLogApply.Text = "OK";
            this.btnLogApply.UseVisualStyleBackColor = true;
            this.btnLogApply.Click += new System.EventHandler(this.btnLogApply_Click);
            // 
            // cmbLog
            // 
            this.cmbLog.FormattingEnabled = true;
            this.cmbLog.Location = new System.Drawing.Point(13, 23);
            this.cmbLog.Name = "cmbLog";
            this.cmbLog.Size = new System.Drawing.Size(130, 21);
            this.cmbLog.TabIndex = 67;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(149, 21);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 68;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(230, 21);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(75, 23);
            this.btnAddNew.TabIndex = 69;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // LoggingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 102);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.cmbLog);
            this.Controls.Add(this.btnLogApply);
            this.Name = "LoggingForm";
            this.Text = "Logging";
            this.Load += new System.EventHandler(this.LoggingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogApply;
        private System.Windows.Forms.ComboBox cmbLog;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAddNew;


    }
}