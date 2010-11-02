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

using LLRP;
using LLRP.DataType;

using System.Diagnostics;

using ReaderLibrary;


namespace ReaderLibrary
{
    public partial class SettingsForm : Form
    {
        private ReaderManager reader;
        private ReaderManager.ReaderConfig readerConfigStruct;
        private ReaderManager.InventoryConfig invConfigStruct;

        public SettingsForm(ReaderManager readerRef)
        {
            reader = readerRef;
            InitializeComponent();
        }

        private void settings_Load_1(object sender, EventArgs e)
        {
            // set position of form
            base.Location = new Point(800, 250);

            if (reader.IsConnected())
            {
                // reader is our handle to the physical reader.
                readerConfigStruct = reader.getReaderConfig();
                invConfigStruct = reader.getInventoryConfig();
                PopulateReaderModeOptions();
                PopulateInvCmbBox();
                ApplySettingsToGUI(reader.getInventoryConfig());
                ApplySettingsToGUI(reader.getReaderConfig());
            }
            else
            {
                Error.message("Reader must be connected");
                this.Close();
            }

        }

        
        #region Reader and Inventory Settings

        // Transfer custom settings from the gui to the reader
        public bool ApplySettingsToReader()
        {
            bool error = false;
            bool[] ants = new bool[4];
            ants[0] = chkSettingsAnt1.Checked;
            ants[1] = chkSettingsAnt2.Checked;
            ants[2] = chkSettingsAnt3.Checked;
            ants[3] = chkSettingsAnt4.Checked;
            try
            {
                readerConfigStruct.antennaID = ants;
                readerConfigStruct.attenuation = (ushort)Convert.ToUInt16(txtSettingsAttenuation.Text);
                readerConfigStruct.channelIndex = (ushort)Convert.ToUInt16(txtSettingsChannelIdx.Text);
                readerConfigStruct.hopTableIndex = (ushort)Convert.ToUInt16(txtSettingsHopTableIdx.Text);
                
                //readerConfigStruct.periodicTriggerValue = (ushort)Convert.ToUInt16(txtSettingsPeriodicTriggerValue.Text);
                readerConfigStruct.readerSensitivity = (ushort)Convert.ToUInt16(txtSettingsReaderSensitivity.Text);
                readerConfigStruct.tagPopulation = (ushort)Convert.ToUInt16(txtSettingsTagPopulation.Text);
                readerConfigStruct.tagTransitTime = (ushort)Convert.ToUInt16(txtSettingsTagTransitTime.Text);
                readerConfigStruct.modeIndex = (ushort)((ReaderManager.ReaderMode)cmbSettingsMode.SelectedItem).GetModeIdentifier();

                invConfigStruct.startTrigger = (LLRP.ENUM_ROSpecStartTriggerType)(cmbSettingsROStartTrigger.SelectedItem);
                invConfigStruct.stopTrigger = (LLRP.ENUM_ROSpecStopTriggerType)(cmbSettingsROStopTrigger.SelectedItem);
                invConfigStruct.AITriggerType = (LLRP.ENUM_AISpecStopTriggerType)(cmbSettingsAIStopTrigger.SelectedItem);
                invConfigStruct.reportTrigger = (LLRP.ENUM_ROReportTriggerType)(cmbSettingsReportTrigger.SelectedItem);

                invConfigStruct.reportN = (ushort)Convert.ToUInt16(txtNumReports.Text);
                invConfigStruct.numAttempts = (ushort)Convert.ToUInt16(txtSettingsNumAttempts.Text);
                invConfigStruct.numTags = (ushort)Convert.ToUInt16(txtSettingsNumTags.Text);
                invConfigStruct.duration = (ushort)Convert.ToUInt16(txtSettingsDuration.Text);
                invConfigStruct.AITimeout = (uint)Convert.ToUInt16(txtSettingsAITimeout.Text);


            }
            catch (Exception e)
            {
                error = true;
                Error.message("Parse error: " + e.ToString());
            }
            try
            {
                reader.setReaderConfig(readerConfigStruct);
                reader.setInventoryConfig(invConfigStruct);
                //txtMessages.Text = "Config Set Successfully";
            }
            catch (Exception e)
            {
                Error.message("Parse error: " + e.ToString());
            }

            return error;
        // Transfer custom settings from the gui to the reader
        
            //AppendToDebugTextBox("Warning: Default Inventory settings are currently used.");
            //reader.SetDefaultInventoryConfig();
        }

        // Transfer reader's settings to the GUI
        public void ApplySettingsToGUI(ReaderManager.ReaderConfig config)
        {
            bool[] ants = config.antennaID;
            chkSettingsAnt1.Checked = ants[0];
            chkSettingsAnt2.Checked = ants[1];
            chkSettingsAnt3.Checked = ants[2];
            chkSettingsAnt4.Checked = ants[3];
            txtSettingsAttenuation.Text = config.attenuation.ToString();
            txtSettingsChannelIdx.Text = config.channelIndex.ToString();
            txtSettingsHopTableIdx.Text = config.hopTableIndex.ToString();
            for (int idx = 0; idx < cmbSettingsMode.Items.Count; idx++)
            {
                uint id = ((ReaderManager.ReaderMode)cmbSettingsMode.Items[idx]).GetModeIdentifier();
                if (id == config.modeIndex)
                    cmbSettingsMode.SelectedIndex = idx;
            }

            // todo: config.modeIndex;

            //txtSettingsPeriodicTriggerValue.Text = config.periodicTriggerValue.ToString();
            txtSettingsReaderSensitivity.Text = config.readerSensitivity.ToString();
            txtSettingsTagPopulation.Text = config.tagPopulation.ToString();
            txtSettingsTagTransitTime.Text = config.tagTransitTime.ToString();
        }

        // Transfer reader's settings to the GUI
        public void ApplySettingsToGUI(ReaderManager.InventoryConfig config)
        {
            // this sets the cmb box to show the new set values
            cmbSettingsROStartTrigger.SelectedItem = config.startTrigger;
            cmbSettingsROStopTrigger.SelectedItem = config.stopTrigger;
            cmbSettingsReportTrigger.SelectedItem = config.reportTrigger;
            cmbSettingsAIStopTrigger.SelectedItem = config.AITriggerType;

            txtNumReports.Text = config.reportN.ToString();
            txtSettingsNumAttempts.Text = config.numAttempts.ToString();
            txtSettingsNumTags.Text = config.numTags.ToString();
            txtSettingsDuration.Text = config.duration.ToString();
            txtSettingsAITimeout.Text = config.AITimeout.ToString();
        }

        private void PopulateInvCmbBox()
        {
            cmbSettingsROStartTrigger.DataSource = Enum.GetValues(typeof(LLRP.ENUM_ROSpecStartTriggerType));
            cmbSettingsROStopTrigger.DataSource = Enum.GetValues(typeof(LLRP.ENUM_ROSpecStopTriggerType));
            cmbSettingsReportTrigger.DataSource = Enum.GetValues(typeof(LLRP.ENUM_ROReportTriggerType));
            cmbSettingsAIStopTrigger.DataSource = Enum.GetValues(typeof(LLRP.ENUM_AISpecStopTriggerType));
        }

        public void PopulateReaderModeOptions()
        {
            cmbSettingsMode.Items.Clear();

            ReaderManager.ReaderMode[] modes = reader.GetReaderModulationModes();
            if (modes == null)
                return;
            for (int i = 0; i < modes.Length && modes[i] != null; i++)
                cmbSettingsMode.Items.Add(modes[i]);
        }

        #endregion

        

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSettingsDefault_Click(object sender, EventArgs e)
        {
            reader.SetDefaultReaderConfig();
            ApplySettingsToGUI(reader.getReaderConfig());
            reader.SetDefaultInventoryConfig();
            ApplySettingsToGUI(reader.getInventoryConfig());
            PopulateInvCmbBox();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // only hides it
            this.Close();
        }

        private void btnSettingsApply_Click(object sender, EventArgs e)
        {
            bool error = ApplySettingsToReader();
            reader.setReaderConfig(readerConfigStruct);
            reader.setInventoryConfig(invConfigStruct);
            //ApplySettingsToGUI(reader.getInventoryConfig());
            //ApplySettingsToGUI(reader.getReaderConfig());
            if (reader.IsInventoryRunning())
            {
                Error.message("Inventory needs to be restarted, for setting change to apply");
            }
            
            if (!error)
            {
                this.Close();
            }
        }

        private void cmbSettingsROStartTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    #region Message Box

    public class Error
    {
        public static int message(string msg)
        {
            MessageBox.Show(msg,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            return 0;
        }
    }


    #endregion
}
        
