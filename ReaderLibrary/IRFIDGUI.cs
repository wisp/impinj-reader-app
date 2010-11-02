using System;
namespace ReaderLibrary
{
    public interface IRFIDGUI
    {
        void AppendToDebugTextBox(string appendToTop);
        void AppendToMainTextBox(string appendToTop);
    }
}
