using System;
using Microsoft.SPOT;

namespace RSmartControl.Robot_Behaviours
{
    public class RobotBehaviourPlugin : IRobotBehaviour
    {

        public void TurnLeft(Robot r)
        {
            r.TurnLeft();
        }

        public void TurnRight( Robot r )
        {
            r.TurnRight();
        }

        public void GoBack( Robot r )
        {
            r.GoBackward();
        }

        public void GoForward( Robot r )
        {
            r.GoForward();
        }

        public void Stop( Robot r )
        {
            r.MotorLeft.Stop();
            r.MotorRight.Stop();
        }

        public void Start( Robot r )
        {
            r.MotorLeft.Start();
            r.MotorRight.Start();
        }
    }
}
