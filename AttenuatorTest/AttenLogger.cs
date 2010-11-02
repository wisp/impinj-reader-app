using System;
using System.Collections.Generic;
using System.Text;

using ReaderLibrary;
using Logging;

namespace AttenuatorTest
{
    public class AttenLogger : Logger
    {


        public AttenLogger(string logFileName)
            : base(logFileName)
        {

        }

        public override void SetupAvailableOptions()
        {
            options.Add(new LoggingOption("Date", true));
            options.Add(new LoggingOption("Time", true));
            options.Add(new LoggingOption("Attenuation", true));
            options.Add(new LoggingOption("TagRate", true));
            options.Add(new LoggingOption("RunTime", false));
            options.Add(new LoggingOption("SettleTime", false));
        }


        public override string BuildStringToLog(Object thingToLog)
        {

            if (!(thingToLog is AttenStepInfo))
                throw new Exception("AttenStepInfo required");

            AttenStepInfo t = (AttenStepInfo)thingToLog;

            string toWrite = "";

            if (IsOptionEnabled("Time"))
                toWrite = toWrite + DateTime.Now.ToString("T") + ",";
            
            if (IsOptionEnabled("Date"))
                toWrite = toWrite + DateTime.Now.ToString("d") + ",";

            if (IsOptionEnabled("Attenuation"))
                toWrite = toWrite + t.attenuation + ",";

            if (IsOptionEnabled("TagRate"))
                toWrite = toWrite + t.tagRate + ",";
            
            if (IsOptionEnabled("RunTime"))
                toWrite = toWrite + t.attenRunTime + ",";

            if (IsOptionEnabled("SettleTime"))
                toWrite = toWrite + t.attenSettleTime + ",";

            return toWrite;
        }




    }
}
