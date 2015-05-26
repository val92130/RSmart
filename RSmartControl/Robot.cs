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
        Motor _motorLeft, _motorRight;
        bool _collideUnder = false;
        Vector2 _pos, _dir;
        
        MainLoop _mainLoop;
        private double RotationSpeed = Utility.DegreeToRadian(30);
        PluginManager _pluginManager;

        private Communication _com;
        public Robot(MainLoop MainLoop, Motor MotorLeft, Motor MotorRight, PluginManager pluginManager)
        {
            _pos = new Vector2();
            _dir = new Vector2();

            _pluginManager = pluginManager;
            _mainLoop = MainLoop;
            _com = pluginManager.CommunicationModule;
            _motorLeft = MotorLeft;
            _motorRight = MotorRight;

            _dir.Y = 1;

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

        public Vector2 Direction
        {
            get
            {
                return _dir;
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

            this._dir.X = this._dir.X * System.Math.Cos( this.RotationSpeed ) - this._dir.Y * System.Math.Sin( this.RotationSpeed );
            this._dir.Y = this._dir.X * System.Math.Sin( this.RotationSpeed ) + this._dir.Y * System.Math.Cos( this.RotationSpeed );

        }

        /// <summary>
        /// Turn left without going backward before
        /// </summary>
        public void TurnLeftDirect()
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorRight.ReverseDirection(0.6);

            this._dir.X = this._dir.X * System.Math.Cos(this.RotationSpeed) - this._dir.Y * System.Math.Sin(this.RotationSpeed);
            this._dir.Y = this._dir.X * System.Math.Sin(this.RotationSpeed) + this._dir.Y * System.Math.Cos(this.RotationSpeed);

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
            
            this._dir.X = this._dir.X * System.Math.Cos( -this.RotationSpeed ) - this._dir.Y * System.Math.Sin( -this.RotationSpeed );
            this._dir.Y = this._dir.X * System.Math.Sin( -this.RotationSpeed ) + this._dir.Y * System.Math.Cos( -this.RotationSpeed );

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
                    this.TurnRight();
                    return;
                } 
                // If collide front-left but not front-right
                if (_pluginManager.SensorsManager.FrontSensorLeft.Collide && !_pluginManager.SensorsManager.FrontSensorRight.Collide)
                {
                    this.TurnRight();
                    return;
                }

                // If collide front-right but not front-left
                if (!_pluginManager.SensorsManager.FrontSensorLeft.Collide && _pluginManager.SensorsManager.FrontSensorRight.Collide)
                {
                    this.TurnRight();
                    return;
                }

                // If collide in the back
                if (_pluginManager.SensorsManager.BackSensor.Collide && _motorLeft.Direction == EDirection.BackWard && _motorRight.Direction == EDirection.BackWard)
                {
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

            if (_motorLeft.IsStarted || _motorRight.IsStarted)
            {

                double _step = _motorRight.DutyCycle / 100;

                if(_motorLeft.Direction == EDirection.Forward && _motorRight.Direction == EDirection.Forward)
                {
                    this._pos.X += _step * this._dir.X;
                    this._pos.Y += _step * this._dir.Y;
                }
                else
                {
                    this._pos.X -= _step * this._dir.X;
                    this._pos.Y -= _step * this._dir.Y;
                }
                

            }

        }
    }
}
