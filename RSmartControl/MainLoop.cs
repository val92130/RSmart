using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Collections;

namespace RSmartControl
{
   public class MainLoop
    {
        readonly Motor _motorLeft;
        readonly Motor _motorRight;
        Communication _com;
        Robot _robot;
        private SyncModule _syncModule;
        SensorsManager _sensorsManager;
        PluginManager _pluginManager;
        public MainLoop(PluginManager pluginManager)
        {
            _pluginManager = pluginManager;
            _syncModule = _pluginManager.SyncModule;
            _com = _pluginManager.CommunicationModule;

            _motorLeft = new Motor(PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D1);
            _motorRight = new Motor(PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D0);

            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;

            _com.MotorLeft = _motorLeft;
            _com.MotorRight = _motorRight;

            _com.MainLoop = this;
            _robot = new Robot(this, _motorLeft, _motorRight, _pluginManager);

            _com.Robot = _robot;

        }

       

       public Robot Robot
       {
           get
           {
               return _robot;
           }
       }

       public SyncModule SyncModule
       {
           get
           {
               return _syncModule;
           }
       }


        public void Run()
        {
            while (true)
            {
                _robot.Update();
            }
        }
    }
}
