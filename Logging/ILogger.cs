using System;
using System.Collections.Generic;
using System.Text;

using ReaderLibrary;

namespace Logging
{
    public interface ILogger
    {
        string BuildStringToLog(MyTag t);
        void SetupAvailableOptions();
    }
}
