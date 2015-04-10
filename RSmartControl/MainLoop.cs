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
        Sensor _frontSensor, _backSensor;

        Robot _robot;
        public MainLoop(Communication Com)
        {
            _frontSensor = new Sensor(this, new AnalogInput(Cpu.AnalogChannel.ANALOG_0), EDirection.Forward);
            _backSensor = new Sensor(this, new AnalogInput(Cpu.AnalogChannel.ANALOG_1), EDirection.BackWard);

            _com = Com;
            _motorLeft = new Motor(PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D1);
            _motorRight = new Motor(PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D0);

            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;

            _com.MotorLeft = _motorLeft;
            _com.MotorRight = _motorRight;

            _com.MainLoop = this;
            _robot = new Robot( this,_motorLeft, _motorRight,  _frontSensor, _backSensor, _com);

        }

       public Robot Robot
       {
           get
           {
               return _robot;
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
