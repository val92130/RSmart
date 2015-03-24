using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RSmartControl
{
    class MainLoop
    {
        long actualTime = DateTime.Now.Ticks;
        long lastTime = DateTime.Now.Ticks;
        OutputPort onboardLED;
        int _interval;
        Motor _motorLeft, _motorRight;


        public MainLoop(int interval)
        {
            _motorLeft = new Motor(PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3);
            _motorRight = new Motor(PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);

            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;

            _interval = interval;
        }


        public void Run()
        {
            _motorLeft.Start();
            _motorRight.Start();
            while (true)
            {
                _motorLeft.Update();
                _motorRight.Update();
                actualTime = DateTime.Now.Ticks;

                // motor correction timer
                if (actualTime - lastTime >= _interval * 10000000)
                {
                    _motorRight.Stop(0.1);
                    lastTime = actualTime;
                }
                else
                {

                }
            }

        }
    }
}
