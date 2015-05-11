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
        SensorsManager _sensorsManager;
        bool _collideUnder = false;
        Vector2 _pos, _dir;
        Box _box;
        OutputPort _led = new OutputPort(Pins.GPIO_PIN_D2, false);
        MainLoop _mainLoop;

        private double RotationSpeed = Utility.DegreeToRadian(30);

        private Communication _com;

        public Robot(MainLoop MainLoop, Motor MotorLeft, Motor MotorRight, SensorsManager SensorsManager, Communication Com)
        {
            _pos = new Vector2();
            _dir = new Vector2();

            _mainLoop = MainLoop;

            _motorLeft = MotorLeft;
            _motorRight = MotorRight;

            _sensorsManager = SensorsManager;

            _com = Com;

            _dir.Y = 1;

            // We blink to tell the robot has initialized correctly
             this.BlinkLed();
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

        public void BlinkLed()
        {
            _led.Write(true);
            Thread.Sleep(100);
            _led.Write(false);
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
                return _com;
            }
            set
            {
                _com = value;
            }
        }

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
        public void TurnLeftDirect()
        {
            _motorRight.Direction = EDirection.Forward;
            _motorLeft.Direction = EDirection.Forward;

            _motorRight.ReverseDirection(0.6);

            this._dir.X = this._dir.X * System.Math.Cos(this.RotationSpeed) - this._dir.Y * System.Math.Sin(this.RotationSpeed);
            this._dir.Y = this._dir.X * System.Math.Sin(this.RotationSpeed) + this._dir.Y * System.Math.Cos(this.RotationSpeed);

        }
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
            _sensorsManager.FrontSensorLeft.sensorBehaviour();
            _sensorsManager.FrontSensorRight.sensorBehaviour();
            _sensorsManager.DownSensor.sensorBehaviour();
            _sensorsManager.BackSensor.sensorBehaviour();

            if (_motorLeft.IsStarted || _motorRight.IsStarted)
            {
                if (_sensorsManager.FrontSensorLeft.Collide  && _sensorsManager.FrontSensorRight.Collide)
                {
                    this.TurnRight();
                    _motorLeft.Direction = EDirection.Forward;
                    _motorRight.Direction = EDirection.Forward;
                    Thread.Sleep(1000);
                    this.TurnLeftDirect();
                    return;
                } 
                if (_sensorsManager.FrontSensorLeft.Collide && !_sensorsManager.FrontSensorRight.Collide)
                {
                    this.TurnRight();
                    return;
                }
                if (!_sensorsManager.FrontSensorLeft.Collide && _sensorsManager.FrontSensorRight.Collide)
                {
                    this.TurnRight();
                    return;
                }
                if (_sensorsManager.BackSensor.Collide)
                {
                    _motorLeft.Direction = EDirection.Forward;
                    _motorRight.Direction = EDirection.Forward;
                    return;
                }
                if (!_sensorsManager.DownSensor.Collide && _collideUnder)
                {
                    _collideUnder = false;
                    this.TurnLeft();
                    return;
                }
                else if(!_sensorsManager.DownSensor.Collide)
                {
                    _collideUnder = true;
                }
 
            }

        }

        public void RandomMethod()
        {
            Random r = new Random();
            int method = r.Next(1);
            if (method == 0)
            {
                this.TurnRight();
                return;
            }
            else
            {
                this.TurnLeft();
                return;

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
