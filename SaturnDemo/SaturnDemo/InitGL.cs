

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
using Tao.Platform.Windows;
using System.Runtime.InteropServices;

namespace SaturnDemo
{
    static class InitGL
    {// graphic pointer
        static IntPtr hRC;
        // states whether the CL has been enabled
        static bool enable = false;

        [DllImport("kernel32", SetLastError = true, EntryPoint = "RtlZeroMemory")]
        public static extern void ZeroMemory(Gdi.PIXELFORMATDESCRIPTOR ptr, int numBytes);

        public static void EnableOpenGL(IntPtr ghDC)
        {
            int PixFormat;   // pixed format

            // Pixed descriptor 
            Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();

            // Set memory 
            //ZeroMemory(pfd, Microsoft.VisualBasic.Strings.Len(pfd));

            // Pixel descriptor configuration
            pfd.nSize = (short)Microsoft.VisualBasic.Strings.Len(pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
            pfd.iPixelType = Gdi.PFD_TYPE_RGBA;
            pfd.cColorBits = 24;
            pfd.cDepthBits = 32;
            pfd.cAlphaBits = 24;
            pfd.iLayerType = Gdi.PFD_MAIN_PLANE;

            // Chose pixel format
            PixFormat = Gdi.ChoosePixelFormat((IntPtr)ghDC, ref pfd);

            // If the operation failed let user know
            if (PixFormat == 0)
            {
                //MessageBox.Show("Can't create OpenGL context!", "Saturn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                enable = false;
                return;
            }
            if (!Gdi.SetPixelFormat((IntPtr)ghDC, PixFormat, ref pfd))
            {
                //MessageBox.Show("Unable to set pixel format", "Saturn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                enable = false;
                return;
            }

            // Set graphic context
            hRC = Wgl.wglCreateContext(ghDC);

            // If the operation failed let user know
            if (hRC.ToInt32() == 0)
            {
                //MessageBox.Show("Unable to get rendering context", "Saturn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                enable = false;
                return;
            }
            if (!(Wgl.wglMakeCurrent(ghDC, hRC)))
            {
                //MessageBox.Show("Unable to make rendering context current", "Saturn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                enable = false;
                return;
            }
            enable = true;   // GL has been enabled

        }

        /// <summary>
        /// Return true if the GL has been enabled 
        /// </summary>
        /// <returns></returns>
        public static bool IsEnable()
        {
            return enable;
        }

        /// <summary>
        /// Disables the GL
        /// </summary>
        public static void DisableOpenGL()
        {
            Wgl.wglMakeCurrent((IntPtr)0, (IntPtr)0);
            Wgl.wglDeleteContext((IntPtr)hRC);
        }
    }
}
