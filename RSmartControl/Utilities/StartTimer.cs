using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    class StartTimer
    {
        long _actualTime;
        long _lastTime;
        bool _tick;
        readonly double _interval;
        public StartTimer(double interval)
        {
            _interval = interval;
            _actualTime = DateTime.Now.Ticks;
            _lastTime = DateTime.Now.Ticks;
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
            _actualTime = DateTime.Now.Ticks;

            if (_actualTime - _lastTime >= _interval * 10000000)
            {
                _tick = true;
                _lastTime = _actualTime;
            }
            else
            {
                _tick = false;
            }
        }
    }
}
