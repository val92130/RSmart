using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using RSmartControl.Robot_Behaviours;

namespace RSmartControl
{
    public class PluginManager
    {
        private readonly SyncModule _syncModule;
        private readonly Communication _communication;
        private readonly SpeedDetectionModule _speedDetectionModule;
        private readonly SensorsManager _sensorsManager;
        private readonly RobotBehaviourPlugin _robotBehaviourPlugin;
        private readonly SDCardManager _sdCardManager;
        private readonly BehaviourControl _behaviourControl;

        public PluginManager()
        {
            _sdCardManager = new SDCardManager();
            _robotBehaviourPlugin = new RobotBehaviourPlugin();
            _behaviourControl = new BehaviourControl(this);          
            _sensorsManager = new SensorsManager();
            _speedDetectionModule = new SpeedDetectionModule(this);
            _syncModule = new SyncModule();
            _communication = new Communication();    
            
        }

        public BehaviourControl BehaviourControl
        {
            get { return _behaviourControl; }
        }

        public SDCardManager SdCardManager
        {
            get
            {
                return _sdCardManager;
            }
        }

        public SensorsManager SensorsManager
        {
            get
            {
                return _sensorsManager;
            }
        }

        public RobotBehaviourPlugin RobotBehaviourPlugin
        {
            get
            {
                return _robotBehaviourPlugin;
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
