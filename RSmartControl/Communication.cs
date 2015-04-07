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

        public Communication()
        {
            _queue = new Queue();
 
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
        public Object GetMessage()
        {
            object o = null;
            lock(_queue)
            {
                if(_queue.Count != 0)
                {
                    o = _queue.Dequeue();
                    Debug.Print((string)o);
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
