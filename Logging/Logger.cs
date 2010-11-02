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

using System.Diagnostics;
using ReaderLibrary;


// Many methods are package private - only the logging forms and logging manager need access.
namespace Logging
{
    public abstract class Logger
    {
        private string name;
        private string filename;

        public Logger()
        {

        }
        
        protected List<LoggingOption> options = new List<LoggingOption>();

        public List<LoggingOption> getOptions()
        {
            return options;
        }

        public class LoggingOption
        {
            public bool enabled;
            public string name;

            public LoggingOption(string nameNew, bool enabledNew)
            {
                enabled = enabledNew;
                name = nameNew;
            }

            public void SetEnabled(bool isEnabled)
            {
                enabled = isEnabled;
            }

            public override string ToString()
            {
                return name;
            }

        }

        // initially logging disabled.
        private bool enabled = false;
        private TextWriter writer = null;


        public Logger(string newName)
        {
            name = newName;
            
            SetupAvailableOptions();
        }

        public abstract void SetupAvailableOptions();

        public abstract string BuildStringToLog(Object thingToLog);

        public string GetName()
        {
            return name;
        }

        public string GetFileName()
        {
            return filename;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetFileName(string filename)
        {
            this.filename = filename;
            if (writer != null)
            {
                writer.Close();
                writer = new StreamWriter(filename);
            }
        }

        public void LogToFile(Object thingToLog)
        {
            if (!enabled)
            {
                return;
            }

            string toWrite = BuildStringToLog(thingToLog);

            if (toWrite != "")
                writer.WriteLine(toWrite);
        }

        public void Flush()
        {
            if(writer != null)
                writer.Flush();
        }


        public void CloseLog()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer = null;
            }
            enabled = false;
        }

        public bool IsEnabled()
        {
            return enabled;
        }

        public void SetEnabled(bool isEnabled)
        {
            enabled = isEnabled;
            
            // create a log file if it doesn't exist.
            if (writer == null && enabled)
            {
                if (filename == "")
                {
                    enabled = false;
                    return;
                }
                writer = new StreamWriter(filename);
            }
            

            if (writer != null && !enabled)
            {
                writer.Close();
                writer = null;
            }
        }


        public bool IsOptionEnabled(string optionName)
        {
            System.Collections.IEnumerator myEnumerator = options.GetEnumerator();
            // for each option
            while (myEnumerator.MoveNext())
            {
                LoggingOption current = (LoggingOption)(myEnumerator.Current);
                if (current.name == optionName)
                    return current.enabled;
            }
            MessageBox.Show("Option " + optionName + " not found");
            return false;
        }

        override public string ToString()
        {
            if (enabled)
                return name + " [on]";
            else
                return name + " [off]";
        }
    }
}
