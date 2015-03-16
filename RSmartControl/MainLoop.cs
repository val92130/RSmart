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
            _motorLeft = new Motor( PWMChannels.PWM_PIN_D10, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3 );
            _motorRight = new Motor( PWMChannels.PWM_PIN_D9, Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1 );

            onboardLED = new OutputPort( Pins.ONBOARD_LED, false );
            _interval = interval;
            this.Update();
        }


        private void Update()
        {
            while( true )
            {
                actualTime = DateTime.Now.Ticks;

                if( actualTime - lastTime >= _interval * 10000000 )
                {
                    
                    if(_motorLeft.Direction == EDirection.BackWard)
                    {
                        _motorLeft.Direction = EDirection.Forward;
                    }
                    else
                    {
                        _motorLeft.Direction = EDirection.BackWard;
                    }

                    if( _motorRight.Direction == EDirection.BackWard )
                    {
                        _motorRight.Direction = EDirection.Forward;
                    }
                    else
                    {
                        _motorRight.Direction = EDirection.BackWard;
                    }
                    lastTime = actualTime;

                }
                else
                {                    
                    //onboardLED.Write( false );
                }
            }

        }
    }
}
