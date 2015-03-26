using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Collections;

namespace RSmartControl
{
    class MainLoop
    {
        long actualTime = DateTime.Now.Ticks;
        long lastTime = DateTime.Now.Ticks;
        //OutputPort onboardLED;
        int _interval;
        Motor _motorLeft, _motorRight;
        Communication _com;

        public MainLoop(int interval, Communication Com)
        {
            _com = Com;
            _motorLeft = new Motor(PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3);
            _motorRight = new Motor(PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);

            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;

            _interval = interval;
        }


        public void Run()
        {
           // _motorLeft.Start();
     //       _motorRight.Start();
            while (true)
            {

                MessageAnalyse(Utility.ParseQueryString(_com.GetMessage()));

                _motorLeft.Update();
                _motorRight.Update();
                actualTime = DateTime.Now.Ticks;

                // motor correction timer
                //if (actualTime - lastTime >= _interval * 10000000)
                //{
                //    _motorRight.Stop(0.1);
                //    lastTime = actualTime;
                //}
                //else
                //{

                //}
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
