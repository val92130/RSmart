using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RSmartControl
{
    public class Sensor
    {
        AnalogInput _sanalog;
        int counter = 0;
        private bool _collide;
        public Sensor(AnalogInput Sanalog)
        {
            _sanalog = Sanalog;
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

        public void sensorBehaviour(Robot robot)
        {
            
            double distance = _sanalog.Read();

            if (distance >= 0.5)
            {
                counter++;

                // if the sensor detects an obstacle for more than 10 frames
                if (counter > 10)

                {
                   
                    robot.GetCommunication.AddObstacle(new Vector2(robot.Position.X + robot.Orientation.X, robot.Position.Y + robot.Orientation.Y));
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
