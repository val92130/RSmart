using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    class CustomTimer
    {
        long actualTime = DateTime.Now.Ticks;
        long lastTime = DateTime.Now.Ticks;
        bool _tick;
        int _interval;
        public CustomTimer(int interval)
        {
            _interval = interval;
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

            if( actualTime - lastTime >= _interval * 10000000 )
            {
                _tick = true;
            }
            else
            {
                _tick = false;
            }
        }
    }
}
