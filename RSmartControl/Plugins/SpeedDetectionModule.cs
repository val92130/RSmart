using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RSmartControl
{
    using System.Threading;

   public class SpeedDetectionModule : ISpeedDetection
    {
        private readonly Thread _mainThread;
        private readonly PluginManager _pluginManager;
        const double WheelDiameter = 12.5;
        double _speed = 0;
        public SpeedDetectionModule(PluginManager pluginManager)
        {
            _pluginManager = pluginManager;
            _mainThread = new Thread(new ThreadStart(this.Detect));         
        }
        public void Run()
        {
            _mainThread.Start();
        }

       public double Speed
       {
           get
           {
                return _speed;
           }
       }

        public void Detect()
        {
            
            DateTime now, prev = DateTime.Now;
            int nbrTour = 0;
            
            bool flag = false;
            DateTime lastRecord = DateTime.Now;
            TimeSpan gap;
            while (true)
            {
                {
                    now = DateTime.Now;

                    double read = _pluginManager.SensorsManager.MagneticSensor.Read();
                    gap = DateTime.Now - lastRecord;
                    if (read != 1 && !flag && gap.Milliseconds >= 100)
                    {
                        nbrTour++;
                        lastRecord = DateTime.Now;
                        flag = true;
                        Debug.Print("Detected : + Nombre tours : " + nbrTour.ToString());
                    }
                    else
                    {
                        flag = false;
                    }

                    TimeSpan time = now - prev;
                    const int waitTime = 3;
                    if (time.Seconds >= waitTime)
                    {
                        prev = DateTime.Now;
                        double dist = nbrTour * WheelDiameter;
                        _speed = dist / waitTime;
                        _speed /= 100000;
                        _speed *= 3600;
                        Debug.Print("Speed : " + _speed +"km/h");
                        nbrTour = 0;
                    }
                }
            }
        }
    }
}
