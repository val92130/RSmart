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
        private bool _collide;

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

        public bool Collide
        {
            get
            {
                return _collide;
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
                    _mainloop.Robot.GetCommunication.AddObstacle(new Vector2(_mainloop.Robot.Position.X + _mainloop.Robot.Direction.X, _mainloop.Robot.Position.Y + _mainloop.Robot.Direction.Y));
                    _collide = true;
                }

            }
            else
            {
                _collide = false;
                counter = 0;
            }

        }
       
    }
}
