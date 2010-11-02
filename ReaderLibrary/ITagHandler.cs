using System;
using System.Collections.Generic;
using System.Text;

namespace ReaderLibrary
{
    public interface ITagHandler
    {
        void HandleTagReceived(MyTag tag);
    }
}
