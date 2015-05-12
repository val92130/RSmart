using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class PluginManager
    {
        private SyncModule _syncModule;
        private Communication _communication;
        private SpeedDetection _speedDetection;
        public PluginManager()
        {
            _speedDetection = new SpeedDetection();
            _syncModule = new SyncModule();
            _communication = new Communication();            
        }

        public SpeedDetection SpeedDetectionModule
        {
            get
            {
                return _speedDetection;
            }
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
