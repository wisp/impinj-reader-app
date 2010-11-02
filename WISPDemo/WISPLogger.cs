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

using Logging;
using ReaderLibrary;

namespace WISPDemo
{
    public class WISPLogger : Logger
    {
        
        public WISPLogger(string logFileName) : base(logFileName)
        {

        }

        public override void SetupAvailableOptions()
        {
            options.Add(new LoggingOption("EPC", false));
            options.Add(new LoggingOption("Date", false));
            options.Add(new LoggingOption("Time", false));
            options.Add(new LoggingOption("AccessData", false));
            options.Add(new LoggingOption("SensorData", false));
            options.Add(new LoggingOption("WISPInfo", false));
        }

        public override string BuildStringToLog(Object myTagObject)
        {

            if (!(myTagObject is MyTag))
                return "";

            MyTag t = (MyTag)myTagObject;

            string toWrite = "";

            if (IsOptionEnabled("Time"))
                toWrite = toWrite + DateTime.Now.ToString("T") + ",";

            if (IsOptionEnabled("EPC")) 
                toWrite = toWrite + t.GetEpcID() + ",";
            
            if (IsOptionEnabled("Date"))
                toWrite = toWrite + DateTime.Now.ToString("d") + ",";

            if (IsOptionEnabled("AccessData"))  
                toWrite = toWrite + t.GetAccessResultData() + ",";

            if (IsOptionEnabled("WISPInfo"))
                toWrite = toWrite + t.GetTagType() + ",";

            if (IsOptionEnabled("SensorData"))
            {
                string sensorString = WriteSensorData(t);
                if(sensorString.Length > 0)
                    toWrite = toWrite + sensorString + ",";
            }
            return toWrite;
        }

        private string WriteSensorData(MyTag tag)
        {
            string temp = "";
            switch (tag.GetTagType())
            {
                case TagType.WISP_ACCELEROMETER:
                    temp = temp + "";
                    temp = temp + tag.GetAccel("x");

                    temp = temp + "\t";
                    temp = temp + tag.GetAccel("y");

                    temp = temp + "\t";
                    temp = temp + tag.GetAccel("z");

                    break;
                case TagType.WISP_TEMPERATURE:

                    temp = temp + "Temp= ";
                    temp = temp + tag.GetTemperature();

                    break;
                case TagType.WISP_SOC:
                    if (tag.GetAccessResultData().Length > 0)
                    {
                        int[] data = tag.GetSOCData();
                        for (int i = 0; i < data.Length; i++)
                        {
                            temp = temp + "ADC,";
                            temp = temp + data[i] + ",";

                            temp = temp + "temp,";
                            temp = temp + tag.socFilteredTemperature + ",";
                        }
                    }

                    break;
                default:
                    // no action for now...
                    // this could be commercial tags, etc.
                    break;
            }
            return temp;
        }
    }
}
