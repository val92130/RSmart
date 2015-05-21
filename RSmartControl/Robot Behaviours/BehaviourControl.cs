using System;
using System.Collections;
using System.Reflection;
using Microsoft.SPOT;

namespace RSmartControl.Robot_Behaviours
{
    public class BehaviourControl
    {
        private readonly PluginManager _pluginManager;
        private static String ConfigurationName = "config.bin";
        private static String _frontMethod, _backMethod, _frontLeftMethod, _frontRightMethod;

        public BehaviourControl(PluginManager plugins)
        {
            ArrayList lst = GetAllMethods();
            _pluginManager = plugins;
            _frontMethod = (string)lst[0];
            _backMethod = (string)lst[2];
            _frontLeftMethod = (string)lst[1];
            _frontRightMethod = (string)lst[0];
            WriteConfigurationFile();
            
        }

        public static ArrayList GetAllMethods()
        {
            ArrayList methodList = new ArrayList();

            MethodInfo[] methodInfos = typeof (IRobotBehaviour).GetMethods();

            foreach (MethodInfo m in methodInfos)
            {
                methodList.Add(m.Name);
            }
            return methodList;
        }

        public string ReadConfigurationFile()
        {
            return _pluginManager.SdCardManager.Read(ConfigurationName);
        }

        public void WriteConfigurationFile()
        {
            string str = "";
            str += _frontMethod + "\n";
            str += _frontLeftMethod + "\n";
            str += _frontRightMethod + "\n";
            str += _backMethod;

            _pluginManager.SdCardManager.Write(str, ConfigurationName);
        }

        public static bool ValidateMethod(string input)
        {
            if (GetAllMethods().Contains((string) input))
            {
                return true;
            }
            return false;
        }

        public static string FrontMethod
        {
            get { return _frontMethod; }
            set { if( ValidateMethod(value)) _frontMethod = value; }
        }

        public static string BackMethod
        {
            get { return _backMethod; }
            set { if (ValidateMethod(value))_backMethod = value; }
        }

        public static string FrontLeftMethod
        {
            get { return _frontLeftMethod; }
            set { if (ValidateMethod(value))_frontLeftMethod = value; }
        }

        public static string FrontRightMethod
        {
            get { return _frontRightMethod; }
            set { if (ValidateMethod(value))_frontRightMethod = value; }
        }

        public void OnObstacleFront(Robot robot)
        {
            MethodInfo m = typeof (IRobotBehaviour).GetMethod(FrontMethod);
            m.Invoke(_pluginManager.RobotBehaviourPlugin, new object[] {robot});
        }

        public void OnObstacleBack(Robot robot)
        {
            MethodInfo m = typeof( IRobotBehaviour ).GetMethod( BackMethod );
            m.Invoke( _pluginManager.RobotBehaviourPlugin, new object[] { robot } );
        }

        public void OnObstacleFrontLeft(Robot robot)
        {
            MethodInfo m = typeof( IRobotBehaviour ).GetMethod( FrontLeftMethod );
            m.Invoke( _pluginManager.RobotBehaviourPlugin, new object[] { robot } );
        }

        public void OnObstacleFrontRight(Robot robot)
        {
            MethodInfo m = typeof( IRobotBehaviour ).GetMethod( FrontRightMethod );
            m.Invoke( _pluginManager.RobotBehaviourPlugin, new object[] { robot } );
        }

        
    }
}
