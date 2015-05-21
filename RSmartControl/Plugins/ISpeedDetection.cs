using System;
using Microsoft.SPOT;

namespace RSmartControl
{
    public interface ISpeedDetection
    {
        void Run();

        void Detect();
    }
}
