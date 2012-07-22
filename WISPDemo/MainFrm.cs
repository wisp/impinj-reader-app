/***
 *
 * Parts of this work are derived from sample code included in
 * https://developer.impinj.com/modules/PDdownloads/visit.php?cid=6&lid=45
 * and copyright 2007 by Impinj, Inc. That code is licensed under the Apache License, Version 2.0, and available at
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Parts of this work use ZedGraph:
 * ZedGraph is licensed under the Lesser or Library General Public License.
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

/*
 ***************************************************************************
 * Theory of Operation:
 * 
 * Sorry, this is a bit out of date. Check the wiki for more current documentation.
 * 
 * Reader runs autonomously, generates a report every 100ms.
 * 
 * RFIDReader.UpdateROReport(...) thread parses the reports and 
 *    calls MainFrm.HandleTagReceived(...) with each tag's information, 
 *    encapsulated in a MyTag object. 
 * MainFrm.HandleTagReceived does sensor data extraction, etc., but doesn't touch GUI!
 * 
 * timerUpdateGUI runs at 10 to 20 hz, and puts data from HandleTagReceived onto the GUI.
 * 
 ***************************************************************************
 */

/*
 ***************************************************************************
 * Operating Modes:
 * 
 * App has various modes (defined in enum GuiModes):
 * a. Idle  (reader disconnected)
 * b. Ready (reader connected, not running)
 * 1. User Inventory
 * 
 * 
 * 
 ***************************************************************************
 */

/*
 ***************************************************************************
 * Adding a new sensor demo:
 * 
 * Sorry, this is a bit out of date. Check the wiki for more current documentation.
 * 
 * First, provide parsing and identifier information to the MyTag class
 * 
 * Second, generate a new handler method in this class. Ex, see HandleAccelTagStats
 * 
 * Third, edit the HandleTagReceived function to call your handler method upon seeing your sensor
 * 
 * Fourth, edit timerUpdateGUI to display your data on the GUI.
 * 
 * Do not add GUI code to the handler method, this can cause instability on some computers.
 * 
 ***************************************************************************
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;

using System.Diagnostics;

using ZedGraph;

using SaturnDemo;
using MIDI_Control_Demo;
using ReaderLibrary;
using Logging;
using BinkBonk;

//using AttenuatorTest;

namespace WISPDemo
{
    public partial class MainFrm : Form, ReaderLibrary.IRFIDGUI
    {
        private SaturnDemo.Saturn saturn;
        private MIDI_Control_Demo.Midi midi;
        private BinkBonk.BinkBonk_Demo binkBonk;
        //private RFIDReader reader;
        private ReaderManager readerMgr;
        private TagStats stats;
        private WispHandleTags handleTags;

        // Keep the default size information so we know by how much to stretch
        // the tag list data grid view
        private int formInitialHeight;
        private int pnlTagListInitialHeight;

        private Form setting;
        private Form loggingForm;

        private LoggingManager log;


        public MainFrm()
        {
            InitializeComponent();

        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

            // reader is our handle to the physical reader.
            //reader = new RFIDReader(this);
            handleTags = new WispHandleTags();
            readerMgr = new ReaderManager(this, handleTags);

            log = new WispLoggingManager();

            // init the saturn object.
            saturn = new SaturnDemo.Saturn();

            // Init midi config
            midi = new MIDI_Control_Demo.Midi();

            // Init bink bonk
            binkBonk = new BinkBonk.BinkBonk_Demo();
            handleTags.setBinkBonkCallback(binkBonk);

            // Setup axis labels for the various graphs.
            InitSOCGraph();
            InitTempGraph();

            // Other init code
            InitTagStats();

            // Grab initial component sizes for easy resize
            formInitialHeight = this.Height;
            pnlTagListInitialHeight = dgvTagStats.Height;

            // Init GUI operational mode to idle (disconnected)
            SetMode(ReaderManager.GuiModes.Idle);
        }

        private string debugTextBoxContent = "";
        /// <summary>
        /// Appends text to the "LLRP Messages" text box
        /// </summary>
        /// <param name="appendToTop">Text to append</param>
        public void AppendToDebugTextBox(string appendToTop)
        {
            debugTextBoxContent = appendToTop + "\r\n" + debugTextBoxContent.Substring(0, Math.Min(debugTextBoxContent.Length, 2000));
        }

        private string tagTextBoxContent = "";
        /// <summary>
        /// Appends text to the running Tag List text box
        /// </summary>
        /// <param name="appendToTop">Text to append</param>
        public void AppendToMainTextBox(string appendToTop)
        {
            tagTextBoxContent = appendToTop + "\r\n" + tagTextBoxContent.Substring(0, Math.Min(tagTextBoxContent.Length, 2000));
        }


        //Handle went here! - ?

        private void ClearGUIAll()
        {
            tagTextBoxContent = "";
            debugTextBoxContent = "";
            // Set the default values for max and min
            ClearMaxMin();
            ClearSOC();
            stats.Clear();
            ClearHandlerTimes();
            ClearSOC();

        }


        #region Accelerometer

        private double xMax = 0;
        private double xMin = 100;

        private double yMax = 0;
        private double yMin = 100;

        private double zMax = 0;
        private double zMin = 100;




        public void ClearMaxMin()
        {
            xMax = 0;
            xMin = 1024;
            yMax = 0;
            yMin = 1024;
            zMax = 0;
            zMin = 1024;
        }


        private void UpdateGraphicsOnSaturn()
        {
            double xac, yac, zac;
            xac = handleTags.GetCurrentX();
            yac = handleTags.GetCurrentY();
            zac = handleTags.GetCurrentZ();

            if (chkFlipX.Checked) xac = 100 - xac;
            if (chkFlipY.Checked) yac = 100 - yac;

            saturn.ModelData(xac, yac, zac);
        }

        private void UpdateMidiGui()
        {
            double xac, yac, zac;
            xac = handleTags.GetCurrentX();
            yac = handleTags.GetCurrentY();
            zac = handleTags.GetCurrentZ();

            if (chkFlipX.Checked) xac = 100 - xac;
            if (chkFlipY.Checked) yac = 100 - yac;

            this.midi.ReOpenMidiConfig();
            this.midi.updateMidi(xac, yac, zac);
        }

        private void UpdateAccelerometerGUI()
        {
            // actually do the work of updating the accelerometer gui elements
            UpdateTiltBarsAndLabels(handleTags.GetCurrentX(), handleTags.GetCurrentY(), handleTags.GetCurrentZ());

            GetMaxMinValues();  // accel min max

            // Update saturn
            if (chkSaturn.Checked)
            {
                UpdateGraphicsOnSaturn();
            }
            else
            {
                this.saturn.DisposeSaturn();
            }
            saturnFrames++;

            // Update MIDI out if checked
            if (chkMIDI.Checked)
            {
                UpdateMidiGui();
                
            }
            
        }


        private void GetMaxMinValues()
        {
            lblXMax.Text = (Math.Round(xMax, 3)).ToString();
            lblXMin.Text = (Math.Round(xMin, 3)).ToString();
            lblDx.Text = (Math.Round(handleTags.GetDeltaX(), 3)).ToString();
            lblYMax.Text = (Math.Round(yMax, 3)).ToString();
            lblYMin.Text = (Math.Round(yMin, 3)).ToString();
            lblDy.Text = (Math.Round(handleTags.GetDeltaY(), 3)).ToString();
            lblZMax.Text = (Math.Round(zMax, 3)).ToString();
            lblZMin.Text = (Math.Round(zMin, 3)).ToString();
            lblDz.Text = (Math.Round(handleTags.GetDeltaZ(), 3)).ToString();
        }

        private void UpdateTiltBarsAndLabels(double xac, double yac, double zac)
        {
            double senseangleX = 0, senseangleY = 0;
            if (zac != 0.0)
            {
                senseangleX = (float)((180 / 3.14149) * Math.Atan(xac / zac));
                senseangleY = (float)(Math.Sign(zac) * (180 / 3.14149) * Math.Atan(yac / zac));
            }

            // Update Tilt bars and labels in GUI
            if ((xac < 101) && (yac < 101) && (zac < 101))
            {
                if (xac > 0)
                {
                    tbarX.Value = (int)xac;
                    lblTiltX.Text = (Math.Round(xac, 3)).ToString();
                }
                if (yac > 0)
                {
                    tbarY.Value = (int)yac;
                    lblTiltY.Text = (Math.Round(yac, 3)).ToString();
                }
                if (zac > 0)
                {
                    tbarZ.Value = (int)zac;
                    lblTiltZ.Text = (Math.Round(zac, 3)).ToString();
                }
            }
        }

        private void btnCalAccel_Click(object sender, EventArgs e)
        {
            // Fix x
            double xcorr = MyTag.GetAccelCorrection("x");
            xcorr = 50.0 / (handleTags.GetCurrentX() / xcorr);
            Trace.WriteLine("New X correction Factor: " + xcorr.ToString());
            // Fix y
            double ycorr = MyTag.GetAccelCorrection("y");
            ycorr = 50.0 / (handleTags.GetCurrentY() / ycorr);
            Trace.WriteLine("New Y correction Factor: " + ycorr.ToString());
            // Fix z
            double zcorr = MyTag.GetAccelCorrection("z");
            zcorr = 41.0 / (handleTags.GetCurrentZ() / zcorr);
            Trace.WriteLine("New Z correction Factor: " + zcorr.ToString());
            MyTag.SetAccelCorrection(xcorr, ycorr, zcorr);
        }


        private int saturnFrames = 0;




        #endregion Accelerometer Variables




        #region SOC Region


        private void ClearSOC()
        {
            handleTags.ClearSOC();
            updateSOCGraph();
        }

        private void InitSOCGraph()
        {
            // setup the axis titles, etc.
            // get a reference to the GraphPane
            GraphPane myPane = graphSOC.GraphPane;

            // Set the Titles
            myPane.Title.Text = "SOC WISP Data";
            myPane.XAxis.Title.Text = "Sample";
            myPane.YAxis.Title.Text = "ADC Output";

            // Generate the line
            LineItem myCurve = myPane.AddCurve(null,
                  handleTags.GetSOCData(), Color.Red, SymbolType.Diamond);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            graphSOC.AxisChange();

        }

        private void updateSOCGraph()
        {
            // Tell ZedGraph to refigure the
            // axes since the data have changed

            PointPairList socData = handleTags.GetSOCData();

            lock (socData)
            {
                graphSOC.AxisChange();

                // don't let graph zoom too close.
                tempPane = graphSOC.GraphPane;
                //if (tempPane.YAxis.Scale.Max - tempPane.YAxis.Scale.Min < 6)
                //{
                //    tempPane.YAxis.Scale.Max += 3;
                //    tempPane.YAxis.Scale.Min -= 3;
                //}

                graphSOC.Refresh();
            }
        }

        private void UpdateSOCGUI()
        {
            updateSOCGraph();

            lblSocADC.Text = "ADC = " + Math.Round(handleTags.GetSocFilteredValue(), 2);
            lblSocTemperature.Text = handleTags.GetSocTemperature() + " C";
            lblSocLowPassFilter.Text = "Filter: " + Math.Round(100.0 * tbarSocFilter.Value / tbarSocFilter.Maximum, 2) + " percent of new value";
        }


        #endregion SOC Region




        #region Temperature



        private void ClearTemperature()
        {
            handleTags.ClearTemperature();
        }

        GraphPane tempPane;
        private void InitTempGraph()
        {
            // setup the axis titles, etc.
            // get a reference to the GraphPane
            GraphPane tempPane = graphTemperature.GraphPane;

            // Set the Titles
            tempPane.Title.Text = "Temperature Data";
            tempPane.XAxis.Title.Text = "Sample Number";
            tempPane.YAxis.Title.Text = "Temperature (Celsius)";

            // Generate the line
            LineItem myCurve = tempPane.AddCurve(null,
                  handleTags.GetTemperatureData(), Color.Red, SymbolType.Diamond);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            graphTemperature.AxisChange();
        }


        // Update temperature-related stuff on the gui.
        private void UpdateTemperatureGUI()
        {
            if (handleTags.GetHasNewTempData())
            {
                handleTags.ClearHasNewTempData();
                lblTemperature.Text = handleTags.GetTemperatureCelsius().ToString("###.0") + " C";
                lblTemperatureSource.Text = handleTags.GetTemperatureSource();

                handleTags.GetTemperatureData().Add(handleTags.GetTemperatureDataCount(), handleTags.GetTemperatureCelsius());
                if (handleTags.GetTemperatureData().Count > 500)
                    handleTags.GetTemperatureData().RemoveAt(0);

                graphTemperature.AxisChange();
                tempPane = graphTemperature.GraphPane;

                if (tempPane.YAxis.Scale.Min > 10)
                    tempPane.YAxis.Scale.Min = 10;
                if (tempPane.YAxis.Scale.Max < 40)
                    tempPane.YAxis.Scale.Max = 40;

                graphTemperature.Refresh();
            }
            handleTags.incrementTemperatureDataCount();
        }


        #endregion


        #region Other Sensors And Statistics


        private void InitTagStats()
        {
            stats = new TagStats(dgvTagStats);
        }


        private void ProcessTagStats()
        {

            // get the list of new tags
            ArrayList newTags = handleTags.GetNewTags();

            lock (newTags)
            {

                stats.AddTags(newTags);

                log.WriteToLog(newTags);

                newTags.Clear();

            }

        }


        static string currTime = ((DateTime.Now.ToString()).Replace(":", ".")).Replace("/", "-");



        #endregion



        #region GUI Update

        public Accel accelInfo = new Accel();

        Stopwatch swGUI = new Stopwatch();
        long peakGuiTime = 0;
        long peakHandlerTime = 0;

        public struct Accel
        {
            public bool filterChkd;
            public double alpha;
        }

        public Accel getAccelInfo()
        {
            return accelInfo;
        }

        private void timerUpdateGUI_Tick(object sender, EventArgs e)
        {
            timerUpdateGUI.Enabled = false; // we don't want timer to start this thread twice

            swGUI.Reset(); // some timers to keep track of how long gui updates are taking.
            swGUI.Start();

            // update this timer's refresh rate from text box
            int newInterval = Convert.ToInt16(Convert.ToDouble(txtSaturnRefreshMs.Text));
            if (newInterval > 10000 || newInterval < 1) newInterval = 100;
            timerUpdateGUI.Interval = newInterval;

            // Tell the Tag Handler what's on the GUI
            SetHandleTagSettings();

            // Update Text Boxes
            txtBoxTags.Text = tagTextBoxContent;
            txtDebugMessages.Text = debugTextBoxContent;

            // Update Saturn checkbox
            if (!saturn.IsSaturnOpen())
            {
                chkSaturn.Checked = false;
            }

            // Update MIDI checkbox
            if (!midi.IsMidiConfigOpen())
            {
                chkMIDI.Checked = false;
            }

            // Update Bink Bonk checkbox
            if (!binkBonk.isBinkBonkOpen())
            {
                chkBinkBonk.Checked = false;
            }

            //  ***** this is the heart of it ******   //
            // Update the relevant sensor gui sections:


            if (readerMgr.IsInventoryRunning())
            {
                // Sensor Processing

                UpdateAccelerometerGUI();
                UpdateTemperatureGUI();
                UpdateSOCGUI();
            }

            // Update tag statistics - this does the grid view
            ArrayList newTags = handleTags.GetNewTags();
            if (newTags.Count > 0)
                ProcessTagStats();

            // Check if Read is enabled and reader is connected. If so, enable read on the Reader.
            if (!chkReadDisabled.Checked && readerMgr.IsConnected())
            {
                if (chkReadAll.Checked)
                {
                    readerMgr.RestartRead(new ReaderManager.ReadCmdSettings("0000", "0000000000000000", 1, 0));
                }
                else if (chkReadSelected.Checked)
                {
                    // Refresh which tag we are performing Read command on
                    // via the selected tag in the grid view.
                    string selectedTag = stats.GetSelectedTagID();
                    string mask = "";
                    if (selectedTag != null)
                    {
                        mask = mask.PadRight(selectedTag.Length * 4, '1');
                        readerMgr.RestartRead(new ReaderManager.ReadCmdSettings(selectedTag, mask, 1, 0));
                    }
                    else
                    {
                        readerMgr.StopRead();
                    }
                }
            }
            else if (readerMgr.IsConnected())
                readerMgr.StopRead();

            // Update time info
            swGUI.Stop();
            long picosecPerTick = (1000L * 1000L * 1000L * 1000L) / Stopwatch.Frequency;
            long microsecGUIrate = swGUI.ElapsedTicks * picosecPerTick / (1000L * 1000L * 1000L);
            //long microsecHandlerrate = swHandler.ElapsedTicks * picosecPerTick / (1000L * 1000L * 1000L);

            if (microsecGUIrate > peakGuiTime) peakGuiTime = microsecGUIrate;
            //if (microsecHandlerrate > peakHandlerTime) peakHandlerTime = microsecHandlerrate;

            lblGUITime.Text = "GUI Time: " + peakGuiTime.ToString() + " ms";
            //lblHandlerTime.Text = "Handler Time: " + peakHandlerTime.ToString() + " ms";

            //Application.DoEvents();  // removing this cut the gui handler down from 55ms to 20ms peak time
            //                             with Saturn open, and with no effect on gui responsiveness!
            timerUpdateGUI.Enabled = true; // re-enable this timer.
        }

        private void SetHandleTagSettings()
        {
            // Acceleromter stuff
            accelInfo.filterChkd = false;
            if (chkFilter.Checked)
            {
                accelInfo.filterChkd = true;
                accelInfo.alpha = tbarLPFilter.Value;
            }

            // SOC WISP Stuff
            if (chkSOCV1V2.Checked)
                handleTags.SetSOCVersion(2, accelInfo);
            else
                handleTags.SetSOCVersion(1, accelInfo);

            // filtering
            handleTags.SetSOCAlpha(Math.Round(1.0 * tbarSocFilter.Value / tbarSocFilter.Maximum, 2));

            // adc to temperature conversion
            double test = Double.Parse(txtSocCalTemp2.Text);
            try
            {
                double slope = (Double.Parse(txtSocCalTemp2.Text) - Double.Parse(txtSocCalTemp1.Text)) /
                    (Double.Parse(txtSocCalAdc2.Text) - Double.Parse(txtSocCalAdc1.Text));
                double intercept = Double.Parse(txtSocCalTemp2.Text) - Double.Parse(txtSocCalAdc2.Text) * slope;
                handleTags.SetSOCIntercept(intercept);
                handleTags.SetSOCSlope(slope);
                lblSocCalOk.Text = "Cal ok.";
            }
            catch
            {
                lblSocCalOk.Text = "Parse Error.";
            }

            handleTags.SetSOCPlotTemp(chkSocPlotTemp.Checked);

        }

        private void ClearHandlerTimes()
        {
            peakHandlerTime = 0;
            peakGuiTime = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearGUIAll();
        }


        #endregion



        #region Tag Rates and Frame Rates



        private void timerFrameRateMeasure_Tick(object sender, EventArgs e)
        {
            if (chkSaturn.Checked)
            {
                double rate = (double)saturnFrames / ((double)timerFrameRateMeasure.Interval / 1000.0);
                Trace.WriteLine(rate.ToString() + " frames per second");
            }
            saturnFrames = 0;
        }

        private double tagReadRate;
        private void timerTagRateMeasure_Tick(object sender, EventArgs e)
        {
            // update this timer's refresh rate from text box
            if (txtTagRateWindowSeconds.Text == "") return;
            int newInterval = Convert.ToInt32(1000 * Convert.ToDouble(txtTagRateWindowSeconds.Text));
            if (newInterval < 100) newInterval = 1000;

            if (newInterval != timerTagRateMeasure.Interval)
            {
                timerTagRateMeasure.Interval = newInterval;
                lblTagsPerSecond.Text = "Measuring";
                handleTags.clearTagCount();
            }
            else
            {
                double timeInMs = (double)timerTagRateMeasure.Interval / 1000.0;
                accelInfo.alpha = (double)tbarTagsPerSecFilter.Value / 100;
                tagReadRate = accelInfo.alpha * tagReadRate + (1 - accelInfo.alpha) * handleTags.GetTagCount() / timeInMs;
                handleTags.clearTagCount();
                lblTagsPerSecond.Text = tagReadRate.ToString("#0.0");
            }
        }


        private void txtTagRateWindowSeconds_TextChanged(object sender, EventArgs e)
        {
            timerTagRateMeasure.Interval = 100; // set it off now when txt changed.
        }

        #endregion


        #region LLRP and Reader Control

        private void SetMode(ReaderManager.GuiModes newMode)
        {

            // Switch performs the action
            try
            {
                readerMgr.SetMode(newMode, txtIPAddress.Text);
            }
            catch (Exception e)
            {
                // todo: handle exception
                MessageBox.Show(e.ToString());

            }

            ReaderManager.GuiModes currentMode = readerMgr.getCurrentMode();
            //MessageBox.Show(currentMode.ToString());

            // Lastly, update button enables:

            // Connect button
            if (currentMode == ReaderManager.GuiModes.Idle)
            {
                txtMessages.Text = "Disconnected.";
                lblStatus.Text = "";
                btnConnect.Text = "Connect";
            }
            else
            {
                txtMessages.Text = "Disconnected.";
                lblStatus.Text = "Connected to IP Address: " + txtIPAddress.Text;
                btnConnect.Text = "Disconnect";
            }
            btnConnect.Enabled = true;

            // User inventory start / stop buttons
            btnInv.Enabled = (currentMode != ReaderManager.GuiModes.Idle);
            if (currentMode == ReaderManager.GuiModes.Ready)
                btnInv.Text = "Inventory";
            else
                btnInv.Text = "Stop";

            // settings button
            btnSettings.Enabled = (currentMode == ReaderManager.GuiModes.Ready || currentMode == ReaderManager.GuiModes.UserInventory);

            // Set the mode label to our current mode
            lblMode.Text = "Mode: " + currentMode.ToString();

        }

        private void btnInv_Click(object sender, EventArgs e)
        {
            if (btnInv.Text == "Inventory") //FIXME: Hack alert. Use getCurrentMode instead.
                SetMode(ReaderManager.GuiModes.UserInventory); // Start Inventory.
            else
                SetMode(ReaderManager.GuiModes.Ready); // Stop Inventory
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            if (btnConnect.Text == "Connect") //FIXME: Hack alert. Use getCurrentMode instead.
                SetMode(ReaderManager.GuiModes.Ready);
            else
                SetMode(ReaderManager.GuiModes.Idle);
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (readerMgr.IsConnected())
            {
                readerMgr.Disconnect();
            }
            log.CloseAllLogs();
        }

        #endregion


        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (setting == null || setting.IsDisposed)
            {
                setting = new ReaderLibrary.SettingsForm(readerMgr);
            }
            setting.Show();
        }

        private void chkSaturn_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAccelerometerGUI();
        }

        private void btnLogging_Click(object sender, EventArgs e)
        {

            if (loggingForm == null || loggingForm.IsDisposed)
            {
                loggingForm = new LoggingForm(log);
            }
            loggingForm.Show();
        }


        private void chkMIDI_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMIDI.Checked)
            {
                this.midi.ReOpenMidiConfig();
                UpdateAccelerometerGUI();
            }
            else
            {
                this.midi.DisposeMidiConfig();
            }
        }

        private void MainFrm_ResizeEnd(object sender, EventArgs e)
        {   
            // Update height of tag panel when resize occurs
            dgvTagStats.Height = pnlTagListInitialHeight + (this.Height - formInitialHeight);
        
        }

        private void chkBinkBonk_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBinkBonk.Checked)
            {
                this.binkBonk.reOpenBinkBonk();
            }
            else
            {
                this.binkBonk.disposeBinkBonk();
            }
        }



    }   // end class

}  // end namespace