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
                Debug.Print( msgServer );
                _queue.Enqueue(msgServer);
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
