using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RSmartControl
{
    public class Motor
    {
        PWM _motor;
        EDirection _direction;
        OutputPort _frontDirection, _backDirection;
        CustomTimer _motorTimer;
        public Motor( Cpu.PWMChannel motor,Cpu.Pin motorFrontDirection, Cpu.Pin motorBackDirection )
        {
            _direction = EDirection.Forward;
            _motor = new PWM( motor, 500, 0.5, false );

            _frontDirection = new OutputPort( motorFrontDirection, false );
            _backDirection = new OutputPort( motorBackDirection, true );




            _motor.Start();
        }

        public EDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                switch(value)
                {
                    case EDirection.Forward :
                        _frontDirection.Write( false );
                        _backDirection.Write( true );
                        break;
                    case EDirection.BackWard:
                        _frontDirection.Write( true );
                        _backDirection.Write( false );
                        break;
                }
            }
        }

        public void Stop()
        {
            _motor.Stop();
        }

        public void Stop(int interval)
        {
            _motor.Stop();
            _motorTimer = new CustomTimer( interval );
        }

        public void Start()
        {
            _motor.Start();
        }

        public double DutyCycle
        {
            get
            {
                return _motor.DutyCycle;
            }
            set
            {
                if(value >= 0 && value <= 1)
                {
                    _motor.DutyCycle = value;
                }
            }
        }


        public void Update()
        {
            if(_motorTimer != null)
            {
                _motorTimer.Update();
                if(_motorTimer.Tick)
                {
                    _motor.Start();
                }
            }
        }
    }
}
