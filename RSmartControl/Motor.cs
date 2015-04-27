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
        const double RobotHeight = 24.5;
        const int MaxSpeed = 1;
        readonly PWM _motor;
        EDirection _direction;
        readonly OutputPort _frontDirection;
        StartTimer _motorTimer;
        private StartTimer _directionTimer;
        private bool _started = false;
        public Motor( Cpu.PWMChannel motor,Cpu.Pin motorFrontDirection )
        {
            _direction = EDirection.Forward;
            _motor = new PWM( motor, 500, 0.8, false );

            _frontDirection = new OutputPort( motorFrontDirection, true );

        }

        public string DirectionString
        {
            get
            {
                switch (this.Direction)
                {
                    case EDirection.Forward:
                        return "Forward";
                    case EDirection.BackWard:
                        return "Backward";
                    case EDirection.Left:
                        return "Left";
                    case EDirection.Right:
                        return "Right";
                    default:
                        return "null";
                }
            }
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
                        _frontDirection.Write( true );
                        break;
                    case EDirection.BackWard:
                        _frontDirection.Write( false );
                        break;
                    
                }
            }
        }

        public bool IsStarted
        {
            get
            {
                return _started;              
            }
        }

        public void ReverseDirection(double interval)
        {
            if (_directionTimer != null && !_directionTimer.Tick)
            {
                return;
            }

            this.Direction = EDirection.BackWard;
            _directionTimer = new StartTimer(interval);
        }

        public void Stop()
        {
            _motor.Stop();
            _started = false;
        }

        public void Stop(double interval)
        {
            _motor.Stop();
            _started = false;
            _motorTimer = new StartTimer(interval);
        }

        public void Start(double interval)
        {
            this.Direction = EDirection.BackWard;
            _motor.Start();
            _started = true;
            _motorTimer = new StartTimer(interval);
        }

        public void Start()
        {
            _motor.Start();
            _started = true;
        }

        public double DutyCycle
        {
            get
            {
                return _motor.DutyCycle;
            }
            set
            {
                if(value >= 0 && value <= 0.8)
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
                    this.Start();
                    _motorTimer = null;
                }

                
            }

            if (_directionTimer != null)
            {
                _directionTimer.Update();
                if( _directionTimer.Tick )
                {
                     this.Direction = EDirection.Forward;
                    _directionTimer = null;
                }
            }
        }

        public static double TimeAngleRotation(double speed, double angle)
        {
            if (speed < 0 || speed > 1)
                throw new ArgumentException("Invalid speed");
            if(angle <= 0 || angle > 360)
                throw new ArgumentException("Invalid angle ");

            double p = (double)(2 * RobotHeight * System.Math.PI);
            double distanceToDo = (double)((double)(angle/(double)360) * p);
            double speedCm = (double)(1 * speed * 27.777);
            double time = ((double)(distanceToDo / speedCm) );
            return time; 
        }
    }
}
