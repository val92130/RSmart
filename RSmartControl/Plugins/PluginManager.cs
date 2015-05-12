using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class PluginManager
    {
        SyncModule _syncModule;
        Communication _communication;
        public PluginManager()
        {
            _syncModule = new SyncModule();
            _communication = new Communication();            
        }


        public SyncModule SyncModule
        {
            get
            {
                return _syncModule;
            }
        }

        public Communication CommunicationModule
        {
            get
            {
                return _communication;
            }
        }
    }
}
