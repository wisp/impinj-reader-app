using System;
using System.Collections.Generic;
using System.Text;

using Logging;

namespace AttenuatorTest
{
    public class AttenLoggingManager : LoggingManager
    {


        public override void DefaultCmb()
        {
            logList.Add(new AttenLogger("Log ID"));
        }


        public override void AddNewLogger(string text)
        {
            AddNewLogger(new AttenLogger(text));
        }



    }
}
