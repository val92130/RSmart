using System;
using Microsoft.SPOT;
using System.Collections;

namespace RSmartControl
{
    public class Communication
    {
        Queue _queue;

        public Communication()
        {
            _queue = new Queue();
 
        }
        public void AddMessage(String msgServer)
        {
            lock (_queue)
            {
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
            lock(_queue)
            {
                return _queue.Dequeue();  
            }
        }
        

    
    }
    
}
