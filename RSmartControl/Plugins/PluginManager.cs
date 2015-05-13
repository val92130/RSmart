using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RSmartControl
{
    public class PluginManager
    {
        private readonly SyncModule _syncModule;
        private readonly Communication _communication;
        private readonly SpeedDetectionModule _speedDetectionModule;
        private readonly SensorsManager _sensorsManager;

        public PluginManager()
        {
            _sensorsManager = new SensorsManager();
            _speedDetectionModule = new SpeedDetectionModule(this);
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

        public SpeedDetectionModule SpeedDetectionModuleModule
        {
            get
            {
                return _speedDetectionModule;
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
