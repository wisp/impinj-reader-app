

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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Runtime.InteropServices;

using System.IO;
namespace SaturnDemo
{
    internal static class LoadTexture
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGBTRIPLE
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public int biStructure;
            public int biWidth;
            public int biHeight;
            public int biWidthBytes;
            public short biPlanes;
            public short biBitsPixel;
            public IntPtr biBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public RGBTRIPLE[] bmColors;
        }

        public const int DIB_RGB_COLORS = 0;

        [System.Runtime.InteropServices.DllImport("gdi32", EntryPoint = "GetDIBits", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern int GetDIBits(IntPtr aHDC, IntPtr hBitmap, int nStartScan, int nNumScans, IntPtr lpBits, ref BITMAPINFO lpBI, int wUsage);

        private static PictureBox pb = new PictureBox();


        public static void loadBMP(string Filename, PictureBox Pict)
        {
            string path = Filename;
            // Verify that the path name exits
            if (!File.Exists(Filename))
            {
                MessageBox.Show("Unable to locate bitmap. Check file path", "LLRP Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)(Image.FromFile(Filename));
            int X, Y;

            BITMAPINFO bi24BitInfo = new BITMAPINFO();
            //Getting data from PictureBox directly
            bi24BitInfo.bmiHeader.biBitCount = 24;
            bi24BitInfo.bmiHeader.biCompression = 0; // BI_RGB
            bi24BitInfo.bmiHeader.biPlanes = 1;
            bi24BitInfo.bmiHeader.biSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bi24BitInfo.bmiHeader.biWidth = bmp.Width;
            bi24BitInfo.bmiHeader.biHeight = bmp.Height;

            // Flattened it. Multidimension won't work here.
            byte[] textureImg = new byte[bmp.Width * bmp.Height * 3];
            // Pin the array in memory, the garbage collector won't play with it.
            GCHandle gch = GCHandle.Alloc(textureImg, GCHandleType.Pinned);
            // Send it off.
            int result = GetDIBits(Pict.CreateGraphics().GetHdc(), bmp.GetHbitmap(), 0, bmp.Height, gch.AddrOfPinnedObject(), ref bi24BitInfo, 0);
            gch.Free();

            //Swap BGR->RGB
            for (Y = 0; Y < bmp.Height; Y++)
            {
                for (X = 0; X < bmp.Width; X++)
                {
                    int index = (Y * (bmp.Width * 3)) + (X * 3);
                    byte temp = textureImg[index];
                    textureImg[index] = textureImg[index + 2];
                    textureImg[index + 2] = temp;
                }
            }

            // Er...
            // Now what. We didn't change anything.
            // We have to get the pixels back into an image now.
            //Dim bmp2 As New System.Drawing.Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat) ' otherwise it would be 32bppARGB
            //Dim ms As New IO.MemoryStream
            //' Save to the stream.
            //bmp2.Save(ms, ImageFormat.Bmp)
            //' Overwrite with the new pixels
            //Dim bw As New IO.BinaryWriter(ms)
            //' Seek to the start of the pixeldata
            //bw.BaseStream.Seek(&H36, IO.SeekOrigin.Begin)
            //' write the bytes
            //bw.Write(textureImg)
            //bw.Flush()
            //bmp2.Dispose()
            //bmp2 = New System.Drawing.Bitmap(ms)
            //bw.Close() ' also disposes of ms and bw
            //Pict.Image = bmp2

            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 3, bmp.Width, bmp.Height, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, textureImg);

            textureImg = null;

        }

        public static void loadRings()
        {
            int ringsWidth = 0;
            //System.IO.FileInfo FileProps = new System.IO.FileInfo("C:\\Documents and Settings\\rprasad\\My Documents\\Visual Studio 2005\\Projects\\WISP Demo Application\\rings.dat");
            System.IO.FileInfo FileProps = new System.IO.FileInfo("rings.dat");
            //System.IO.BinaryReader reader = new System.IO.BinaryReader(System.IO.File.OpenRead("rings.dat"));

            System.IO.BinaryReader reader = null;
            try
            {
                reader = new System.IO.BinaryReader(System.IO.File.OpenRead("rings.dat"));
            }
            catch (Exception e)
            {
                Trace.WriteLine("couldn't find graphics files");
                MessageBox.Show("Couldn't find rings.dat or saturn.jpg", "Saturn Stage",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

            if (reader == null) return;
            

            //Open App.Path & "\rings.dat" For Binary As #1
            ringsWidth = (int)FileProps.Length / 2; //half of data is alpha info

            // ReDim rings((ringsWidth*2)-1) As Byte

            byte[] rings = new byte[(ringsWidth * 2)];
            // Get #1, , rings()
            rings = reader.ReadBytes((ringsWidth * 2) - 1);
            reader.Close();

            Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, ringsWidth, 1, Gl.GL_LUMINANCE_ALPHA, Gl.GL_UNSIGNED_BYTE, rings);
        }
    }
}
