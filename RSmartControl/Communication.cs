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
                _queue.Enqueue(msgServer);
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
