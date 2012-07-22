using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace BinkBonk
{
    public partial class BinkBonk_control: Form
    {
        // This class helps store info about the note that is playing.
        private class NoteInfo
        {
            public int instr;
            public int val;
        }

        private OutputDevice outDevice;

        // Is this always the proper device ID??
        private int outDeviceID = 0;

        private OutputDeviceDialog outDialog;

        private NoteInfo curNote;

        private bool midiStarted;

        private int binkBonkCounter = 0;

        enum BinkBonkStatus_e
        {
            BINK,
            BONK
        };

        BinkBonkStatus_e binkBonkStatus;

        static int VELOCITY = 127;

        public BinkBonk_control()
        {
            midiStarted = false;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            //updateParamDisplay();

            base.OnLoad(e);

            curNote = new NoteInfo();
            binkBonkStatus = BinkBonkStatus_e.BONK;
        }

        private void updateNote(int instrValue, int noteValue)
        {

            if (!midiStarted)
                return;

            // Build up new note candidate
            NoteInfo newNote = new NoteInfo();

            newNote.instr = instrValue;
            newNote.val = noteValue;

            // Keep track; should note be restarted after all channel commands?

            if (newNote.instr != curNote.instr)
            {
                // Change instrument patch (aka program)
                outDevice.Send(new ChannelMessage(ChannelCommand.ProgramChange, 0, newNote.instr, 0));
            }


            // Restart the note based on what was updated...
            outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, curNote.val, 0));
            outDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, newNote.val, VELOCITY));

            curNote = newNote;

        }

        private void killNote()
        {
            if (!midiStarted)
                return;

            outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, curNote.val, 0));

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

                updateNote((int)(this.instrumentSelectA.Value), (int)(this.noteSelectA.Value));

                killNote();

            }
           
        }


        private void BinkBonk_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeMidiOutput();

            /* // USE THE FOLLOWING IN A LARGER APPLICATION
            this.Hide();

            e.Cancel = true;
             */
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

        public void handleEPC(String EPC)
        {
            // Learn the current EPC value
            if (learn_A.Checked)
            {
                EPC_A.Text = EPC;
                learn_A.Checked = false;

            } else if (learn_B.Checked) {
                EPC_B.Text = EPC;
                learn_B.Checked = false;
            }

            //if EPC == EPC_A
            if(EPC.Equals(this.EPC_A.Text))
            {

                if (binkBonkCounter < 2)
                {
                    binkBonkCounter++;
                } 

                // If we last BONKed
                if (binkBonkStatus == BinkBonkStatus_e.BONK && binkBonkCounter > 0)
                {
                    // Bink
                    this.updateNote((int)(instrumentSelectA.Value), (int)(noteSelectA.Value));                    
                    binkBonkStatus = BinkBonkStatus_e.BINK;
                }                
            
            }

            // If EPC == EPC_B 
            if(EPC.Equals(this.EPC_B.Text))
            {
                if (binkBonkCounter > -2)
                {
                    binkBonkCounter--;
                }

                // If we last BINKed
                if (binkBonkStatus == BinkBonkStatus_e.BINK && binkBonkCounter < 0)
                {
                    // Bink
                    this.updateNote((int)(instrumentSelectB.Value), (int)(noteSelectB.Value));
                    binkBonkStatus = BinkBonkStatus_e.BONK;
                }        
            }
        }
    }
}