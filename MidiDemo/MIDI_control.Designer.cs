using Sanford.Multimedia.Midi;

namespace MIDI_Control_Demo
{
    partial class MIDI_control
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
            if(disposing && (components != null))
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
            this.btn_startMIDI = new System.Windows.Forms.Button();
            this.btn_noteOn = new System.Windows.Forms.Button();
            this.btn_noteOff = new System.Windows.Forms.Button();
            this.volumeSelect = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.modSelect = new System.Windows.Forms.NumericUpDown();
            this.pitchBendSelect = new System.Windows.Forms.NumericUpDown();
            this.noteSelect = new System.Windows.Forms.NumericUpDown();
            this.instrumentSelect = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.opt_mode3 = new System.Windows.Forms.RadioButton();
            this.opt_mode2 = new System.Windows.Forms.RadioButton();
            this.opt_mode1 = new System.Windows.Forms.RadioButton();
            this.opt_modeMan = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.openTestPanel = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.volumeSelect)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchBendSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentSelect)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_startMIDI
            // 
            this.btn_startMIDI.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btn_startMIDI.Location = new System.Drawing.Point(12, 12);
            this.btn_startMIDI.Name = "btn_startMIDI";
            this.btn_startMIDI.Size = new System.Drawing.Size(104, 42);
            this.btn_startMIDI.TabIndex = 3;
            this.btn_startMIDI.Text = "Start MIDI";
            this.btn_startMIDI.UseVisualStyleBackColor = false;
            this.btn_startMIDI.Click += new System.EventHandler(this.btn_startMIDI_Click);
            // 
            // btn_noteOn
            // 
            this.btn_noteOn.Location = new System.Drawing.Point(122, 12);
            this.btn_noteOn.Name = "btn_noteOn";
            this.btn_noteOn.Size = new System.Drawing.Size(75, 42);
            this.btn_noteOn.TabIndex = 4;
            this.btn_noteOn.Text = "Note ON";
            this.btn_noteOn.UseVisualStyleBackColor = true;
            this.btn_noteOn.Click += new System.EventHandler(this.btn_noteOn_Click);
            // 
            // btn_noteOff
            // 
            this.btn_noteOff.Location = new System.Drawing.Point(203, 12);
            this.btn_noteOff.Name = "btn_noteOff";
            this.btn_noteOff.Size = new System.Drawing.Size(75, 42);
            this.btn_noteOff.TabIndex = 5;
            this.btn_noteOff.Text = "Note OFF";
            this.btn_noteOff.UseVisualStyleBackColor = true;
            this.btn_noteOff.Click += new System.EventHandler(this.btn_noteOff_Click);
            // 
            // volumeSelect
            // 
            this.volumeSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumeSelect.Location = new System.Drawing.Point(69, 55);
            this.volumeSelect.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.volumeSelect.Name = "volumeSelect";
            this.volumeSelect.Size = new System.Drawing.Size(87, 20);
            this.volumeSelect.TabIndex = 0;
            this.volumeSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.volumeSelect.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Instrument";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.modSelect, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.pitchBendSelect, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.noteSelect, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.instrumentSelect, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.volumeSelect, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 60);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(159, 133);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // modSelect
            // 
            this.modSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modSelect.Location = new System.Drawing.Point(69, 107);
            this.modSelect.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.modSelect.Name = "modSelect";
            this.modSelect.Size = new System.Drawing.Size(87, 20);
            this.modSelect.TabIndex = 7;
            this.modSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pitchBendSelect
            // 
            this.pitchBendSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pitchBendSelect.Location = new System.Drawing.Point(69, 81);
            this.pitchBendSelect.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.pitchBendSelect.Name = "pitchBendSelect";
            this.pitchBendSelect.Size = new System.Drawing.Size(87, 20);
            this.pitchBendSelect.TabIndex = 7;
            this.pitchBendSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.pitchBendSelect.Value = new decimal(new int[] {
            63,
            0,
            0,
            0});
            // 
            // noteSelect
            // 
            this.noteSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noteSelect.Location = new System.Drawing.Point(69, 29);
            this.noteSelect.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.noteSelect.Name = "noteSelect";
            this.noteSelect.Size = new System.Drawing.Size(87, 20);
            this.noteSelect.TabIndex = 7;
            this.noteSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.noteSelect.Value = new decimal(new int[] {
            67,
            0,
            0,
            0});
            // 
            // instrumentSelect
            // 
            this.instrumentSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instrumentSelect.Location = new System.Drawing.Point(69, 3);
            this.instrumentSelect.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.instrumentSelect.Name = "instrumentSelect";
            this.instrumentSelect.Size = new System.Drawing.Size(87, 20);
            this.instrumentSelect.TabIndex = 7;
            this.instrumentSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.instrumentSelect.Value = new decimal(new int[] {
            78,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 29);
            this.label8.TabIndex = 8;
            this.label8.Text = "Modulation";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 26);
            this.label6.TabIndex = 6;
            this.label6.Text = "Pitch Bend";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 26);
            this.label4.TabIndex = 4;
            this.label4.Text = "Volume";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Note Value";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.opt_mode3, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.opt_mode2, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.opt_mode1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.opt_modeMan, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(177, 60);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(101, 127);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Right;
            this.label9.Location = new System.Drawing.Point(19, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 37);
            this.label9.TabIndex = 16;
            this.label9.Text = "Mode 3";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Location = new System.Drawing.Point(19, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 30);
            this.label7.TabIndex = 15;
            this.label7.Text = "Mode 2";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(19, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 30);
            this.label5.TabIndex = 14;
            this.label5.Text = "Mode 1";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opt_mode3
            // 
            this.opt_mode3.AutoSize = true;
            this.opt_mode3.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.opt_mode3.Dock = System.Windows.Forms.DockStyle.Left;
            this.opt_mode3.Location = new System.Drawing.Point(68, 93);
            this.opt_mode3.Name = "opt_mode3";
            this.opt_mode3.Size = new System.Drawing.Size(14, 31);
            this.opt_mode3.TabIndex = 11;
            this.opt_mode3.TabStop = true;
            this.opt_mode3.UseVisualStyleBackColor = true;
            this.opt_mode3.CheckedChanged += new System.EventHandler(this.opt_mode3_CheckedChanged);
            // 
            // opt_mode2
            // 
            this.opt_mode2.AutoSize = true;
            this.opt_mode2.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.opt_mode2.Dock = System.Windows.Forms.DockStyle.Left;
            this.opt_mode2.Location = new System.Drawing.Point(68, 63);
            this.opt_mode2.Name = "opt_mode2";
            this.opt_mode2.Size = new System.Drawing.Size(14, 24);
            this.opt_mode2.TabIndex = 10;
            this.opt_mode2.TabStop = true;
            this.opt_mode2.UseVisualStyleBackColor = true;
            this.opt_mode2.CheckedChanged += new System.EventHandler(this.opt_mode2_CheckedChanged);
            // 
            // opt_mode1
            // 
            this.opt_mode1.AutoSize = true;
            this.opt_mode1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.opt_mode1.Checked = true;
            this.opt_mode1.Dock = System.Windows.Forms.DockStyle.Left;
            this.opt_mode1.Location = new System.Drawing.Point(68, 33);
            this.opt_mode1.Name = "opt_mode1";
            this.opt_mode1.Size = new System.Drawing.Size(14, 24);
            this.opt_mode1.TabIndex = 12;
            this.opt_mode1.TabStop = true;
            this.opt_mode1.UseVisualStyleBackColor = true;
            this.opt_mode1.CheckedChanged += new System.EventHandler(this.opt_mode1_CheckedChanged);
            // 
            // opt_modeMan
            // 
            this.opt_modeMan.AutoSize = true;
            this.opt_modeMan.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.opt_modeMan.Dock = System.Windows.Forms.DockStyle.Left;
            this.opt_modeMan.Location = new System.Drawing.Point(68, 3);
            this.opt_modeMan.Name = "opt_modeMan";
            this.opt_modeMan.Size = new System.Drawing.Size(14, 24);
            this.opt_modeMan.TabIndex = 9;
            this.opt_modeMan.UseVisualStyleBackColor = true;
            this.opt_modeMan.CheckedChanged += new System.EventHandler(this.opt_modeMan_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(20, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 30);
            this.label3.TabIndex = 13;
            this.label3.Text = "Manual";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openTestPanel
            // 
            this.openTestPanel.AutoSize = true;
            this.openTestPanel.Location = new System.Drawing.Point(12, 199);
            this.openTestPanel.Name = "openTestPanel";
            this.openTestPanel.Size = new System.Drawing.Size(77, 17);
            this.openTestPanel.TabIndex = 10;
            this.openTestPanel.Text = "Test Panel";
            this.openTestPanel.UseVisualStyleBackColor = true;
            this.openTestPanel.CheckedChanged += new System.EventHandler(this.openTestPanel_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 236);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(255, 39);
            this.label10.TabIndex = 11;
            this.label10.Text = "Uses the sound card MIDI synthesizer to generate a \r\ntone based on accelerometer " +
    "data. \r\nFirst \"Start MIDI\", then \"Note ON\".";
            // 
            // MIDI_control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(286, 284);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.openTestPanel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btn_noteOff);
            this.Controls.Add(this.btn_noteOn);
            this.Controls.Add(this.btn_startMIDI);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MIDI_control";
            this.Text = "RFID-Vox";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MIDI_control_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.volumeSelect)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchBendSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentSelect)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_startMIDI;
        private System.Windows.Forms.Button btn_noteOn;
        private System.Windows.Forms.Button btn_noteOff;
        private System.Windows.Forms.NumericUpDown volumeSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown pitchBendSelect;
        private System.Windows.Forms.NumericUpDown noteSelect;
        private System.Windows.Forms.NumericUpDown instrumentSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton opt_mode1;
        private System.Windows.Forms.RadioButton opt_mode3;
        private System.Windows.Forms.RadioButton opt_mode2;
        private System.Windows.Forms.RadioButton opt_modeMan;
        private System.Windows.Forms.NumericUpDown modSelect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox openTestPanel;
        private System.Windows.Forms.Label label10;
    }
}

