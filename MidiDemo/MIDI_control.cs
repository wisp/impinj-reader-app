using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace MIDI_Control_Demo
{
    public partial class MIDI_control : Form
    {
        // This class helps store info about the note that is playing.
        private class NoteInfo
        {
            public int instr;
            public int val;
            public int vol;
            public int pitch;
            public int mod;
        }

        private OutputDevice outDevice;

        // Is this always the proper device ID??
        private int outDeviceID = 0;

        private OutputDeviceDialog outDialog;

        private NoteInfo curNote;

        private bool midiStarted = false;

        private bool noteStarted = false;

        static int VELOCITY = 127;

        // Local acceleration data: current value
        private int xac_Scaled;
        private int yac_Scaled;
        private int zac_Scaled;

        // Local accel scaling: min and max recorded values
        private double xac_min, xac_max;
        private double yac_min, yac_max;
        private double zac_min, zac_max;

        // Signals that this is the first run after a load or MIDI reset
        private bool firstRunFlag = true;

        // The little XY plot for testing
        XY_panel myXYPanel;
        //

        public MIDI_control()
        {
            InitializeComponent();
            myXYPanel = new XY_panel(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            updateParamDisplay();

            base.OnLoad(e);

            curNote = new NoteInfo();
        }

        private void updateNote(bool forceRestart)
        {

            if (!midiStarted)
                return;

            if (!noteStarted)
            {
                killNote();
                curNote = new NoteInfo(); // Reset note info
                return;
            }

            // Build up new note candidate
            NoteInfo newNote = new NoteInfo();

            if(opt_modeMan.Checked)
            {
                // All settings manual, no accel control
                newNote.instr = (int)(instrumentSelect.Value);
                newNote.val = (int)(noteSelect.Value);
                newNote.vol = (int)(volumeSelect.Value);
                newNote.pitch = (int)(pitchBendSelect.Value);
                newNote.mod = (int)(modSelect.Value);
            }

            else if(opt_mode1.Checked)
            {
                // X accel controls note value, Y accel controls modulation
                newNote.instr = (int)(instrumentSelect.Value);

                // Use WHOLE TONE SCALE for creepy sounds, and rescale for mid-high range notes
                newNote.val = ((int)((xac_Scaled + 240) / 8)) * 2;

                newNote.vol = yac_Scaled;
                newNote.pitch = (int)(pitchBendSelect.Value);
                newNote.mod = (int)(modSelect.Value);
            }
            else if(opt_mode2.Checked)
            {
                // X accel controls pitch bend, Y accel controls modulation
                newNote.instr = (int)(instrumentSelect.Value);
                newNote.val = (int)(noteSelect.Value);
                newNote.vol = (int)(volumeSelect.Value);
                newNote.pitch = xac_Scaled;
                newNote.mod = yac_Scaled;
            }
            else if(opt_mode3.Checked)
            {
                // X accel controls pitch bend, Y accel controls volume
                newNote.instr = (int)(instrumentSelect.Value);
                newNote.val = (int)(noteSelect.Value);
                newNote.vol = yac_Scaled;
                newNote.pitch = xac_Scaled;
                newNote.mod = (int)(modSelect.Value);
            }
            else
            {
                // None of the option boxes are checked?!
                return;
            }

            // Keep track; should note be restarted after all channel commands
            bool restartNote = forceRestart;

            if (newNote.instr != curNote.instr || forceRestart)
            {
                // Change instrument patch (aka program)
                outDevice.Send(new ChannelMessage(ChannelCommand.ProgramChange, 0, newNote.instr, 0));
                restartNote = true;
            }

            if (newNote.val != curNote.val)
            {
                // Restarting note will update note value
                restartNote = true;
            }

            if (newNote.vol != curNote.vol || forceRestart)
            {
                // Change master volume (controller # 0x07)
                outDevice.Send(new ChannelMessage(ChannelCommand.Controller, 0, 0x07, newNote.vol));
            }

            if (newNote.pitch != curNote.pitch || forceRestart)
            {
                // Change pitch wheel setting
                outDevice.Send(new ChannelMessage(ChannelCommand.PitchWheel, 0, 0, newNote.pitch));
            }

            if (newNote.mod != curNote.mod || forceRestart)
            {
                // Change Modulation controller (0x01) setting
                outDevice.Send(new ChannelMessage(ChannelCommand.Controller, 0, 0x01, newNote.mod));
            }

            // Restart the note based on what was updated...
            if (restartNote)
            {
                outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, curNote.val, 0));
                outDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, newNote.val, VELOCITY));
            }

            curNote = newNote;

        }

        private void killNote()
        {
            if (!midiStarted)
                return;

            outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, curNote.val, 0));

            noteStarted = false;

        }

        private void btn_startMIDI_Click(object sender, EventArgs e)
        {
            if (midiStarted)
            {
                closeMidiOutput();
            }
            else
            {
                // Try to open midi out. If successful, initialize other stuff
                if (!openMidiOutput())
                    return;

                updateNote(true);
                killNote();

                firstRunFlag = true;

            }
           
        }

        private void btn_noteOn_Click(object sender, EventArgs e)
        {
            noteStarted = true;
            updateNote(true);
        }

        private void btn_noteOff_Click(object sender, EventArgs e)
        {
            killNote();
        }


        public void updateAccelValues(double xac, double yac, double zac)
        {
            if (firstRunFlag)
            {
                // Don't use initial values sent in by accel demo, as they are garbage.
                xac_max = yac_max = zac_max = 51;
                xac_min = yac_min = zac_min = 49;
                xac = yac = zac = 50;
                firstRunFlag = false;
            }
            else
            {
                if (xac > xac_max)
                    xac_max = xac;
                if (xac < xac_min)
                    xac_min = xac;

                if (yac > yac_max)
                    yac_max = yac;
                if (yac < yac_min)
                    yac_min = yac;

                if (zac > zac_max)
                    zac_max = zac;
                if (zac < zac_min)
                    zac_min = zac;

                /*
                // High pass filter the max/min vals
                xac_max = 0.9 * xac_max + 0.1 * (xac_max + xac_min) / 2;
                xac_min = 0.9 * xac_min + 0.1 * (xac_max + xac_min) / 2;
                yac_max = 0.9 * yac_max + 0.1 * (xac_max + xac_min) / 2;
                yac_min = 0.9 * yac_min + 0.1 * (xac_max + xac_min) / 2;
                zac_max = 0.9 * zac_max + 0.1 * (xac_max + xac_min) / 2;
                zac_min = 0.9 * zac_min + 0.1 * (xac_max + xac_min) / 2;
                */
                
                // Autoscale based on historical min/max values
                xac_Scaled = (int)Math.Round(((xac - xac_min) * 127) / (xac_max - xac_min));
                yac_Scaled = (int)Math.Round(((yac - yac_min) * 127) / (yac_max - yac_min));
                zac_Scaled = (int)Math.Round(((zac - zac_min) * 127) / (zac_max - zac_min));
            }
            this.updateNote(false);
        }

        private void openTestPanel_CheckedChanged(object sender, EventArgs e)
        {
            if (openTestPanel.Checked)
                myXYPanel.Show();
            else
                myXYPanel.Hide();
        }

        public void testPanelHidden()
        {
            openTestPanel.Checked = false;
        }

        private void MIDI_control_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeMidiOutput();

            if (!myXYPanel.IsDisposed)
                myXYPanel.Close();
        }


        private bool openMidiOutput()
        {

            outDialog = new OutputDeviceDialog();

            if (OutputDevice.DeviceCount == 0)
            {
                MessageBox.Show("No MIDI output devices available.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            else
            {
                try
                {
                    outDevice = new OutputDevice(outDeviceID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

            }

            // Flag that MIDI output has successfully initialized
            midiStarted = true;
            btn_startMIDI.Text = "Stop MIDI";
            btn_startMIDI.BackColor = Color.LightCoral;

            return true;
        }


        private void closeMidiOutput()
        {
            // Flag that MIDI out will/has stop(ped)
            midiStarted = false;
            btn_startMIDI.Text = "Start MIDI";
            btn_startMIDI.BackColor = Color.DarkSeaGreen;

            try
            {
                if (outDevice != null && !outDevice.IsDisposed)
                    outDevice.Dispose();
            }
            catch (Exception e)
            {

            }

        }

        private void updateParamDisplay()
        {
            if (opt_modeMan.Checked)
            {
                // All settings manual, no accel control
                instrumentSelect.Enabled = true;
                noteSelect.Enabled = true;
                volumeSelect.Enabled = true;
                pitchBendSelect.Enabled = true;
                modSelect.Enabled = true;
            }

            else if (opt_mode1.Checked)
            {

                // X accel controls note value, Y accel controls modulation
                instrumentSelect.Enabled = true;
                noteSelect.Enabled = false;
                volumeSelect.Enabled = false;
                pitchBendSelect.Enabled = true;
                modSelect.Enabled = true;
            }
            else if (opt_mode2.Checked)
            {
                // X accel controls pitch bend, Y accel controls modulation
                instrumentSelect.Enabled = true;
                noteSelect.Enabled = true;
                volumeSelect.Enabled = true;
                pitchBendSelect.Enabled = false;
                modSelect.Enabled = false;
            }
            else if (opt_mode3.Checked)
            {
                // X accel controls pitch bend, Y accel controls volume
                instrumentSelect.Enabled = true;
                noteSelect.Enabled = true;
                volumeSelect.Enabled = false;
                pitchBendSelect.Enabled = false;
                modSelect.Enabled = true;
            }
            else
            {
                // None of the option boxes are checked?!
                return;
            }
        }

        private void opt_modeMan_CheckedChanged(object sender, EventArgs e)
        {
            updateParamDisplay();
        }

        private void opt_mode1_CheckedChanged(object sender, EventArgs e)
        {
            updateParamDisplay();
        }

        private void opt_mode2_CheckedChanged(object sender, EventArgs e)
        {
            updateParamDisplay();
        }

        private void opt_mode3_CheckedChanged(object sender, EventArgs e)
        {
            updateParamDisplay();
        }




    }
}