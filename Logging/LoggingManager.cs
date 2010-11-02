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

namespace Logging
{
    public abstract class LoggingManager
    {

        public ArrayList newTags;
        public ArrayList logList = new ArrayList();

        public LoggingManager()
        {
            DefaultCmb();
        }

        public abstract void DefaultCmb();

        public void CloseAllLogs()
        {
            // close each logger
            for (int i = 0; i < logList.Count; i++)
            {
                ((Logger)logList[i]).CloseLog();
            }
        }

        public ArrayList getLogList()
        {
            return logList;
        }

        public abstract void AddNewLogger(string text);

        public void AddNewLogger(Logger newLogger)
        {
            logList.Add(newLogger);
        }

        public Logger FindLogger(string logName)
        {
            // for each logger
            for (int i = 0; i < logList.Count; i++)
            {
                if(((Logger)logList[i]).GetName() == logName)
                    return (Logger)logList[i];
            }
            return null;
        }

        public void WriteToLog(Object thingToLog)
        {
            // for each logger
            for (int i = 0; i < logList.Count; i++)
            {
                Logger current = (Logger)logList[i];

                // write to logger
                current.LogToFile(thingToLog);

                current.Flush(); // make sure the tags actually get written to hard drive.
            }
        }

        public void WriteToLog(ArrayList thingsToLog)
        {
            System.Collections.IEnumerator myEnumerator = thingsToLog.GetEnumerator();
            // for each logger
            for (int i = 0; i < logList.Count; i++)
            {
                Logger current = (Logger)logList[i];

                // for each tag
                while (myEnumerator.MoveNext())
                {
                    // write MyTag to logger
                    current.LogToFile(myEnumerator.Current);
                }
                current.Flush(); // make sure the tags actually get written to hard drive.
            }
        }
    }
}
