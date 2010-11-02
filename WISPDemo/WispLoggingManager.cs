using System;
using System.Collections.Generic;
using System.Text;

using Logging;

namespace WISPDemo
{
    class WispLoggingManager : LoggingManager
    {

        public override void DefaultCmb()
        {
            logList.Add(new WISPLogger("Log ID"));
        }


        public override void AddNewLogger(string text)
        {
            AddNewLogger(new WISPLogger(text));
        }
    }
}
