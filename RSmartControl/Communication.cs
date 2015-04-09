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


        public Communication()
        {
            _queue = new Queue();
            this._obstacles = new  ArrayList();

            
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
        public void AddMessage(String msgServer)
        {

            lock (_queue)
            {
                // if there are too many messages pending, we leave
                if( _queue.Count >= MaxMessages )
                {
                    return;
                }

                if(Utility.IsQueryValid(msgServer))
                {
                    //Debug.Print( "Valid message : " + msgServer );
                    _queue.Enqueue( msgServer );                  
                }
                else
                {
                    //Debug.Print( "Invalid message " );
                }
                //Debug.Print( "Queue count : " + _queue.Count );

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
        public Object GetMessage()
        {
            object o = null;
            lock(_queue)
            {
                if(_queue.Count != 0)
                {
                    o = _queue.Dequeue();
                    return o;
                }
                else
                {
                    return null;
                }
                              
            }
        }
       
         
    }
    
}
