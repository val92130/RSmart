using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    class CustomTimer
    {
        long actualTime = DateTime.Now.Ticks;
        long lastTime = DateTime.Now.Ticks;
        bool _tick;
        double _interval;
        public CustomTimer(double interval)
        {
            _interval = interval;
            actualTime = DateTime.Now.Ticks;
            lastTime = DateTime.Now.Ticks;
        }

        public bool Tick
        {
            get
            {
                return _tick;
            }
        }

        public void Update()
        {
            actualTime = DateTime.Now.Ticks;

            if (actualTime - lastTime >= _interval * 10000000)
            {
                _tick = true;
                lastTime = actualTime;
            }
            else
            {
                _tick = false;
            }
        }
    }
}
