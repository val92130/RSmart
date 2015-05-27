using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace Server.Lib
{
    public class DebugLog
    {
        public delegate void MessageArrivedHandler();
        readonly Queue<LogMessage> _debugQueue;
        public event MessageArrivedHandler TextArrived;
        public DebugLog()
        {
            _debugQueue = new Queue<LogMessage>();
        }

        public virtual void OnChanged()
        {
            if (TextArrived != null)
            {
                TextArrived();
            }
        }
        public void Write(LogMessage log)
        {
            lock(_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(log);
                Debug.Print(log.Text);
            }
        }

        public void Write(String text)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text));
                Debug.Print(text);
            }
        }

        public void Write(String text, Color color)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text, color));
                Debug.Print(text);
            }
        }

        public void Write(String text, Color color, EMessageCategory category)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text, color, category));
                Debug.Print(text);
            }
        }

        public void Write(String text, EMessageCategory category)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text, category));
                Debug.Print(text);
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

        public LogMessage Get
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
