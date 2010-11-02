namespace AttenuatorTest
{
    partial class EditAttenSettings
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtAttnStepOffTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAttnStepOnTime = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSettingsDefault = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSettingsApply = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "Off";
            // 
            // txtAttnStepOffTime
            // 
            this.txtAttnStepOffTime.Location = new System.Drawing.Point(249, 19);
            this.txtAttnStepOffTime.Name = "txtAttnStepOffTime";
            this.txtAttnStepOffTime.Size = new System.Drawing.Size(66, 20);
            this.txtAttnStepOffTime.TabIndex = 78;
            this.txtAttnStepOffTime.Text = "10";
            this.txtAttnStepOffTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 77;
            this.label3.Text = "On";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox3.Location = new System.Drawing.Point(130, 103);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(104, 17);
            this.checkBox3.TabIndex = 76;
            this.checkBox3.Text = "Issue Read Cmd";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 69;
            this.label7.Text = "Step Time:";
            // 
            // txtAttnStepOnTime
            // 
            this.txtAttnStepOnTime.Location = new System.Drawing.Point(97, 19);
            this.txtAttnStepOnTime.Name = "txtAttnStepOnTime";
            this.txtAttnStepOnTime.Size = new System.Drawing.Size(66, 20);
            this.txtAttnStepOnTime.TabIndex = 68;
            this.txtAttnStepOnTime.Text = "30";
            this.txtAttnStepOnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAttnStepOffTime);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtAttnStepOnTime);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 127);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btnSettingsDefault
            // 
            this.btnSettingsDefault.Location = new System.Drawing.Point(255, 162);
            this.btnSettingsDefault.Name = "btnSettingsDefault";
            this.btnSettingsDefault.Size = new System.Drawing.Size(89, 22);
            this.btnSettingsDefault.TabIndex = 119;
            this.btnSettingsDefault.Text = "Set To Default";
            this.btnSettingsDefault.UseVisualStyleBackColor = true;
            this.btnSettingsDefault.Click += new System.EventHandler(this.btnSettingsDefault_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(142, 162);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 22);
            this.btnCancel.TabIndex = 120;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSettingsApply
            // 
            this.btnSettingsApply.Location = new System.Drawing.Point(26, 162);
            this.btnSettingsApply.Name = "btnSettingsApply";
            this.btnSettingsApply.Size = new System.Drawing.Size(89, 22);
            this.btnSettingsApply.TabIndex = 118;
            this.btnSettingsApply.Text = "OK";
            this.btnSettingsApply.UseVisualStyleBackColor = true;
            this.btnSettingsApply.Click += new System.EventHandler(this.btnSettingsApply_Click);
            // 
            // EditAttenSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 211);
            this.Controls.Add(this.btnSettingsDefault);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSettingsApply);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditAttenSettings";
            this.Text = "EditAttenSettings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAttnStepOffTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAttnStepOnTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSettingsDefault;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSettingsApply;
    }
}