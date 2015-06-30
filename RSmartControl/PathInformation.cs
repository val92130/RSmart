using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class PathInformation
    {
        private double _rotationAngle;
        private int _durationMilli;
        private Vector2 _orientation, _destination;
        public PathInformation(double rotationAngle, int durationMilliSeconds, Vector2 orientation)
        {
            _rotationAngle = rotationAngle;
            _durationMilli = durationMilliSeconds;
            _orientation = orientation;
        }

        public PathInformation(double rotationAngle, int durationMilliSeconds, Vector2 endOrientation,
            Vector2 destination) : this(rotationAngle, durationMilliSeconds, endOrientation)
        {
            _destination = destination;
        }

        public Vector2 Destination
        {
            get { return _destination; }
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

        public Vector2 Orientation
        {
            get { return _orientation; }
        }
    }
}
