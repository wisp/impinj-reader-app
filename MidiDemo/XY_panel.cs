using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MIDI_Control_Demo
{
    public partial class XY_panel : Form
    {
        bool mouseIsPressed = false;
        MIDI_control callMeBack;

        public XY_panel(MIDI_control myParent)
        {
            InitializeComponent();
            callMeBack = myParent;
        }


        private void XY_panel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseIsPressed = true;
        }
        
        private void XY_panel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsPressed = false;
        }

        private void XY_panel_MouseMove(object sender, MouseEventArgs e)
        {
            mouseEventHappened();
        }

        private void XY_panel_MouseClick(object sender, MouseEventArgs e)
        {
            mouseEventHappened();
        }

        private void mouseEventHappened()
        {
            if (!mouseIsPressed)
                return;

            System.Drawing.Point mousePosition = Cursor.Position;

            // Make this a relative position
            mousePosition = this.PointToClient(mousePosition);

            int mouseX = mousePosition.X;
            int mouseY = mousePosition.Y;

            int scaledX = (mouseX * 128) / this.Width;
            int scaledY = (mouseY * 128) / this.Height;

            callMeBack.updateAccelValues((double)scaledX, (double)scaledY, 0);
        }

        private void XY_panel_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            callMeBack.testPanelHidden();
            e.Cancel = true;
        }

  

    }
}
