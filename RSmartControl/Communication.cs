using System;
using Microsoft.SPOT;
using System.Collections;

namespace RSmartControl
{
    public class Communication
    {
        Queue _queue;
        ArrayList _obstacleList;
        // Maximum messages that the queue can handle
        private const int MaxMessages = 15000;

        private Motor _motorLeft, _motorRight;
        private MainLoop _mainLoop;

        public Communication()
        {
            _queue = new Queue();
            _obstacleList = new ArrayList();
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
                    Debug.Print( "Valid message : " + msgServer );
                    _queue.Enqueue( msgServer );                  
                }
                else
                {
                    Debug.Print( "Invalid message " );
                }
                Debug.Print( "Queue count : " + _queue.Count );

            }
        }
        public void AddObstacle(Vector2 Obstacle )
        {
            if (_obstacleList.Count ==0)
            {
                _obstacleList.Add(Obstacle);
                return;
            }

            foreach (Object o in _obstacleList)
            {
                Vector2 t = (Vector2)o;
                double x = t.X;
                double x2 = Obstacle.X;

                double y = t.Y;
                double y2 = Obstacle.Y;
                double diffX ;
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
                if ((diffX + diffY) > 10)
                {
                    _obstacleList.Add(Obstacle);
                    Debug.Print("Obstacle Added " + _obstacleList.Count.ToString());
                    return;
                }

                
                
            }

        }

        public ArrayList ObstacleList
        {
            get {
                lock (_obstacleList)
                {
                    return _obstacleList;
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
