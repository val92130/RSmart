using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class Vector2
    {
        private double x, y;
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2()
        {
            this.x = 0;
            this.y = 0;
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public static double Length(Vector2 vector)
        {
            return System.Math.Sqrt((vector.X*vector.X) + (vector.Y*vector.Y));
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            Vector2 newVec = new Vector2();

            newVec.X = vector.X/Vector2.Length(vector);
            newVec.Y = vector.Y / Vector2.Length( vector );

            return newVec;
        }
    }
}
