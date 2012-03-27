namespace MIDI_Control_Demo
{
    public class Midi
    {
        private MIDI_control myMidiControlDemo;

        public Midi()
        {
            myMidiControlDemo = new MIDI_control();
        }

        public bool IsMidiConfigOpen()
        {
            return !myMidiControlDemo.IsDisposed;
        }

        ///<summary>
        /// If midi configuration window is disposed, create a new one
        ///</summary>
        public void ReOpenMidiConfig()
        {
            if (myMidiControlDemo.IsDisposed)
            {
                myMidiControlDemo = new MIDI_control();
            }

            myMidiControlDemo.Show();
        }

        public void DisposeMidiConfig()
        {

            if (!myMidiControlDemo.IsDisposed)
            {
                myMidiControlDemo.Close();
            }
        }

        /// <summary>
        /// Updates the MIDI note value, pitch bend, modulation, and other parameters 
        /// based on acceleration data
        /// </summary>

        public void updateMidi(double xAccel, double yAccel, double zAccel)
        {
            myMidiControlDemo.updateAccelValues(xAccel, yAccel, zAccel);
        }

    }
}
