using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RSmartControl
{
    using System.Threading;

   public class SpeedDetection : ISpeedDetection
    {
        private Thread _mainThread;
        public SpeedDetection()
        {
            _mainThread = new Thread(new ThreadStart(this.Detect));
           
        }
        public void Run()
        {
            _mainThread.Start();
        }

        public void Detect()
        {
            AnalogInput _magneticSensor = new AnalogInput(Cpu.AnalogChannel.ANALOG_4);
            DateTime now, prev = DateTime.Now;
            int nbrTour = 0;
            double wheelDiameter = 12.5;
            double speed = 0;
            bool flag = false;
            DateTime lastRecord = DateTime.Now;
            TimeSpan gap;
            while (true)
            {
                {
                    now = DateTime.Now;

                    double read = _magneticSensor.Read();
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
                    int waitTime = 3;
                    // We check the speed every 3 seconds
                    if (time.Seconds >= waitTime)
                    {
                        prev = DateTime.Now;
                        double dist = nbrTour * wheelDiameter;
                        speed = dist / waitTime;
                        Debug.Print("Speed : " + speed.ToString());
                        nbrTour = 0;
                    }
                }
            }
        }
    }
}
