using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class PluginManager
    {
        private SyncModule _syncModule;
        private Communication _communication;
        private SpeedDetection _speedDetection;
        private SensorsManager _sensorsManager;
        public PluginManager()
        {
            _sensorsManager = new SensorsManager();
            _speedDetection = new SpeedDetection();
            _syncModule = new SyncModule();
            _communication = new Communication();            
        }

        public SensorsManager SensorsManager
        {
            get
            {
                return _sensorsManager;
            }
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
