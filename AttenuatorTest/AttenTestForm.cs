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
//using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;
using System.Diagnostics;

using ReaderLibrary;
using Logging;
using ZedGraph;

namespace AttenuatorTest
{
    public partial class AttenTestForm : Form, IRFIDGUI
    {
        private AttenuatorTest attenMgr;
        private ReaderManager readerMgr;

        public AttenTestForm()
        {
            attenMgr = new AttenuatorTest();
            readerMgr = new ReaderManager(this, attenMgr);
            InitializeComponent();
        }

        private void AttenTestForm_Load(object sender, EventArgs e)
        {
            InitAttenTestGraph();
        }

        private void AttenTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            attenMgr.GetLoggingMgr().CloseAllLogs();
            readerMgr.Disconnect();
        }


        string debugText = "";
        public void AppendToDebugTextBox(string text)
        {
            debugText += text;
            if (debugText.Length > 1000)
                debugText.Remove(0, Math.Min(debugText.Length - 1000, 0));
        }

        string mainText = "";
        public void AppendToMainTextBox(string text)
        {
            mainText += text;
            if (mainText.Length > 1000)
                mainText.Remove(0, Math.Min(mainText.Length - 1000, 0));
        }




        PointPairList attenTestGraphReadRateData = new PointPairList();
        public void InitAttenTestGraph()
        {
            // setup the axis titles, etc.
            // get a reference to the GraphPane
            GraphPane myPane = graphAttenTest.GraphPane;

            // Set the Titles
            myPane.Title.Text = "Read Rate versus Attenuation";
            myPane.XAxis.Title.Text = "Attenuation";
            myPane.YAxis.Title.Text = "Rate (tags/sec)";

            // Generate the line
            LineItem myCurve = myPane.AddCurve(null,
                  attenTestGraphReadRateData, Color.Red, SymbolType.Diamond);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            graphAttenTest.AxisChange();
        }





        private Form readerSettingsForm;
        private void btnReaderSettings_Click(object sender, EventArgs e)
        {
            if (readerSettingsForm == null || readerSettingsForm.IsDisposed)
            {
                readerSettingsForm = new ReaderLibrary.SettingsForm(readerMgr);
            }
            readerSettingsForm.Show();
        }

        private Form attenSettingsForm;
        private void btnAttenSettings_Click(object sender, EventArgs e)
        {
            if (attenSettingsForm == null || attenSettingsForm.IsDisposed)
            {
                // todo - this could causing threading issues. if gui is only thread, we may be ok.
                attenSettingsForm = new EditAttenSettings(attenMgr);
            }
            attenSettingsForm.Show();
        }

        private Form attenLoggingForm;
        private void btnLoggingSettings_Click(object sender, EventArgs e)
        {
            if (attenLoggingForm == null || attenLoggingForm.IsDisposed)
            {
                // todo - this could causing threading issues. if gui is only thread, we may be ok.
                attenLoggingForm = new LoggingForm(attenMgr.GetLoggingMgr());
            }
            attenLoggingForm.Show();
        }



        private double ticks = 0;
        
        double oldTagCount = 0.0;

        private void timerProgress_Tick(object sender, EventArgs e)
        {

            // We need roughly 1 seconds timer
            // for telling user the progress of the attenuation test.
            // This just increments the value of the progress bar by the time interval.
            if (timerAttenuatorTest.Enabled)
            {
                ticks++;

                double pcntComplete = ticks * 1000;

                progressAttenTest.Value = Convert.ToInt32(Math.Min(progressAttenTest.Maximum, pcntComplete));
                
            }

            double currentTagCount = attenMgr.GetTagCount();
            double changeTagCount = currentTagCount - oldTagCount;
            oldTagCount = currentTagCount;
            lblTagsPerSecond.Text = changeTagCount + "";
        }

        private void timerUpdateGui_Tick_1(object sender, EventArgs e)
        {


            ReaderManager.GuiModes currentMode = readerMgr.getCurrentMode();

            // Update whether buttons are enabled.
            btnAbort.Enabled = readerMgr.IsConnected() && timerAttenuatorTest.Enabled;
            btnStart.Enabled = readerMgr.IsConnected() && !timerAttenuatorTest.Enabled;
            btnReaderSettings.Enabled = readerMgr.IsConnected() && !timerAttenuatorTest.Enabled;

            // Connect button and connect status
            if (currentMode == ReaderManager.GuiModes.Idle)
            {
                lblStatus.Text = "";
                btnConnect.Text = "Connect";
            }
            else
            {
                lblStatus.Text = "Connected to IP Address: " + txtIPAddress.Text;
                btnConnect.Text = "Disconnect";
            }
            btnConnect.Enabled = true;

            // Set the mode label to our current mode
            lblMode.Text = "Mode: " + currentMode.ToString();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            try
            {
                if (btnConnect.Text == "Connect")
                    readerMgr.Connect(txtIPAddress.Text);
                else
                    readerMgr.Disconnect();
            }
            catch (Exception exc)
            {
                // todo: handle exception
                MessageBox.Show(exc.ToString());

            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            timerAttenuatorTest.Enabled = false;
            if (readerMgr.IsInventoryRunning())
            {
                readerMgr.StopInventory();
            }
            attenMgr.Abort();
        }




        private void btnStart_Click(object sender, EventArgs e)
        {
            if (readerMgr.IsConnected())
            {
                attenMgr.GetAttenConfig().attenuation = -1;
                timerAttenuatorTest.Interval = 10;
                timerAttenuatorTest.Enabled = true;
            }
            else
            {
                MessageBox.Show("Must be connected.");
            }
        }

        private void timerAttenuatorTest_Tick(object sender, EventArgs e)
        {

            timerAttenuatorTest.Enabled = false;



            if (!readerMgr.IsConnected())
            {
                txtAttnTestStatus.Text = "Reader must be connected to run.";
                readerMgr.SetMode(ReaderManager.GuiModes.Idle, txtIPAddress.Text);
                return;
            }



            // We just finished the wait period or are just starting.
            if (!readerMgr.IsInventoryRunning())
            {
                // Starting Condition
                if (attenMgr.GetAttenConfig().attenuation == -1)
                {
                    // Init graph
                    attenTestGraphReadRateData.Clear();
                    graphAttenTest.AxisChange();
                    graphAttenTest.Refresh();
                }

                // Run condition
                if (attenMgr.GetAttenConfig().attenuation < 16)
                {
                    // Set reader settings.
                    ReaderManager.ReaderConfig readerConfigStruct = readerMgr.getReaderConfig();

                    if (attenMgr.GetAttenConfig().attenuation < 0)
                        attenMgr.GetAttenConfig().attenuation = 0;

                    readerConfigStruct.attenuation = (ushort)attenMgr.GetAttenConfig().attenuation;
                    readerMgr.setReaderConfig(readerConfigStruct);

                    // Adjust GUI

                    timerAttenuatorTest.Interval = attenMgr.GetAttenConfig().attenRunTime;
                    timerAttenuatorTest.Enabled = true;

                    progressAttenTest.Maximum = attenMgr.GetAttenConfig().attenRunTime;
                    ticks = 0;
                    attenMgr.ClearTagCount();
                    oldTagCount = 0.0;

                    txtAttnTestStatus.Text = "Running Attenuation Step: " + attenMgr.GetAttenConfig().attenuation.ToString();

                    // Start test.
                    readerMgr.StartInventory();
                }
                else
                {
                    // don't restart timer or reader.

                    txtAttnTestStatus.Text = "Test Done.";

                    readerMgr.SetMode(ReaderManager.GuiModes.Ready, txtIPAddress.Text);
                }

            }

            // We just finished an attenuation value
            else
            {
                readerMgr.StopInventory();

                attenMgr.AttenStepFinished(attenTestGraphReadRateData);

                // Update graph
                graphAttenTest.AxisChange();
                graphAttenTest.Refresh();

                // Display friendly message, run the settle time.
                timerAttenuatorTest.Interval = attenMgr.GetAttenConfig().attenSettleTime;
                timerAttenuatorTest.Enabled = true;

                progressAttenTest.Maximum = attenMgr.GetAttenConfig().attenSettleTime;
                ticks = 0;

                txtAttnTestStatus.Text = "Parsing/Settling Attenuation Step: " + attenMgr.GetAttenConfig().attenuation.ToString();
            }

            // Reset progress bar (for user display only).
            progressAttenTest.Value = 0;
        }
    }
}