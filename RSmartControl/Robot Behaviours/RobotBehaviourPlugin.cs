using System;
using Microsoft.SPOT;

namespace RSmartControl.Robot_Behaviours
{
    public class RobotBehaviourPlugin : IRobotBehaviour
    {
        private Robot _robot;

        public void Run(Robot r)
        {
            _robot = r;
        }
        private void Left()
        {
            if (_robot == null)
                return;
            _robot.TurnLeft(); 
        }

        private void Right()
        {
            if (_robot == null)
                return;
            _robot.TurnRight();
        }

        private void Back()
        {
            if (_robot == null)
                return;
            _robot.GoBackward();
        }

        private void Forward()
        {
            if (_robot == null)
                return;
            _robot.GoForward();
        }

        private void DoStop()
        {
            if (_robot == null)
                return;
            _robot.MotorLeft.Stop();
            _robot.MotorRight.Stop();
        }

        private void DoStart()
        {
            if (_robot == null)
                return;
            _robot.MotorLeft.Start();
            _robot.MotorRight.Start();
        }
        public BehaviourControl.RobotBehaviourDel TurnLeft()
        {
            return new BehaviourControl.RobotBehaviourDel(Left);
        }

        public BehaviourControl.RobotBehaviourDel TurnRight()
        {
            return new BehaviourControl.RobotBehaviourDel(Right);
        }

        public BehaviourControl.RobotBehaviourDel GoBack()
        {
            return new BehaviourControl.RobotBehaviourDel(Back);
        }

        public BehaviourControl.RobotBehaviourDel GoForward()
        {
            return new BehaviourControl.RobotBehaviourDel(Forward);
        }

        public BehaviourControl.RobotBehaviourDel Stop()
        {
            return new BehaviourControl.RobotBehaviourDel(DoStop);
        }

        public BehaviourControl.RobotBehaviourDel Start()
        {
            return new BehaviourControl.RobotBehaviourDel(DoStart);
        }
    }
}
