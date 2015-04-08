using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;
using System.Collections;

namespace RSmartControl
{
    using System.Diagnostics;

    public class Robot
    {
        Motor _motorLeft, _motorRight;
        Sensor _frontSensor, _backSensor;

         Vector2 _pos, _dir;

         Box _box;

         MainLoop _mainLoop;

        private double RotationSpeed = Utility.DegreeToRadian(30);

        private Communication _com;

         public Robot(MainLoop MainLoop,Motor MotorLeft, Motor MotorRight, Sensor frontSensor, Sensor BackSensor, Communication Com)
        {
            _pos = new Vector2();
            _dir = new Vector2();

             _mainLoop = MainLoop;

             _motorLeft = MotorLeft;
             _motorRight = MotorRight;

             _frontSensor = frontSensor;
             _backSensor = BackSensor;

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

        public void TurnRight()
        {
            _motorRight.Stop(0.6);

            this._dir.X = this._dir.X * System.Math.Cos( this.RotationSpeed ) - this._dir.Y * System.Math.Sin( this.RotationSpeed );
            this._dir.Y = this._dir.X * System.Math.Sin( this.RotationSpeed ) + this._dir.Y * System.Math.Cos( this.RotationSpeed );

        }
        public void TurnLeft()
        {
            _motorLeft.Stop(0.6);
            

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

        public void Update()
        {
            MessageAnalyse(Utility.ParseQueryString(_com.GetMessage()));
            

            _motorLeft.Update();
            _motorRight.Update();

            _frontSensor.sensorBehaviour();
            _backSensor.sensorBehaviour();

            if (_motorLeft.IsStarted || _motorRight.IsStarted)
            {

                double _step = _motorRight.DutyCycle / 50;

                this._pos.X += _step * this._dir.X;
                this._pos.Y += _step * this._dir.Y;

            }

        }
    }
}
