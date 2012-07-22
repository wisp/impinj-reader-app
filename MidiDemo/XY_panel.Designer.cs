namespace MIDI_Control_Demo
{
    partial class XY_panel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XY_panel));
            this.SuspendLayout();
            // 
            // XY_panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "XY_panel";
            this.Text = "XY_panel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XY_panel_FormClosing);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.XY_panel_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.XY_panel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.XY_panel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.XY_panel_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

    }
}