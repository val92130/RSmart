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
        Sensor _frontSensor, _backSensor, _leftSensor, _rightSensor;

         Vector2 _pos, _dir;

         Box _box;

         MainLoop _mainLoop;

        private double RotationSpeed = Utility.DegreeToRadian(30);

        private Communication _com;

         public Robot(MainLoop MainLoop,Motor MotorLeft, Motor MotorRight, Sensor frontSensor, Sensor BackSensor, Sensor LeftSensor, Sensor RightSensor, Communication Com)
        {
            _pos = new Vector2();
            _dir = new Vector2();

             _mainLoop = MainLoop;

             _motorLeft = MotorLeft;
             _motorRight = MotorRight;

             _frontSensor = frontSensor;
             _backSensor = BackSensor;
             _leftSensor = LeftSensor;
             _rightSensor = RightSensor;

             _com = Com;

             _dir.Y = 1;
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

        public void TurnRight()
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
        public void TurnLeft()
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

                    case "Rotate":
                        if ((string)entry.Value == "left")
                        {
                            _motorLeft.Stop(Motor.TimeAngleRotation(_motorLeft.DutyCycle, 90));
                        }

                        if ((string)entry.Value == "right")
                        {
                            _motorRight.Stop(Motor.TimeAngleRotation(_motorLeft.DutyCycle, 90));
                        }
                        break;
                    case "Speed":
                        if (entry.Value == null)
                            return;
                        int speed;
                        try
                        {
                            speed = Convert.ToInt32((string)entry.Value);
                        }
                        catch (Exception e)
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

       
        public void Behavior()
        {
            _frontSensor.sensorBehaviour();
            //_backSensor.sensorBehaviour();
            _rightSensor.sensorBehaviour();
            _leftSensor.sensorBehaviour();

            if (_motorLeft.IsStarted || _motorRight.IsStarted)
            {
                if (_frontSensor.Collide  && !_leftSensor.Collide && !_rightSensor.Collide)
                {
                    this.RandomMethod();
                    return;
                }
                //if (_frontSensor.Collide && !_leftSensor.Collide && _rightSensor.Collide)
                //{
                //    this.TurnLeft();
                //    return;
                //}
                //if (_frontSensor.Collide && _leftSensor.Collide && _rightSensor.Collide)
                //{
                //    this.TurnRight();
                //    return;
                //}
                //if (!_frontSensor.Collide && !_leftSensor.Collide && !_rightSensor.Collide)
                //{
                //    _motorLeft.ReverseDirection(0.5);
                //    _motorRight.ReverseDirection(0.5);
                //    return;

                //}
                //if (!_frontSensor.Collide && _leftSensor.Collide && !_rightSensor.Collide)
                //{
                //    _motorLeft.ReverseDirection(0.2);
                //    _motorRight.ReverseDirection(0.2);
                //    this.TurnRight();
                //    return;

                //}
                //if (!_frontSensor.Collide && !_leftSensor.Collide && _rightSensor.Collide)
                //{
                //    _motorLeft.ReverseDirection(0.2);
                //    _motorRight.ReverseDirection(0.2);
                //    this.TurnLeft();
                //    return;

                //}
 
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
            MessageAnalyse(Utility.ParseQueryString(_com.GetMessage()));
            
            _motorLeft.Update();
            _motorRight.Update();

            Behavior();

            if (_motorLeft.IsStarted || _motorRight.IsStarted)
            {

                double _step = _motorRight.DutyCycle / 100;

                this._pos.X += _step * this._dir.X;
                this._pos.Y += _step * this._dir.Y;

            }

        }
    }
}
