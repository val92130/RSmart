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
        Motor _motorLeft, _motorRight;
        Communication _com;
        Sensor _frontSensorLeft, _frontSensorRight;

       private PWM p;
        Robot _robot;
       private SyncModule _syncModule;
        public MainLoop(Communication Com, SyncModule syncModule)
        {
            _syncModule = syncModule;
            _frontSensorLeft = new Sensor(this, new AnalogInput(Cpu.AnalogChannel.ANALOG_0), EDirection.Forward);
            _frontSensorRight = new Sensor(this, new AnalogInput(Cpu.AnalogChannel.ANALOG_1), EDirection.Forward);

            _com = Com;
            _motorLeft = new Motor(PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D1);
            _motorRight = new Motor(PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D0);

            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;

            _com.MotorLeft = _motorLeft;
            _com.MotorRight = _motorRight;

            _com.MainLoop = this;
            _robot = new Robot( this,_motorLeft, _motorRight,  _frontSensorLeft, _frontSensorRight,_com);

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
