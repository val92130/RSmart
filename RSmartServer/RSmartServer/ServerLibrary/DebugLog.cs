using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace Server.Lib
{
    /// <summary>
    /// Used to create a log of information
    /// </summary>
    public class DebugLog
    {
        public delegate void MessageArrivedHandler();
        readonly Queue<LogMessage> _debugQueue;
        public event MessageArrivedHandler TextArrived;
        /// <summary>
        /// Creates a new instance of DebugLog
        /// </summary>
        public DebugLog()
        {
            _debugQueue = new Queue<LogMessage>();
        }

        /// <summary>
        /// Event triggered when a new log is arrived
        /// </summary>
        public virtual void OnChanged()
        {
            if (TextArrived != null)
            {
                TextArrived();
            }
        }
        /// <summary>
        /// Add a new message in the queue
        /// </summary>
        /// <param name="log">Message input</param>
        public void Write(LogMessage log)
        {
            lock(_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(log);
                Debug.Print(log.Text);
            }
        }

        /// <summary>
        /// Add a new message in the queue
        /// </summary>
        /// <param name="text">Message input</param>
        public void Write(String text)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text));
                Debug.Print(text);
            }
        }
        /// <summary>
        /// Add a new message in the queue with a specified color to display
        /// </summary>
        /// <param name="text">Message input</param>
        /// <param name="color">Color of the text to display in the console</param>
        public void Write(String text, Color color)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text, color));
                Debug.Print(text);
            }
        }

        /// <summary>
        /// Add a new message in the queue with a specified color to display and a specified message category
        /// </summary>
        /// <param name="text">Message input</param>
        /// <param name="color">Color of the text to display in the console</param>
        /// <param name="category">Message category</param>
        public void Write(String text, Color color, EMessageCategory category)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text, color, category));
                Debug.Print(text);
            }
        }

        /// <summary>
        ///  Add a new message in the queue with a specified message category
        /// </summary>
        /// <param name="text">Message input</param>
        /// <param name="category">Message category</param>
        public void Write(String text, EMessageCategory category)
        {
            lock (_debugQueue)
            {
                OnChanged();
                _debugQueue.Enqueue(new LogMessage(text, category));
                Debug.Print(text);
            }
        }

        /// <summary>
        /// Returns the number of messages in the log queue
        /// </summary>
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

        /// <summary>
        /// Returns the last message of the queue and remove it from the queue
        /// </summary>
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
