using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RSmartControl
{
    public class Sensor
    {
        MainLoop _mainloop;
        AnalogInput _sanalog;
        int counter = 0;

        private EDirection _direction;
        public Sensor(MainLoop mainLoop, AnalogInput Sanalog, EDirection direction)
        {
            _mainloop = mainLoop;
            _sanalog = Sanalog;
            _direction = direction;
        }

        public AnalogInput AnalogInput
        {
            get
            {
                return _sanalog;              
            }
            set
            {
                _sanalog = value;
            }
        }

        public void sensorBehaviour()
        {
            double distance = _sanalog.Read();

            if (distance >= 0.5)
            {
                counter++;

                // if the sensor detects an obstacle for more than 10 frames
                if (counter > 10)
                {
                    switch (this._direction)
                    {
                        case EDirection.Forward:
                            if( _mainloop.Robot.MotorRight.Direction != EDirection.BackWard && _mainloop.Robot.MotorLeft.Direction != EDirection.BackWard )
                            {

                                _mainloop.Robot.MotorLeft.Stop();
                                _mainloop.Robot.MotorRight.Stop();
                            }
                            break;
                        case EDirection.BackWard:
                            if( _mainloop.Robot.MotorRight.Direction != EDirection.Forward && _mainloop.Robot.MotorLeft.Direction != EDirection.Forward )
                            {
                                _mainloop.Robot.MotorLeft.Stop();
                                _mainloop.Robot.MotorRight.Stop();
                            }
                            break;
                    }
                }

            }
            else
            {
                counter = 0;
            }

        }
    }
}