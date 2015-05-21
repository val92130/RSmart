using System;
using Microsoft.SPOT;

namespace RSmartControl.Plugins
{
    public interface ISDCardManager
    {
        void Close();
        void Write(String msg, String fileName);
        string Read(String filename);
    }
}
