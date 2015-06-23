using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;
using System.Collections;
using System.Threading;

namespace RSmartControl
{
    using System.Diagnostics;

    public class Robot
    {
        private ERobotMode _robotMode = ERobotMode.Autonomous;
        Motor _motorLeft, _motorRight;
        bool _collideUnder = false;
        Vector2 _pos, _orientation;

        private int _directionOffset = 0;
        
        MainLoop _mainLoop;
        private readonly double _rotationSpeed = Utility.DegreeToRadian(30);
        readonly PluginManager _pluginManager;

        private DateTime now, prev = DateTime.UtcNow;

        private Communication _com;
        public Robot(MainLoop mainLoop, Motor motorLeft, Motor motorRight, PluginManager pluginManager)
        {
            _pos = new Vector2();
            _orientation = new Vector2();

            _pluginManager = pluginManager;
            _mainLoop = mainLoop;
            _com = pluginManager.CommunicationModule;
            _motorLeft = motorLeft;
            _motorRight = motorRight;

            _orientation.Y = 1;

            _pluginManager.SensorsManager.BlinkLed();
           
        }
        public Motor MotorLeft
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

        public Communication GetCommunication
        {
            get
            {
                return _pluginManager.CommunicationModule;
            }
        }
        public Vector2 Position
        {
            get
            {
                return _pos;
            }
        }

        public Vector2 Orientation
        {
            get
            {
                return _orientation;
            }
        }

        public int DirectionOffset
        {
            get { return _directionOffset; }
            set
            {
                if (value >= 100 || value <= -100)
                {
                    throw new Exception("Direction offset has to be between -100 and 100");
                }

                if (value >= 0)
                {
                    _motorLeft.DutyCycle = Motor.MaxDutyCycle - (double)(Motor.MaxDutyCycle * (double)(((double)value / (double)100)));
                    _motorRight.DutyCycle = Motor.MaxDutyCycle;
                }
                if (value <= 0)
                {
                    int v = value * -1;
                    _motorLeft.DutyCycle = Motor.MaxDutyCycle;
                    _motorRight.DutyCycle = Motor.MaxDutyCycle - (double)(Motor.MaxDutyCycle * (double)(((double)v / (double)100)));
                }
            }
        }

        /// <summary>
        /// Reverse the direction of the robot for a given time
        /// </summary>
        /// <param name="time"></param>
        public void Reverse(int time)
        {
            if (_motorLeft.Direction == EDirection.Forward && _motorRight.Direction == EDirection.Forward)
            {
                _motorRight.Direction = EDirection.BackWard;
                _motorLeft.Direction = EDirection.BackWard;
                _motorRight.Start();
                _motorLeft.Start();
                Thread.Sleep(time);
                _motorRight.Direction = EDirection.Forward;
                _motorLeft.Direction = EDirection.Forward;
            }
            if (_motorLeft.Direction == EDirection.BackWard && _motorRight.Direction == EDirection.BackWard)
            {
                _motorRight.Direction = EDirection.Forward;
                _motorLeft.Direction = EDirection.Forward;
                _motorRight.Start();
                _motorLeft.Start();
                Thread.Sleep(time);
                _motorRight.Direction = EDirection.BackWard;
                _motorLeft.Direction = EDirection.BackWard;
            }
        }


        /// <summary>
        /// Go backward for a few milliseconds then turn left
        /// </summary>
        public void  TurnLeft()
        {
            _motorRight.Direction = EDirection.BackWard;
            _motorLeft.Direction = EDirection.BackWard;

            Thread.Sleep(700);
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorRight.ReverseDirection(0.6);

            this._orientation.X = this._orientation.X * System.Math.Cos( this._rotationSpeed ) - this._orientation.Y * System.Math.Sin( this._rotationSpeed );
            this._orientation.Y = this._orientation.X * System.Math.Sin( this._rotationSpeed ) + this._orientation.Y * System.Math.Cos( this._rotationSpeed );

        }

        /// <summary>
        /// Turn left without going backward before
        /// </summary>
        public void TurnLeftDirect()
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorRight.ReverseDirection(0.6);

            this._orientation.X = this._orientation.X * System.Math.Cos(this._rotationSpeed) - this._orientation.Y * System.Math.Sin(this._rotationSpeed);
            this._orientation.Y = this._orientation.X * System.Math.Sin(this._rotationSpeed) + this._orientation.Y * System.Math.Cos(this._rotationSpeed);

        }
        /// <summary>
        /// Go backward for a few milliseconds then turn right
        /// </summary>
        public void TurnRight()
        {
            _motorRight.Direction = EDirection.BackWard;
            _motorLeft.Direction = EDirection.BackWard;

            Thread.Sleep(700);

            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorLeft.ReverseDirection( 0.6 );
            
            this._orientation.X = this._orientation.X * System.Math.Cos( -this._rotationSpeed ) - this._orientation.Y * System.Math.Sin( -this._rotationSpeed );
            this._orientation.Y = this._orientation.X * System.Math.Sin( -this._rotationSpeed ) + this._orientation.Y * System.Math.Cos( -this._rotationSpeed );
        }

        public void FollowPath(ArrayList pathInformationList)
        {
            _motorLeft.Start();
            _motorRight.Start();

            foreach (object path in pathInformationList)
            {
                
                PathInformation p = path as PathInformation;
                Thread.Sleep(p.DurationMilli);
                this.TurnAngle(p.Angle);
            }
        }

        public void TurnAngle(double angle)
        {
            double timeInSec = (angle / 360) * 5.25; // In one sec the robot rotates from 75 deg

            if(angle > 0)
            {
                TurnRight(timeInSec);
            }
            else
            {
                TurnLeft(-timeInSec);
            }
        }

        public void OrientateTo(Vector2 destination)
        {
            double newX = this.Position.X + this.Orientation.X;
            double newY = this.Position.Y + this.Orientation.Y;


            double deltaY = destination.Y - newY;
            double deltaX = destination.X - newX;

            int angleInDegrees = (int)(System.Math.Atan((deltaY / deltaX) * 180 / System.Math.PI));

            TurnAngle(angleInDegrees);
        }

        public void TurnRight(double timeInSeconds)
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorLeft.Start();
            _motorRight.Stop();

            Thread.Sleep((int)(timeInSeconds * 1000));

            _motorLeft.Stop();
            _motorRight.Stop();
        }

        public void TurnLeft(double timeInSeconds)
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorLeft.Stop();
            _motorRight.Start();

            Thread.Sleep((int)(timeInSeconds * 1000));

            _motorLeft.Stop();
            _motorRight.Stop();
        }

        public void GoForward(double timeInSeconds)
        {
            _motorLeft.Direction = EDirection.Forward;
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Start();
            _motorRight.Start();
            Thread.Sleep((int)((double)1000 * timeInSeconds));
            _motorLeft.Stop();
            _motorRight.Stop();
        }
      

        public void GoForward()
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;
        }

        public void GoBackward()
        {
            _motorRight.Direction = EDirection.BackWard;
            _motorLeft.Direction = EDirection.BackWard;
        }

        public void Behavior()
        {
            _pluginManager.SensorsManager.FrontSensorLeft.sensorBehaviour(this);
            _pluginManager.SensorsManager.FrontSensorRight.sensorBehaviour(this);
            _pluginManager.SensorsManager.DownSensor.sensorBehaviour(this);
            _pluginManager.SensorsManager.BackSensor.sensorBehaviour(this);


            if (_motorLeft.IsStarted || _motorRight.IsStarted)
            {
                if (_pluginManager.SensorsManager.FrontSensorLeft.Collide && _pluginManager.SensorsManager.FrontSensorRight.Collide)
                {
                    
                    if (_robotMode == ERobotMode.Manual)
                    {
                        _pluginManager.BehaviourControl.OnObstacleFront();
                    }
                    else
                    {
                        this.TurnRight();
                    }
                    
                    return;
                } 
                // If collide front-left but not front-right
                if (_pluginManager.SensorsManager.FrontSensorLeft.Collide && !_pluginManager.SensorsManager.FrontSensorRight.Collide)
                {
                    if( _robotMode == ERobotMode.Manual )
                    {
                        _pluginManager.BehaviourControl.OnObstacleFrontLeft();
                    }
                    else
                    {
                        this.TurnRight();
                    }

                    return;
                }

                // If collide front-right but not front-left
                if (!_pluginManager.SensorsManager.FrontSensorLeft.Collide && _pluginManager.SensorsManager.FrontSensorRight.Collide)
                {
                    
                    if( _robotMode == ERobotMode.Manual )
                    {
                        _pluginManager.BehaviourControl.OnObstacleFrontRight();
                    }
                    else
                    {
                        this.TurnRight();
                    }
      
                    return;
                }

                // If collide in the back
                if (_pluginManager.SensorsManager.BackSensor.Collide && _motorLeft.Direction == EDirection.BackWard && _motorRight.Direction == EDirection.BackWard)
                {
                    _pluginManager.BehaviourControl.OnObstacleBack();
                    _motorLeft.Direction = EDirection.Forward;
                    _motorRight.Direction = EDirection.Forward;
                    return;
                }

                // If we don't collide on the down
                if (!_pluginManager.SensorsManager.DownSensor.Collide && _collideUnder)
                {
                    _collideUnder = false;
                    this.TurnLeft();
                    return;
                }
                else if (!_pluginManager.SensorsManager.DownSensor.Collide)
                {
                    _collideUnder = true;
                }
 
            }

        }


        public void Update()
        {
            _motorLeft.Update();
            _motorRight.Update();

            Behavior();

            now = DateTime.UtcNow;

            TimeSpan t = now - prev;

            if (t.Seconds >= 1)
            {
                prev = DateTime.UtcNow;
                if (_motorLeft.IsStarted || _motorRight.IsStarted)
                {
                    if (_motorLeft.Direction == EDirection.Forward && _motorRight.Direction == EDirection.Forward)
                    {
                        //this._pos.X += _pluginManager.SpeedDetectionModuleModule.SpeedCm*this._orientation.X;
                        //this._pos.Y += _pluginManager.SpeedDetectionModuleModule.SpeedCm*this._orientation.Y;

                        this._pos.X += 46 * this._orientation.X;
                        this._pos.Y += 46 * this._orientation.Y;
                    }
                    else
                    {
                       // this._pos.X -= _pluginManager.SpeedDetectionModuleModule.SpeedCm*this._orientation.X;
                        //this._pos.Y -= _pluginManager.SpeedDetectionModuleModule.SpeedCm*this._orientation.Y;

                        this._pos.X -= 46 * this._orientation.X;
                        this._pos.Y -= 46 * this._orientation.Y;
                    }
                }
            }

        }
    }
}
