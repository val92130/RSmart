using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class PathInformation
    {
        private double _rotationAngle;
        private int _durationMilli;
        public PathInformation(double rotationAngle, int durationMilliSeconds)
        {
            _rotationAngle = rotationAngle;
            _durationMilli = durationMilliSeconds;
        }

        public double Angle
        {
            get { return _rotationAngle; }
        }

        public int DurationMilli
        {
            get
            {
                return _durationMilli;
            }
        }
    }
}
