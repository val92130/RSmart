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
        long actualTime = DateTime.Now.Ticks;
        long lastTime = DateTime.Now.Ticks;
        //OutputPort onboardLED;
        int _interval;
        Motor _motorLeft, _motorRight;
        Communication _com;
        Sensor _frontSensor, _backSensor;

        public MainLoop(int interval, Communication Com)
        {
            _frontSensor = new Sensor(this, new AnalogInput(Cpu.AnalogChannel.ANALOG_0), EDirection.Forward);
            _backSensor = new Sensor(this, new AnalogInput(Cpu.AnalogChannel.ANALOG_1), EDirection.BackWard);
            _com = Com;
            _motorLeft = new Motor(PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3);
            _motorRight = new Motor(PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);

            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;

            _interval = interval;

            _com.MotorLeft = _motorLeft;
            _com.MotorRight = _motorRight;
        }


        public void Run()
        {

           // _motorLeft.Start();
     //       _motorRight.Start();
            while (true)
            {
                double t = _backSensor.AnalogInput.Read();
                MessageAnalyse(Utility.ParseQueryString(_com.GetMessage()));

                _motorLeft.Update();
                _motorRight.Update();
                actualTime = DateTime.Now.Ticks;
                _frontSensor.sensorBehaviour();
                _backSensor.sensorBehaviour();

            }

        }

        public Motor  MotorLeft
        {
            get
            {
                return _motorLeft;
            }
            set
            {
                _motorLeft = value;
            }
        }

        public Motor MotorRight
        {
            get
            {
                return _motorRight;
            }
            set
            {
                _motorRight = value;
            }
        }

        public void TurnRight()
        {
            _motorRight.Stop(0.6);
           
        }
        public void TurnLeft()
        {
            _motorLeft.Stop(0.6);
            
        }
        public void GoForward()
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;
        }
        public void GoBackward()
        {
            _motorRight.Direction= EDirection.BackWard;
            _motorLeft.Direction = EDirection.BackWard;
        }

        public void MessageAnalyse(Hashtable nvc)
        {
            if (nvc == null)
                return;
            foreach (DictionaryEntry entry in nvc)
            {
                switch ((string)entry.Key)
                {
                    case "Start":
                        if ((string)entry.Value == "true")
                        {
                            _motorLeft.Start();
                            _motorRight.Start();
                        }
                        break;
                    case "Stop":
                        if ((string)entry.Value == "true")
                        {
                            _motorLeft.Stop();
                            _motorRight.Stop();
                        }
                        break;

                    case "Rotate" :
                        if ((string)entry.Value == "left")
                        {
                            _motorLeft.Stop(Motor.timeAngleRotation(_motorLeft.DutyCycle, 90));
                        }

                        if ((string)entry.Value == "right")
                        {
                            _motorRight.Stop(Motor.timeAngleRotation(_motorLeft.DutyCycle, 90));
                        }
                        break;
                    case "Speed":
                        if(entry.Value == null)
                            return;
                        int speed;
                        try
                        {
                                speed = Convert.ToInt32((string)entry.Value);
                        } catch(Exception e)
                        {
                            Debug.Print(e.ToString());
                            return;
                        }
                        
                        if (speed < 0 || speed > 100)
                            return;
                        _motorLeft.DutyCycle = (double)((double)(speed) / (double)(100));
                        _motorRight.DutyCycle = (double)((double)(speed) / (double)(100));
                        break;
                    case "Forward":
                        if ((string)entry.Value == "true")
                        {
                            GoForward();
                        }
                        break;
                    case "Backward":
                        if ((string)entry.Value == "true")
                        {
                            GoBackward();
                        }
                        break;
                    case "Right":
                        if ((string)entry.Value == "true")
                        {
                            TurnRight();
                        }
                        break;
                    case "Left":
                        if ((string)entry.Value == "true")
                        {
                            TurnLeft();
                        }
                        break;


                }
            }
        }


    }
}
