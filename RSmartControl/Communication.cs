using System;
using Microsoft.SPOT;
using System.Collections;

namespace RSmartControl
{
    public class Communication
    {
        Queue _queue;
        // Maximum messages that the queue can handle
        private const int MaxMessages = 15000;

        private Motor _motorLeft, _motorRight;
        private MainLoop _mainLoop;
        private ArrayList _obstacles;
        private Robot _robot;


        public Communication()
        {
            _queue = new Queue();
            this._obstacles = new  ArrayList();
        }

        public Robot Robot
        {
            get
            {
                    return _robot;
            }
            set
            {

                    _robot = value;
            }
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

        public MainLoop MainLoop
        {
            get
            {
                return _mainLoop;
                
            }
            set { _mainLoop = value; }
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
        public void AddObstacle(Vector2 Obstacle )
        {

            bool found = false;
            double x2 = Obstacle.X;
            double y2 = Obstacle.Y;
            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (!found)
                {
                    Vector2 t = (Vector2)_obstacles[i];
                    double x = t.X;
                    
                    double y = t.Y;
                    
                    double diffX;
                    double diffY;

                    if (x > x2)
                    {
                        diffX = x - x2;
                    }
                    else
                    {
                        diffX = x2 - x;
                    }
                    if (y > y2)
                    {
                        diffY = y - y2;
                    }
                    else
                    {
                        diffY = y2 - y;
                    }
                    if ((diffX + diffY) < 50)
                    {
                        found = true;
                    }
                     
                }

            }

            // if there is not already an obstacle near to this position, we add it to the list
            if (!found)
            {
                this._obstacles.Add(Obstacle);
                Debug.Print("Obstacle Added " + this._obstacles.Count.ToString());
                return;
            }
            if (this._obstacles.Count == 0)
            {
                this._obstacles.Add(Obstacle);
                return;
            }

        }

        public ArrayList ObstacleList
        {
            get {
                lock( this._obstacles )
                {
                    return this._obstacles;
                }
            }
        }
       
         
    }
    
}
