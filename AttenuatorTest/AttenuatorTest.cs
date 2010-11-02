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
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;

using ReaderLibrary;

using System.Diagnostics;

using ZedGraph;

namespace AttenuatorTest
{
    public class AttenuatorTest : ITagHandler
    {

        private AttenStepInfo attenConfig;
        private Form attenFrm;

        private AttenLoggingManager logger;

        private double tagCount = 0;

        public AttenuatorTest()
        {
            attenConfig = new AttenStepInfo();
            logger = new AttenLoggingManager();
        }

        public AttenuatorTest(AttenStepInfo config)
        {
            this.attenConfig = config;
            logger = new AttenLoggingManager();
        }

        public AttenStepInfo GetAttenConfig()
        {
            return attenConfig;
        }

        public void setAttenConfig(AttenStepInfo config)
        {
            this.attenConfig = config;
        }

        public void ClearTagCount()
        {
            tagCount = 0;
        }

        public void HandleTagReceived(MyTag t)
        {
            tagCount += t.GetCount();
        }

        public double GetTagCount()
        {
            return tagCount;
        }


        // user aborted atten test - do we need to take action?
        public void Abort()
        {

        }

        public AttenLoggingManager GetLoggingMgr()
        {
            return logger;
        }



        public void AttenStepFinished(PointPairList graphData)
        {

            // Collect stats
            attenConfig.tagRate = 1000.0 * (double)tagCount / (double)attenConfig.attenRunTime;

            // Log to file.
            logger.WriteToLog(attenConfig);

            // Update graph
            graphData.Add(attenConfig.attenuation, attenConfig.tagRate);

            // Increase attenuation 
            attenConfig.attenuation++;
        }


        #region Old Code

        /*private void btnAttenTestStart_Click(object sender, EventArgs e)
        {
            SetMode(ReaderManager.GuiModes.AttenuatorTest);
        }

        private void btnAttenTestAbort_Click(object sender, EventArgs e)
        {
            if (!chkAttenTestCreateFiles.Enabled)
            {
                chkAttenTestCreateFiles.Enabled = true;
                chkAttenTestCreateFiles.Checked = false;
            }

            if (twAttenTest != null) twAttenTest.Close();
            if (twAttenTestDebug != null) twAttenTestDebug.Close();
            if (twAttenTestMonitorEvents != null) twAttenTestMonitorEvents.Close();
            if (twAttenTestMonitorOutput != null) twAttenTestMonitorOutput.Close();
            if (twAttenTestMonitorRawEvents != null) twAttenTestMonitorRawEvents.Close();
            SetMode(ReaderManager.GuiModes.Ready);
        }*/

        //private int attenTagsSeen = 0;
        /*private void HandleAttenTestStats(MyTag tag)
        {
            if (tag != null)
                attenTagsSeen += tag.GetCount();

            if (twAttenTest != null)
            {
                // Raw data
                twAttenTest.WriteLine(
                    tag.GetTime() + "\t" +
                    tag.GetEpcID() + "\t" +
                    tag.GetCount() + "\t" +
                    tag.GetAccessResultData() + "\t" +
                    lblTagsPerSecond.Text + "\t" +
                    tag.GetRSSI() + "\t" +
                    tag.GetFrequency());


                // Debugging info
                string epcInfo = tag.GetEpcID();
                twAttenTestDebug.WriteLine(
                    tag.GetTime() + "\t" +
                    tag.GetEpcID() + "\t" +
                    tag.GetCount() + "\t" +
                    tag.GetAccessResultData() + "\t" +
                    "0" + "\t" +
                    tag.GetRSSI() + "\t" +
                    tag.GetFrequency());
                twAttenTestDebug.WriteLine("IDENTIFIERS # \t" + Convert.ToInt32(epcInfo.Substring(0, 2), 16));
                twAttenTestDebug.WriteLine("# STATE_READY \t" + Convert.ToInt32(epcInfo.Substring(2, 2), 16));
                twAttenTestDebug.WriteLine("# STATE_REPLY \t" + Convert.ToInt32(epcInfo.Substring(4, 2), 16));
                twAttenTestDebug.WriteLine("# STATE_ACKGD \t" + Convert.ToInt32(epcInfo.Substring(6, 2), 16));
                twAttenTestDebug.WriteLine("# QUERY_PAKCT \t" + Convert.ToInt32(epcInfo.Substring(8, 2), 16));
                twAttenTestDebug.WriteLine("# ACK_PACKETS \t" + Convert.ToInt32(epcInfo.Substring(10, 2), 16));
                twAttenTestDebug.WriteLine("# TAR_OVRFLOW \t" + Convert.ToInt32(epcInfo.Substring(12, 2), 16));
                twAttenTestDebug.WriteLine("# SLEEP_START \t" + Convert.ToInt32(epcInfo.Substring(14, 2), 16));
                twAttenTestDebug.WriteLine("# DELNOTFOUND \t" + Convert.ToInt32(epcInfo.Substring(16, 2), 16));
                twAttenTestDebug.WriteLine("# QUERY_REPET \t" + Convert.ToInt32(epcInfo.Substring(18, 2), 16));
                twAttenTestDebug.WriteLine("# QUERY_ADJUT \t" + Convert.ToInt32(epcInfo.Substring(20, 2), 16));
                twAttenTestDebug.WriteLine();
            }


        }*/

        /*private short attenuatorStep;
        private int attenRunTime;
        private int attenSettleTime;
        private void timerAttenuatorTest_Tick(object sender, EventArgs e)
        {
            timerAttenuatorTest.Enabled = false;

            if (!readerMgr.IsConnected())
            {
                //txtMessages.Text = "Reader must be connected to run.";
                txtAttnTestStatus.Text = "Reader must be connected to run.";
                SetMode(ReaderManager.GuiModes.Idle);
                return;
            }

            // We just finished the wait period or are just starting.
            if (!readerMgr.IsInventoryRunning())
            {
                // Starting Condition
                if (attenuatorStep == -1)
                {
                    // Start out with defaults.
                    readerMgr.SetDefaultReaderConfig();
                    readerMgr.SetDefaultInventoryConfig();

                    ReaderManager.ReaderConfig readerConfigStruct = readerMgr.getReaderConfig();
                    ReaderManager.InventoryConfig invConfigStruct = readerMgr.getInventoryConfig();

                    // Collect required settings
                    attenRunTime = Int32.Parse(txtAttenStepOnTime.Text) * 1000;
                    attenSettleTime = Int32.Parse(txtAttenStepOffTime.Text) * 1000;

                    if (attenConfig.duration.ToString() != "0")
                    {
                        invConfigStruct.AITriggerType = LLRP.ENUM_AISpecStopTriggerType.Duration;
                        invConfigStruct.duration = UInt16.Parse(txtAttenTestDuration.Text);
                    }
                    else if (attenConfig.attempts.ToString() != "0")
                    {
                        invConfigStruct.AITriggerType = LLRP.ENUM_AISpecStopTriggerType.Tag_Observation;
                        invConfigStruct.numAttempts = UInt16.Parse(txtAttenTestAttempts.Text);
                    }
                    else if (attenConfig.numTags.ToString() != "0")
                    {
                        invConfigStruct.AITriggerType = LLRP.ENUM_AISpecStopTriggerType.Tag_Observation;
                        invConfigStruct.numTags = UInt16.Parse(txtAttenTestNumTags.Text);
                    }

                    // Set inventory settings
                    readerMgr.setInventoryConfig(invConfigStruct);

                    // Init graph
                    attenTestGraphReadRateData.Clear();
                    graphAttenTest.AxisChange();
                    graphAttenTest.Refresh();
                }

                // Run condition
                if (attenuatorStep < 16)
                {
                    // Set reader settings.
                    ReaderManager.ReaderConfig readerConfigStruct = readerMgr.getReaderConfig();

                    if (attenuatorStep < 0)
                        attenuatorStep = 0;

                    readerConfigStruct.attenuation = (ushort)attenuatorStep;
                    readerMgr.setReaderConfig(readerConfigStruct);

                    // Adjust GUI
                    btnClear_Click(null, null);
                    timerAttenuatorTest.Interval = attenRunTime;
                    timerAttenuatorTest.Enabled = true;
                    progressAttenTest.Maximum = attenRunTime;
                    //txtMessages.Text = "Attenuation test running...";
                    txtAttnTestStatus.Text = "Running Attenuation Step: " + attenuatorStep.ToString();

                    // Start test.
                    readerMgr.StartInventory();
                }
                else
                {
                    // don't restart timer or reader.
                    attenConfig.runID = ((int)(Convert.ToInt16(txtAttnRunID.Text)) + 1).ToString();
                    txtAttnTestStatus.Text = "Test Done.";
                    //txtMessages.Text = "Attenuator Test Done.";
                    SetMode(ReaderManager.GuiModes.Ready);
                }

            }

            // We just finished an attenuation value
            else
            {
                readerMgr.StopInventory();

                // Collect stats
                double rate = 1000.0 * (double)attenTagsSeen / (double)attenRunTime;

                // Finish logging and close out file
                twAttenTest.Close();
                if (twAttenTestDebug != null) twAttenTestDebug.Close();
                if (twAttenTestMonitorEvents != null) twAttenTestMonitorEvents.Close();
                if (twAttenTestMonitorOutput != null) twAttenTestMonitorOutput.Close();
                if (twAttenTestMonitorRawEvents != null) twAttenTestMonitorRawEvents.Close();

                // Update graph
                attenTestGraphReadRateData.Add(attenuatorStep, rate);
                graphAttenTest.AxisChange();
                graphAttenTest.Refresh();

                // Open text files for logging.
                attenuatorStep += 1;
                if (!chkAttenTestCreateFiles.Enabled && attenuatorStep > 0)
                    if (!CreateAttenTestFiles())
                        return;

                // Display friendly message, run the settle time.
                timerAttenuatorTest.Interval = attenSettleTime;
                timerAttenuatorTest.Enabled = true;
                progressAttenTest.Maximum = attenSettleTime;
                //txtMessages.Text = "Attenuation settling running...";
                txtAttnTestStatus.Text = "Parsing/Settling Attenuation Step: " + attenuatorStep.ToString();
            }

            // Reset progress bar (for user display only).
            progressAttenTest.Value = 0;
        }*/

        

        /*public bool CreateAttenTestFiles()
        {
            // Check run ID
            if (attenConfig.runID.ToString() == "0")
                txtAttnRunID.Text = "0";

            // Make sure path exists
            string path = "c:\\local\\RFID Data\\" + txtAttnRunID.Text + "\\";
            try
            {
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                //txtMessages.Text = "Error.";
                txtAttnTestStatus.Text = "Couldn't Create Folder: " + path;
                AppendToDebugTextBox(e.ToString());
                btnAttenTestAbort_Click(null, null);
                return false;
            }

            // Open Text Writer for writing run info file.
            try
            {
                twAttenTest = new StreamWriter(path + "Info.log");
            }
            catch (Exception e)
            {
                //txtMessages.Text = "Error.";
                txtAttnTestStatus.Text = "File Open Error.";
                AppendToDebugTextBox(e.ToString());
                btnAttenTestAbort_Click(null, null);
                return false;
            }
            twAttenTest.WriteLine(System.DateTime.Now.ToString());
            twAttenTest.WriteLine("Atten run time ms: " + attenRunTime.ToString());
            twAttenTest.WriteLine(readerMgr.getReaderConfig().antennaID.ToString()); // an example
            twAttenTest.Close();

            // Open Text Writer for logging - normal file.
            string prefix = "Reader_";
            string note = null;
            if (attenuatorStep.ToString().Length < 2)
                note = "0" + attenuatorStep.ToString() + "dBm";
            else
                note = attenuatorStep.ToString() + "dBm";

            string currTime = ((DateTime.Now.ToString()).Replace("/", "-")).Replace(":", ".");
            try
            {
                twAttenTest = new StreamWriter(path + prefix + note + ".log");
            }
            catch (Exception e)
            {
                //txtMessages.Text = "Error.";
                txtAttnTestStatus.Text = "File Open Error.";
                AppendToDebugTextBox(e.ToString());
                btnAttenTestAbort_Click(null, null);
                return false;
            }
            twAttenTest.WriteLine(System.DateTime.Now.ToString());
            //twAttenTest.WriteLine("Last Seen Time" + "\t" + "ID" + "\t\t\t" + "Count" + "\t" + "Data" + "\t" + "Speed" + "\t" + "Diff in Data");

            // Open text file for debugging info.
            if (chkAttenTestWriteDebuggingFile.Checked)
            {
                prefix = "ReaderDebug_";
                currTime = ((DateTime.Now.ToString()).Replace("/", "-")).Replace(":", ".");
                try
                {
                    twAttenTestDebug = new StreamWriter(path + prefix + note + ".log");
                }
                catch (Exception e)
                {
                    txtMessages.Text = "Error.";
                    txtAttnTestStatus.Text = "File Open Error.";
                    AppendToDebugTextBox(e.ToString());
                    btnAttenTestAbort_Click(null, null);
                    return false;
                }
                twAttenTestDebug.WriteLine(System.DateTime.Now.ToString());
                //twAttenTestDebug.WriteLine("Last Seen Time" + "\t" + "ID" + "\t\t\t" + "Count" + "\t" + "Data" + "\t" + "Speed" + "\t" + "Diff in Data");
            }

            // Open events text file for monitor to write into
            prefix = "MonitorDebug_Events_";
            currTime = ((DateTime.Now.ToString()).Replace("/", "-")).Replace(":", ".");
            try
            {
                twAttenTestMonitorEvents = new StreamWriter(path + prefix + note + ".log");
                twAttenTestMonitorEvents.Close();
            }
            catch (Exception e)
            {
                txtMessages.Text = "Error.";
                txtAttnTestStatus.Text = "File Open Error.";
                AppendToDebugTextBox(e.ToString());
                btnAttenTestAbort_Click(null, null);
                return false;
            }

            // Open output text file for monitor to write into
            prefix = "MonitorDebug_Output_";
            currTime = ((DateTime.Now.ToString()).Replace("/", "-")).Replace(":", ".");
            try
            {
                twAttenTestMonitorOutput = new StreamWriter(path + prefix + note + ".log");
                twAttenTestMonitorOutput.Close();
            }
            catch (Exception e)
            {
                txtMessages.Text = "Error.";
                txtAttnTestStatus.Text = "File Open Error.";
                AppendToDebugTextBox(e.ToString());
                btnAttenTestAbort_Click(null, null);
                return false;
            }

            // Open raw events text file for monitor to write into
            prefix = "MonitorDebug_Raw_";
            currTime = ((DateTime.Now.ToString()).Replace("/", "-")).Replace(":", ".");
            try
            {
                twAttenTestMonitorRawEvents = new StreamWriter(path + prefix + note + ".log");
                twAttenTestMonitorRawEvents.Close();
            }
            catch (Exception e)
            {
                txtMessages.Text = "Error.";
                txtAttnTestStatus.Text = "File Open Error.";
                AppendToDebugTextBox(e.ToString());
                btnAttenTestAbort_Click(null, null);
                return false;
            }

            return true;
        }

        /*private void txtAttenTestDuration_TextChanged(object sender, EventArgs e)
        {
            // disable event for other boxes
            txtAttenTestNumTags.TextChanged -= new EventHandler(txtAttenTestNumTags_TextChanged);
            txtAttenTestAttempts.TextChanged -= new EventHandler(txtAttenTestAttempts_TextChanged);

            if (Int32.Parse(txtAttenTestDuration.Text) < 0)
                txtAttenTestDuration.Text = "100";
            else
            {
                txtAttenTestAttempts.Text = "0";
                txtAttenTestNumTags.Text = "0";
            }

            // enable event for other boxes
            txtAttenTestNumTags.TextChanged += new EventHandler(txtAttenTestNumTags_TextChanged);
            txtAttenTestAttempts.TextChanged += new EventHandler(txtAttenTestAttempts_TextChanged);
        }

        private void txtAttenTestNumTags_TextChanged(object sender, EventArgs e)
        {
            // disable event for other boxes
            txtAttenTestAttempts.TextChanged -= new EventHandler(txtAttenTestAttempts_TextChanged);
            txtAttenTestDuration.TextChanged -= new EventHandler(txtAttenTestDuration_TextChanged);

            if (Int32.Parse(txtAttenTestNumTags.Text) < 0)
                txtAttenTestNumTags.Text = "0";
            else
            {
                txtAttenTestAttempts.Text = "0";
                txtAttenTestDuration.Text = "0";
            }

            // enable event for other boxes
            txtAttenTestAttempts.TextChanged += new EventHandler(txtAttenTestAttempts_TextChanged);
            txtAttenTestDuration.TextChanged += new EventHandler(txtAttenTestDuration_TextChanged);
        }

        private void txtAttenTestAttempts_TextChanged(object sender, EventArgs e)
        {
            // disable event for other boxes
            txtAttenTestNumTags.TextChanged -= new EventHandler(txtAttenTestNumTags_TextChanged);
            txtAttenTestDuration.TextChanged -= new EventHandler(txtAttenTestDuration_TextChanged);


            if (Int32.Parse(txtAttenTestAttempts.Text) < 0)
                txtAttenTestAttempts.Text = "0";
            else
            {
                txtAttenTestNumTags.Text = "0";
                txtAttenTestDuration.Text = "0";
            }

            // enable event for other boxes
            txtAttenTestNumTags.TextChanged += new EventHandler(txtAttenTestNumTags_TextChanged);
            txtAttenTestDuration.TextChanged += new EventHandler(txtAttenTestDuration_TextChanged);
        }*/


        /*PointPairList attenTestGraphReadRateData = new PointPairList();
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

        public void InitAttenTestRunNumber()
        {
            int i = 0;
            string filename;

            while (i < 500)
            {
                filename = "c:\\local\\RFID Data\\" + i.ToString();
                if (!System.IO.Directory.Exists(filename))
                    break;
                i++;
            }
            txtAttnRunID.Text = i.ToString();
        }*/

        #endregion
    }
}
