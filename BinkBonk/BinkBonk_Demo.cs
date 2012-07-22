using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinkBonk
{
    public class BinkBonk_Demo
    {
        BinkBonk_control myBinkBonkControl;

        public BinkBonk_Demo()
        {
            myBinkBonkControl = new BinkBonk_control();
        }

        public bool isBinkBonkOpen()
        {
            return !myBinkBonkControl.IsDisposed;
        }

        public void reOpenBinkBonk()
        {
            if (myBinkBonkControl.IsDisposed)
            {
                myBinkBonkControl = new BinkBonk_control();
            }

            myBinkBonkControl.Show();

        }

        public void disposeBinkBonk()
        {
            if (!myBinkBonkControl.IsDisposed)
            {
                myBinkBonkControl.Close();
            }

        }

        public void handleEPC(String EPC)
        {
            myBinkBonkControl.handleEPC(EPC);
        }


    }
}
