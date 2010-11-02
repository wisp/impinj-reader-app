namespace AttenuatorTest
{
    partial class AttenTestForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAttenTestCreateFiles = new System.Windows.Forms.CheckBox();
            this.label67 = new System.Windows.Forms.Label();
            this.txtAttenTestRunID = new System.Windows.Forms.TextBox();
            this.chkAttenTestWriteDebuggingFile = new System.Windows.Forms.CheckBox();
            this.label47 = new System.Windows.Forms.Label();
            this.txtAttenStepOffTime = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label44 = new System.Windows.Forms.Label();
            this.txtAttenTestDuration = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.txtAttenTestNumTags = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.txtAttenTestAttempts = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.txtAttenStepOnTime = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAttenTestStatus = new System.Windows.Forms.TextBox();
            this.btnAttenTestAbort = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAttnTestStatus = new System.Windows.Forms.TextBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.graphAttenTest = new ZedGraph.ZedGraphControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.timerAttenuatorTest = new System.Windows.Forms.Timer(this.components);
            this.btnReaderSettings = new System.Windows.Forms.Button();
            this.btnAttenSettings = new System.Windows.Forms.Button();
            this.progressAttenTest = new System.Windows.Forms.ProgressBar();
            this.btnLoggingSettings = new System.Windows.Forms.Button();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.timerUpdateGui = new System.Windows.Forms.Timer(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTagsPerSecond = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAttenTestCreateFiles);
            this.groupBox1.Controls.Add(this.label67);
            this.groupBox1.Controls.Add(this.txtAttenTestRunID);
            this.groupBox1.Controls.Add(this.chkAttenTestWriteDebuggingFile);
            this.groupBox1.Controls.Add(this.label47);
            this.groupBox1.Controls.Add(this.txtAttenStepOffTime);
            this.groupBox1.Controls.Add(this.label46);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.txtAttenTestDuration);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.txtAttenTestNumTags);
            this.groupBox1.Controls.Add(this.label42);
            this.groupBox1.Controls.Add(this.txtAttenTestAttempts);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.txtAttenStepOnTime);
            this.groupBox1.Location = new System.Drawing.Point(15, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 144);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // chkAttenTestCreateFiles
            // 
            this.chkAttenTestCreateFiles.AutoSize = true;
            this.chkAttenTestCreateFiles.Location = new System.Drawing.Point(190, 71);
            this.chkAttenTestCreateFiles.Name = "chkAttenTestCreateFiles";
            this.chkAttenTestCreateFiles.Size = new System.Drawing.Size(101, 17);
            this.chkAttenTestCreateFiles.TabIndex = 84;
            this.chkAttenTestCreateFiles.Text = "Create files now";
            this.chkAttenTestCreateFiles.UseVisualStyleBackColor = true;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(201, 48);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(41, 13);
            this.label67.TabIndex = 83;
            this.label67.Text = "Run ID";
            // 
            // txtAttenTestRunID
            // 
            this.txtAttenTestRunID.Location = new System.Drawing.Point(249, 45);
            this.txtAttenTestRunID.Name = "txtAttenTestRunID";
            this.txtAttenTestRunID.Size = new System.Drawing.Size(66, 20);
            this.txtAttenTestRunID.TabIndex = 82;
            this.txtAttenTestRunID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkAttenTestWriteDebuggingFile
            // 
            this.chkAttenTestWriteDebuggingFile.AutoSize = true;
            this.chkAttenTestWriteDebuggingFile.Checked = true;
            this.chkAttenTestWriteDebuggingFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttenTestWriteDebuggingFile.Location = new System.Drawing.Point(190, 94);
            this.chkAttenTestWriteDebuggingFile.Name = "chkAttenTestWriteDebuggingFile";
            this.chkAttenTestWriteDebuggingFile.Size = new System.Drawing.Size(125, 17);
            this.chkAttenTestWriteDebuggingFile.TabIndex = 81;
            this.chkAttenTestWriteDebuggingFile.Text = "Write Debugging File";
            this.chkAttenTestWriteDebuggingFile.UseVisualStyleBackColor = true;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(222, 22);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(21, 13);
            this.label47.TabIndex = 79;
            this.label47.Text = "Off";
            // 
            // txtAttenStepOffTime
            // 
            this.txtAttenStepOffTime.Location = new System.Drawing.Point(249, 19);
            this.txtAttenStepOffTime.Name = "txtAttenStepOffTime";
            this.txtAttenStepOffTime.Size = new System.Drawing.Size(66, 20);
            this.txtAttenStepOffTime.TabIndex = 78;
            this.txtAttenStepOffTime.Text = "10";
            this.txtAttenStepOffTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(169, 22);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(21, 13);
            this.label46.TabIndex = 77;
            this.label46.Text = "On";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(35, 120);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 76;
            this.checkBox1.Text = "Issue Read Cmd";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(19, 97);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(72, 13);
            this.label44.TabIndex = 75;
            this.label44.Text = "Duration (ms):";
            // 
            // txtAttenTestDuration
            // 
            this.txtAttenTestDuration.Location = new System.Drawing.Point(97, 94);
            this.txtAttenTestDuration.Name = "txtAttenTestDuration";
            this.txtAttenTestDuration.Size = new System.Drawing.Size(66, 20);
            this.txtAttenTestDuration.TabIndex = 74;
            this.txtAttenTestDuration.Text = "100";
            this.txtAttenTestDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(32, 71);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(59, 13);
            this.label43.TabIndex = 73;
            this.label43.Text = "Num Tags:";
            // 
            // txtAttenTestNumTags
            // 
            this.txtAttenTestNumTags.Location = new System.Drawing.Point(97, 68);
            this.txtAttenTestNumTags.Name = "txtAttenTestNumTags";
            this.txtAttenTestNumTags.Size = new System.Drawing.Size(66, 20);
            this.txtAttenTestNumTags.TabIndex = 72;
            this.txtAttenTestNumTags.Text = "0";
            this.txtAttenTestNumTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(40, 48);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(51, 13);
            this.label42.TabIndex = 71;
            this.label42.Text = "Attempts:";
            // 
            // txtAttenTestAttempts
            // 
            this.txtAttenTestAttempts.Location = new System.Drawing.Point(97, 45);
            this.txtAttenTestAttempts.Name = "txtAttenTestAttempts";
            this.txtAttenTestAttempts.Size = new System.Drawing.Size(66, 20);
            this.txtAttenTestAttempts.TabIndex = 70;
            this.txtAttenTestAttempts.Text = "0";
            this.txtAttenTestAttempts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(32, 22);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(58, 13);
            this.label41.TabIndex = 69;
            this.label41.Text = "Step Time:";
            // 
            // txtAttenStepOnTime
            // 
            this.txtAttenStepOnTime.Location = new System.Drawing.Point(97, 19);
            this.txtAttenStepOnTime.Name = "txtAttenStepOnTime";
            this.txtAttenStepOnTime.Size = new System.Drawing.Size(66, 20);
            this.txtAttenStepOnTime.TabIndex = 68;
            this.txtAttenStepOnTime.Text = "30";
            this.txtAttenStepOnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(23, 191);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 58;
            this.label14.Text = "Test Status:";
            // 
            // txtAttenTestStatus
            // 
            this.txtAttenTestStatus.Enabled = false;
            this.txtAttenTestStatus.Location = new System.Drawing.Point(93, 188);
            this.txtAttenTestStatus.Name = "txtAttenTestStatus";
            this.txtAttenTestStatus.Size = new System.Drawing.Size(268, 20);
            this.txtAttenTestStatus.TabIndex = 57;
            // 
            // btnAttenTestAbort
            // 
            this.btnAttenTestAbort.Location = new System.Drawing.Point(293, 8);
            this.btnAttenTestAbort.Name = "btnAttenTestAbort";
            this.btnAttenTestAbort.Size = new System.Drawing.Size(68, 23);
            this.btnAttenTestAbort.TabIndex = 56;
            this.btnAttenTestAbort.Text = "Abort";
            this.btnAttenTestAbort.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(11, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(174, 16);
            this.label11.TabIndex = 1;
            this.label11.Text = "WISP Read Rate Tester";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(48, 475);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 74;
            this.label8.Text = "Test Status:";
            // 
            // txtAttnTestStatus
            // 
            this.txtAttnTestStatus.Enabled = false;
            this.txtAttnTestStatus.Location = new System.Drawing.Point(120, 517);
            this.txtAttnTestStatus.Name = "txtAttnTestStatus";
            this.txtAttnTestStatus.Size = new System.Drawing.Size(268, 20);
            this.txtAttnTestStatus.TabIndex = 73;
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(121, 133);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(68, 40);
            this.btnAbort.TabIndex = 72;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // graphAttenTest
            // 
            this.graphAttenTest.Location = new System.Drawing.Point(12, 224);
            this.graphAttenTest.Name = "graphAttenTest";
            this.graphAttenTest.ScrollGrace = 0;
            this.graphAttenTest.ScrollMaxX = 0;
            this.graphAttenTest.ScrollMaxY = 0;
            this.graphAttenTest.ScrollMaxY2 = 0;
            this.graphAttenTest.ScrollMinX = 0;
            this.graphAttenTest.ScrollMinY = 0;
            this.graphAttenTest.ScrollMinY2 = 0;
            this.graphAttenTest.Size = new System.Drawing.Size(431, 287);
            this.graphAttenTest.TabIndex = 75;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblMode);
            this.groupBox3.Controls.Add(this.lblStatus);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtIPAddress);
            this.groupBox3.Controls.Add(this.btnConnect);
            this.groupBox3.Location = new System.Drawing.Point(47, 62);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(357, 65);
            this.groupBox3.TabIndex = 76;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Connection ";
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(244, 47);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(34, 13);
            this.lblMode.TabIndex = 20;
            this.lblMode.Text = "Mode";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(7, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 18;
            this.lblStatus.Text = "Status";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Reader Address:";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(98, 19);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(152, 20);
            this.txtIPAddress.TabIndex = 1;
            this.txtIPAddress.Text = "speedway-10-1B-21";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(256, 16);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(92, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // timerAttenuatorTest
            // 
            this.timerAttenuatorTest.Tick += new System.EventHandler(this.timerAttenuatorTest_Tick);
            // 
            // btnReaderSettings
            // 
            this.btnReaderSettings.Location = new System.Drawing.Point(195, 134);
            this.btnReaderSettings.Name = "btnReaderSettings";
            this.btnReaderSettings.Size = new System.Drawing.Size(68, 39);
            this.btnReaderSettings.TabIndex = 77;
            this.btnReaderSettings.Text = "Reader Settings";
            this.btnReaderSettings.UseVisualStyleBackColor = true;
            this.btnReaderSettings.Click += new System.EventHandler(this.btnReaderSettings_Click);
            // 
            // btnAttenSettings
            // 
            this.btnAttenSettings.Location = new System.Drawing.Point(269, 134);
            this.btnAttenSettings.Name = "btnAttenSettings";
            this.btnAttenSettings.Size = new System.Drawing.Size(68, 39);
            this.btnAttenSettings.TabIndex = 78;
            this.btnAttenSettings.Text = "Atten Settings";
            this.btnAttenSettings.UseVisualStyleBackColor = true;
            this.btnAttenSettings.Click += new System.EventHandler(this.btnAttenSettings_Click);
            // 
            // progressAttenTest
            // 
            this.progressAttenTest.Location = new System.Drawing.Point(12, 543);
            this.progressAttenTest.Maximum = 1000;
            this.progressAttenTest.Name = "progressAttenTest";
            this.progressAttenTest.Size = new System.Drawing.Size(431, 20);
            this.progressAttenTest.Step = 1000;
            this.progressAttenTest.TabIndex = 80;
            // 
            // btnLoggingSettings
            // 
            this.btnLoggingSettings.Location = new System.Drawing.Point(343, 134);
            this.btnLoggingSettings.Name = "btnLoggingSettings";
            this.btnLoggingSettings.Size = new System.Drawing.Size(68, 39);
            this.btnLoggingSettings.TabIndex = 81;
            this.btnLoggingSettings.Text = "Logging Settings";
            this.btnLoggingSettings.UseVisualStyleBackColor = true;
            this.btnLoggingSettings.Click += new System.EventHandler(this.btnLoggingSettings_Click);
            // 
            // timerProgress
            // 
            this.timerProgress.Enabled = true;
            this.timerProgress.Interval = 1000;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // timerUpdateGui
            // 
            this.timerUpdateGui.Enabled = true;
            this.timerUpdateGui.Interval = 250;
            this.timerUpdateGui.Tick += new System.EventHandler(this.timerUpdateGui_Tick_1);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(44, 133);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(68, 40);
            this.btnStart.TabIndex = 82;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 25);
            this.label2.TabIndex = 83;
            this.label2.Text = "Total Tag Rate : ";
            // 
            // lblTagsPerSecond
            // 
            this.lblTagsPerSecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagsPerSecond.Location = new System.Drawing.Point(171, 185);
            this.lblTagsPerSecond.Name = "lblTagsPerSecond";
            this.lblTagsPerSecond.Size = new System.Drawing.Size(67, 25);
            this.lblTagsPerSecond.TabIndex = 84;
            this.lblTagsPerSecond.Text = "0";
            this.lblTagsPerSecond.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 38);
            this.label1.TabIndex = 85;
            this.label1.Text = "Attenuator Test App";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            this.label36.Font = new System.Drawing.Font("Georgia", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(282, 20);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(122, 31);
            this.label36.TabIndex = 86;
            this.label36.Text = "Version 3.0";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AttenTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 577);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTagsPerSecond);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnLoggingSettings);
            this.Controls.Add(this.btnAttenSettings);
            this.Controls.Add(this.btnReaderSettings);
            this.Controls.Add(this.progressAttenTest);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.graphAttenTest);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtAttnTestStatus);
            this.Controls.Add(this.btnAbort);
            this.Name = "AttenTestForm";
            this.Text = "WISP Read Rate Tester";
            this.Load += new System.EventHandler(this.AttenTestForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AttenTestForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAttenTestCreateFiles;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.TextBox txtAttenTestRunID;
        private System.Windows.Forms.CheckBox chkAttenTestWriteDebuggingFile;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtAttenStepOffTime;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox txtAttenTestDuration;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox txtAttenTestNumTags;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txtAttenTestAttempts;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox txtAttenStepOnTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAttenTestStatus;
        private System.Windows.Forms.Button btnAttenTestAbort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAttnTestStatus;
        private System.Windows.Forms.Button btnAbort;
        private ZedGraph.ZedGraphControl graphAttenTest;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Timer timerAttenuatorTest;
        private System.Windows.Forms.Button btnReaderSettings;
        private System.Windows.Forms.Button btnAttenSettings;
        private System.Windows.Forms.ProgressBar progressAttenTest;
        private System.Windows.Forms.Button btnLoggingSettings;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Timer timerUpdateGui;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTagsPerSecond;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label36;
    }
}

