﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Lib
{
        public class Vector2
        {
            private double x, y;
            public Vector2( double x, double y )
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

            public static double Radius(Vector2 position, Vector2 direction, Vector2 destination)
            {
                double dist = Distance( position, destination ) / 2;
                double angle = GetAngle(direction, destination);

                double newAngle = 1.5707963268 - angle; // equals 90 deg

                double radius = dist/Math.Cos(newAngle);
                return radius;

            }

            public static double DegreeToRadian(double rad)
            {
                return (Math.PI*rad)/180.0;
            }
            public static double RadianToDegree(double rad)
            {
                return (rad * 180.0) / Math.PI;
            }
            public static double ArcDistance(Vector2 p1, Vector2 p2, Vector2 direction)
            {
                double radius = Radius(p1, direction, p2);
                double Circonferance = radius * 2 * Math.PI;
                double newAngle = 1.5707963268 - (GetAngle(direction,p2));
                double alpha =  3.1415926536 - (newAngle + 1.5707963268);
                alpha = RadianToDegree(alpha);
                double acrDistance = (double)(((2 * alpha) / 360) * Circonferance);
                return acrDistance;
            }
            public static double TimeBetweenPoints(double Dist, Vector2 p1, Vector2 p2, Vector2 direction)
            {
                Dist = ArcDistance(p1, p2, direction);
                double timeBetweenPoints = Dist / 46;
                return timeBetweenPoints;

            }

            public static double Distance(Vector2 a, Vector2 b)
            {
                return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            }
            public static float GetAngle( Vector2 a, Vector2 b )
            {
                a = Normalize( a );
                b = Normalize( b );

                double dotProduct = (a.X * b.X) + (a.Y * b.Y);             
                return (float)(Math.Acos( dotProduct ));
            }

            public static double Length( Vector2 vector )
            {
                return System.Math.Sqrt( (vector.X * vector.X) + (vector.Y * vector.Y) );
            }

            public static Vector2 Normalize( Vector2 vector )
            {
                Vector2 newVec = new Vector2();
                newVec.X = vector.X / Vector2.Length( vector );
                newVec.Y = vector.Y / Vector2.Length( vector );
                return newVec;
            }

            public static Vector2 operator +( Vector2 v1, Vector2 v2 )
            {
                Vector2 nv = new Vector2();
                nv.X = v1.X + v2.X;
                nv.Y = v1.Y + v2.Y;
                return nv;
            }

            public static Vector2 operator -( Vector2 v1, Vector2 v2 )
            {
                Vector2 nv = new Vector2();
                nv.X = v1.X - v2.X;
                nv.Y = v1.Y - v2.Y;
                return nv;
            }

            public override string ToString()
            {
                return "X : " + this.X.ToString() + "Y : " + this.Y.ToString();
            }
        }
}
