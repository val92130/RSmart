using System;
using Microsoft.SPOT;

namespace RSmartControl.Robot_Behaviours
{
    public interface IRobotBehaviour
    {
        void TurnLeft(Robot r);
        void TurnRight( Robot r );
        void GoBack( Robot r );
        void GoForward( Robot r );
        void Stop( Robot r );
        void Start( Robot r );

    }
}
