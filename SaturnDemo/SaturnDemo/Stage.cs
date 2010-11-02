using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

using System.Diagnostics;

namespace SaturnDemo
{
    public partial class Stage : Form
    {
        //Author        : Jan Tosovsky
        //Email         : j.tosovsky@tiscali.cz
        //Website       : http://nio.astronomy.cz/vb/opengl.html
        //Date          : 28 November 2004
        //Version       : 1.0
        //Description   : Saturn

        //Rings data were derived from images at:
        //http://www.mmedia.is/~bjj/data/saturn/rings.html

        //Program requires Gl Type Library from Patrice Scribe
        //http://is6.pacific.net.hk/~edx/tlb.htm
        // now: http://home.pacific.net.hk/~edx/tlb.htm   --- djy
        //With library you needn't declare any used Gl functions or constants
        //Copy library to system directory and then register it:
        //regsvr32 "C:\Windows\System\vbogl.tlb" where path may vary
        //In Project>References... in VB menu check item VB Gl API 1.2 (ANSI) 

        #region Structs
        // Struts for the componets of the Saturn drawing
        public struct glVertex
        {
            public float U;
            public float v;
        }

        public struct glCoord
        {
            public float X;
            public float Y;
            public float z;
        }

        #endregion

        #region InstaceVariables

        // instaces variables
        private bool stageLoaded = false;

        // Defines viewer's position
        public float defviewangleX;
        public float defviewangleY;
        public float viewangleX;
        public float viewangleY;

        public float clutchangleX;
        public float clutchangleY;
        public float angleX;

        // Displayed value
        public float angleY;
        public float senseangleX;

        // "raw" sensor angle
        public float senseangleY;

        private int startX;
        private int startY;
        private bool moving;
        private Glu.GLUquadric quadObj = new Glu.GLUquadric();
        private glCoord[] DiskVertex;
        private int[] Texture = new int[2];

        #endregion

      


        public Stage()
        {
            InitializeComponent();
        }



        private void Stage_Load(object sender, EventArgs e)
        {
            // Enable GL
            do
            {
                //InitGL.EnableOpenGL(this.CreateGraphics.GetHdc);
                InitGL.EnableOpenGL(this.CreateGraphics().GetHdc());
            } while (!InitGL.IsEnable());

            //Initial rotation
            defviewangleX = 0;
            //defviewangleY = 70;
            defviewangleY = 90;

            //viewangleX = 20 ' very old
            viewangleX = defviewangleX;
            viewangleY = defviewangleY;
            angleX = 0 - viewangleX;
            angleY = 0 - viewangleY;
            
            DrawInit();

            stageLoaded = true;


            Stage_Resize(null, null);

        }

        public bool Loaded()
        {
            return stageLoaded;
        }

        public float GetViewAngelX()
        {
            return viewangleX;
        }

        public float GetViewAngelY()
        {
            return viewangleY;
        }

        public bool IsLoaded()
        {
            return stageLoaded;
        }



        private void Stage_FormClosed(object sender, FormClosedEventArgs e)
        {
            stageLoaded = false;
            if (InitGL.IsEnable())
            {
                InitGL.DisableOpenGL();
            }

        }



        private void DrawInit()
        {
            //Uncoment next procedure to take material properties in account

            SetMaterial();
            //only for demostration of possibilities, not very accurate

            SetLight();

            //Gl.glViewport(0, 0, this.ClientSize.Width, this.ClientSize.Height);

            Gl.glClearColor(0, 0, 0, 0);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            Gl.glGenTextures(2, Texture);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture[0]);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            LoadTexture.loadBMP("saturn.jpg", this.Pict);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture[1]);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            LoadTexture.loadRings();
            CalculateDisk();
            quadObj = Glu.gluNewQuadric();
            Glu.gluQuadricTexture(quadObj, Gl.GL_TRUE);


        }



        public float GetAngleX()
        {
            return angleX;
        }

        public float GetAngleY()
        {
            return angleY;
        }


        public void Render()
        {
            if (stageLoaded)  // If the stage has been loaded
            {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glLoadIdentity();

                Gl.glPushMatrix();
                // Put something in here to translate object in place (so it still rotates about its center)
                //glTranslatef hpfx, hpfy, hpfz

                //rotate
                Gl.glRotatef(angleY, 1, 0, 0);
                Gl.glRotatef(angleX, 0, 1, 0);
                //draw planet
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture[0]);
                Gl.glColor3f(1, 1, 1);

                Glu.gluSphere(quadObj, 0.601, 40, 40);
                //draw rings
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, Texture[1]);
                Gl.glColor3f((float)1.0, (float)0.88, (float)0.82);
                DrawDisk();
                Gl.glPopMatrix();

                System.Windows.Forms.Application.DoEvents();
                //Gdi.SwapBuffers(this.CreateGraphics.GetHdc);
                try
                {
                    Gdi.SwapBuffers((IntPtr)this.CreateGraphics().GetHdc());
                }
                catch (Exception e)
                {
                    Trace.WriteLine("gdi error");
                }
            }
        }

        private void Stage_MouseDown(object sender, MouseEventArgs e)
        {
            glCoord glCo = new glCoord();
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                moving = true;
                startX = (int)glCo.X;
                startY = (int)glCo.Y;
            }
        }

        private void Stage_MouseMove(object sender, MouseEventArgs e)
        {
            glCoord glCo = new glCoord();
            if (moving)
            {
                angleX = angleX + (glCo.X - startX);
                angleY = angleY + (glCo.Y - startY);
                startX = (int)glCo.X;
                startY = (int)glCo.Y;
                Render();
            }
        }

        private void SetLight()
        {
            float[] AmbientLight = new float[4];
            float[] DiffuseLight = new float[4];
            float[] LightPos = new float[4];
            float[] SpotDirection = new float[4];

            AmbientLight[0] = 1;
            AmbientLight[1] = (float)0.8;
            AmbientLight[2] = (float)0.8;
            AmbientLight[3] = 1;

            DiffuseLight[0] = 1;
            DiffuseLight[1] = (float)0.8;
            DiffuseLight[2] = (float)0.8;
            DiffuseLight[3] = 1;

            LightPos[0] = 0;
            LightPos[1] = 0;
            LightPos[2] = 50;
            LightPos[3] = 0;

            SpotDirection[0] = 0;
            SpotDirection[1] = -2;
            SpotDirection[2] = -1;
            SpotDirection[3] = 1;

            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_TRUE);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, LightPos);

            //glLightfv GL_LIGHT0, GL_SPOT_DIRECTION, SpotDirection(0)
            //glLightfv GL_LIGHT0, GL_SPOT_CUTOFF, 60#
            //glLightfv GL_LIGHT0, GL_SPOT_EXPONENT, 10
            //glLightfv GL_LIGHT0, GL_EMISSION, AmbientLight(0)
            //glLightfv GL_LIGHT0, GL_AMBIENT, AmbientLight(0)

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, DiffuseLight);

            Gl.glEnable(Gl.GL_LIGHTING);
        }

        private void SetMaterial()
        {
            float[] mat_specular = new float[4];
            float[] mat_shininess = new float[1];
            float[] mat_difuse = new float[4];
            float[] mat_ambient = new float[4];

            mat_specular[0] = 0;
            mat_specular[1] = 0;
            mat_specular[2] = 1;
            mat_specular[3] = 1;

            mat_difuse[0] = 0;
            mat_difuse[1] = 0;
            mat_difuse[2] = (float)0.7;
            mat_difuse[3] = 1;

            mat_ambient[0] = 0;
            mat_ambient[1] = 0;
            mat_ambient[2] = 1;
            mat_ambient[3] = 1;

            mat_shininess[0] = 5;

            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_difuse);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient);

            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            //glColorMaterial GL_FRONT, GL_AMBIENT_AND_DIFFUSE

        }


        private void CalculateDisk()
        {
            long angle;
            double radius1;
            double radius2;
            int i;
            double rads;

            DiskVertex = new glCoord[180];

            rads = Math.Atan(1) / 45;
            radius1 = 0.744;
            radius2 = 1.402;

            //radius1 = 0.744 / 2;
            //radius2 = 1.402 / 2;
            i = 0;

            for (angle = 0; angle <= 360; angle += 8)
            {
                DiskVertex[i].X = (float)(radius1 * Math.Sin(rads * (angle - 90)));
                //x position
                DiskVertex[i].Y = (float)(radius1 * Math.Sin(rads * angle));
                //y position
                i++;

                //I = I + 1;
                DiskVertex[i].X = (float)(radius2 * Math.Sin(rads * (angle - 90)));
                DiskVertex[i].Y = (float)(radius2 * Math.Sin(rads * angle));

                //i= i + 1;
                i++;
            }
        }

        public void DrawDisk()
        {
            //int i;
            //int arrayLength = DiskVertex.Length;
            int arrayRank = DiskVertex.Rank;
            //int max = DiskVertex.GetUpperBound(arrayRank - 1);
            //int value = Microsoft.VisualBasic.Information.UBound(DiskVertex, arrayRank);

            for (int i = 0; i <= DiskVertex.Length - 3; i += 2)
            //for (int i = 0; i <= value - 3; i += 2)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
                //1st triange
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3f(DiskVertex[i].X, DiskVertex[i].Y, 0);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3f(DiskVertex[i + 1].X, DiskVertex[i + 1].Y, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3f(DiskVertex[i + 2].X, DiskVertex[i + 2].Y, 0);

                //2nd triangle
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3f(DiskVertex[i + 3].X, DiskVertex[i + 3].Y, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3f(DiskVertex[i + 2].X, DiskVertex[i + 2].Y, 0);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3f(DiskVertex[i + 1].X, DiskVertex[i + 1].Y, 0);
                Gl.glEnd();
            }
        }

        private void Stage_Resize(object sender, EventArgs e)
        {
            int w;
            int h;
            //if (FormWindowState.Minimized) return; // TODO: might not be correct. Was : Exit Sub

            if (this.WindowState == System.Windows.Forms.FormWindowState.Maximized)
            {
                return;
            }
            w = this.ClientSize.Width;
            h = this.ClientSize.Height;
            Gl.glViewport(0, 0, w, h);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            if (w <= h)
            {
                Gl.glOrtho(-1.5, 1.5, -1.5 * h / w, 1.5 * h / w, -10.0, 10.0);
            }
            else
            {
                Gl.glOrtho(-1.5 * w / h, 1.5 * w / h, -1.5, 1.5, -10.0, 10.0);
            }

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            System.Windows.Forms.Application.DoEvents();
            Render();
        }

        private void Stage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Glu.gluDeleteQuadric(quadObj);
            InitGL.DisableOpenGL();
            //Debug.Print("stage unload.");
            //MainFrm.CheckSaturn() = false;
            stageLoaded = false;
        }

        private void Stage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                moving = false;
            }
        }

    }
}