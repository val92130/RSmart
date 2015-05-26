using System;
using Microsoft.SPOT;
namespace RSmartControl.Robot_Behaviours
{
    public interface IRobotBehaviour
    {
        BehaviourControl.RobotBehaviourDel TurnLeft();
        BehaviourControl.RobotBehaviourDel TurnRight();
        BehaviourControl.RobotBehaviourDel GoBack();
        BehaviourControl.RobotBehaviourDel GoForward();
        BehaviourControl.RobotBehaviourDel Stop();
        BehaviourControl.RobotBehaviourDel Start();

    }
}
