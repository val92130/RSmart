using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class DebugLog
    {
        Queue<String> _debugQueue;
        public DebugLog()
        {
            _debugQueue = new Queue<String>();
        }

        public void Write(string log)
        {
            lock(_debugQueue)
            {
                _debugQueue.Enqueue(log);
                Debug.Print(log);
            }
        }

        public int Count
        {
            get
            {
                lock(_debugQueue)
                {
                    return _debugQueue.Count;
                }
            }
        }

        public string Get
        {
            get
            {
                lock(_debugQueue)
                {
                    if(_debugQueue.Count > 0)
                    {
                        return _debugQueue.Dequeue();
                    }
                    return null;
                    
                }
            }
        }

    }
}
