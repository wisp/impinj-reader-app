namespace SaturnDemo
{
    partial class Stage
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
            this.Pict = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Pict)).BeginInit();
            this.SuspendLayout();
            // 
            // Pict
            // 
            this.Pict.ErrorImage = null;
            this.Pict.InitialImage = null;
            this.Pict.Location = new System.Drawing.Point(78, 376);
            this.Pict.Name = "Pict";
            this.Pict.Size = new System.Drawing.Size(100, 50);
            this.Pict.TabIndex = 3;
            this.Pict.TabStop = false;
            this.Pict.Visible = false;
            // 
            // Stage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 493);
            this.Controls.Add(this.Pict);
            this.Name = "Stage";
            this.Text = "Saturn";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Stage_FormClosed);
            this.Resize += new System.EventHandler(this.Stage_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Stage_MouseUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Stage_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Stage_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Stage_MouseDown);
            this.Load += new System.EventHandler(this.Stage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pict)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PictureBox Pict;
    }
}