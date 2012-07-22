

/***
 *
 * Parts of this work are derived from sample code included in
 * https://developer.impinj.com/modules/PDdownloads/visit.php?cid=6&lid=45
 * and copyright 2007 by Impinj, Inc. That code is licensed under the Apache License, Version 2.0, and available at
 * http://www.apache.org/licenses/LICENSE-2.0
 ***/

/*
Copyright (c) 2009, University of Washington
Copyright (c) 2009, Intel Corporation
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following
conditions are met:
 
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following
disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following
disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the University of Washington nor Intel Corporation nor the names of its contributors may be
used to endorse or promote products derived from this software without specific prior written permission.
 
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


using System;
using System.Collections.Generic;
using System.Text;


using System.Diagnostics;

namespace SaturnDemo
{
    public class Saturn
    {
        private Stage myStage;

        public Saturn()
        {
            myStage = new Stage();
        }
        /// <summary>
        /// Models data on Saturn 
        /// </summary>
        /// <param name="data"></param>
        public void ModelData(double xac, double yac, double zac)
        {
            ReOpenSaturn(); // Create a new Stage if needed

            updatePlanet(xac, yac, zac);
            myStage.Show();     // Show the form
            myStage.Render();   // Render the new values
        }

        public bool IsSaturnOpen()
        {
            return !myStage.IsDisposed;
        }

        ///<summary>
        ///If myStage is disposed, create a new one.
        ///</summary>
        public void ReOpenSaturn()
        {
            if (myStage.IsDisposed) myStage = new Stage();   // create a new reference
        }

        public void DisposeSaturn()
        {
            if (!myStage.IsDisposed)
            {
                myStage.Dispose();
            }
        }

        /// <summary>
        /// Updates the Saturn to the given xac, yac, and zac accelerations
        /// </summary>
        /// <param name="xac"></param>
        /// <param name="yac"></param>
        /// <param name="zac"></param>

        public void updatePlanet(double xAccel, double yAccel, double zAccel)
        {

            // Center values around 0
            xAccel = xAccel - 50;
            yAccel = yAccel - 50;
            zAccel = zAccel - 50;

            // Saturn values
            // Usual old method, pre axis permutation
            if (zAccel != 0.0)
            {
                myStage.senseangleX = (float)((180 / 3.14149) * Math.Atan(xAccel / zAccel));
                myStage.senseangleY = (float)(Math.Sign(zAccel) * (180 / 3.14149) * Math.Atan(yAccel / zAccel));
            }

            myStage.angleX = myStage.senseangleX - myStage.viewangleX;
            myStage.angleY = myStage.senseangleY - myStage.viewangleY;


        }

    }
}
