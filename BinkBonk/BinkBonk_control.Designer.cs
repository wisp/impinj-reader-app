using Sanford.Multimedia.Midi;

namespace BinkBonk  

{
    partial class BinkBonk_control
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.EPC_A = new System.Windows.Forms.TextBox();
            this.EPC_B = new System.Windows.Forms.TextBox();
            this.learn_A = new System.Windows.Forms.RadioButton();
            this.learn_B = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.instrumentSelectA = new System.Windows.Forms.NumericUpDown();
            this.noteSelectA = new System.Windows.Forms.NumericUpDown();
            this.instrumentSelectB = new System.Windows.Forms.NumericUpDown();
            this.noteSelectB = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentSelectA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteSelectA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentSelectB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteSelectB)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_startMIDI
            // 
            this.btn_startMIDI.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btn_startMIDI.Location = new System.Drawing.Point(12, 11);
            this.btn_startMIDI.Name = "btn_startMIDI";
            this.btn_startMIDI.Size = new System.Drawing.Size(104, 121);
            this.btn_startMIDI.TabIndex = 3;
            this.btn_startMIDI.Text = "Start MIDI";
            this.btn_startMIDI.UseVisualStyleBackColor = false;
            this.btn_startMIDI.Click += new System.EventHandler(this.btn_startMIDI_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.EPC_A, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.EPC_B, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.learn_A, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.learn_B, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 152);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(312, 66);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "EPC";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "A";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "B";
            // 
            // EPC_A
            // 
            this.EPC_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EPC_A.Location = new System.Drawing.Point(24, 16);
            this.EPC_A.Name = "EPC_A";
            this.EPC_A.Size = new System.Drawing.Size(240, 20);
            this.EPC_A.TabIndex = 5;
            // 
            // EPC_B
            // 
            this.EPC_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EPC_B.Location = new System.Drawing.Point(24, 42);
            this.EPC_B.Name = "EPC_B";
            this.EPC_B.Size = new System.Drawing.Size(240, 20);
            this.EPC_B.TabIndex = 6;
            // 
            // learn_A
            // 
            this.learn_A.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.learn_A.AutoSize = true;
            this.learn_A.Location = new System.Drawing.Point(282, 19);
            this.learn_A.Name = "learn_A";
            this.learn_A.Size = new System.Drawing.Size(14, 13);
            this.learn_A.TabIndex = 9;
            this.learn_A.TabStop = true;
            this.learn_A.UseVisualStyleBackColor = true;
            // 
            // learn_B
            // 
            this.learn_B.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.learn_B.AutoSize = true;
            this.learn_B.Location = new System.Drawing.Point(282, 46);
            this.learn_B.Name = "learn_B";
            this.learn_B.Size = new System.Drawing.Size(14, 13);
            this.learn_B.TabIndex = 10;
            this.learn_B.TabStop = true;
            this.learn_B.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(270, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Learn";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.instrumentSelectA, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.noteSelectA, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.instrumentSelectB, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.noteSelectB, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(122, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(202, 120);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Instrument B";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Note Value A";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Instrument A";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // instrumentSelectA
            // 
            this.instrumentSelectA.Location = new System.Drawing.Point(104, 3);
            this.instrumentSelectA.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.instrumentSelectA.Name = "instrumentSelectA";
            this.instrumentSelectA.Size = new System.Drawing.Size(95, 20);
            this.instrumentSelectA.TabIndex = 4;
            this.instrumentSelectA.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // noteSelectA
            // 
            this.noteSelectA.Location = new System.Drawing.Point(104, 33);
            this.noteSelectA.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.noteSelectA.Name = "noteSelectA";
            this.noteSelectA.Size = new System.Drawing.Size(95, 20);
            this.noteSelectA.TabIndex = 5;
            this.noteSelectA.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // instrumentSelectB
            // 
            this.instrumentSelectB.Location = new System.Drawing.Point(104, 63);
            this.instrumentSelectB.Name = "instrumentSelectB";
            this.instrumentSelectB.Size = new System.Drawing.Size(95, 20);
            this.instrumentSelectB.TabIndex = 6;
            this.instrumentSelectB.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // noteSelectB
            // 
            this.noteSelectB.Location = new System.Drawing.Point(104, 93);
            this.noteSelectB.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.noteSelectB.Name = "noteSelectB";
            this.noteSelectB.Size = new System.Drawing.Size(95, 20);
            this.noteSelectB.TabIndex = 6;
            this.noteSelectB.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Note Value B";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 232);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(295, 26);
            this.label9.TabIndex = 6;
            this.label9.Text = "Plays one of two MIDI tones when A and B EPC is detected. \r\nDesigned for use with" +
    " the alpha-WISP.";
            // 
            // BinkBonk_control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(341, 270);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btn_startMIDI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "BinkBonk_control";
            this.Text = "Bink-Bonk Demo";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BinkBonk_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentSelectA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteSelectA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentSelectB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noteSelectB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_startMIDI;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown instrumentSelectA;
        private System.Windows.Forms.NumericUpDown noteSelectA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown noteSelectB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox EPC_A;
        private System.Windows.Forms.TextBox EPC_B;
        private System.Windows.Forms.RadioButton learn_A;
        private System.Windows.Forms.RadioButton learn_B;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown instrumentSelectB;
        private System.Windows.Forms.Label label9;
    }
}

