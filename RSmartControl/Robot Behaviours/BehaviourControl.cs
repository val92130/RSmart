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

        public delegate void RobotBehaviourDel();

        private RobotBehaviourDel _fMethodDel;
        private RobotBehaviourDel _fLeftMethodDel;
        private RobotBehaviourDel _fRightMethodDel;
        private RobotBehaviourDel _bMethodDel;

        public BehaviourControl(PluginManager plugins)
        {
            ArrayList lst = GetAllMethods();
            _pluginManager = plugins;
            _frontMethod = (string)lst[0];
            _backMethod = (string)lst[2];
            _frontLeftMethod = (string)lst[1];
            _frontRightMethod = (string)lst[0];
            WriteConfigurationFile();
            this.Init();
        }

        public void Refresh()
        {
            this.Init();
        }

        private void Init()
        {
            // Init front method
            MethodInfo mFrontMethodInfo = typeof(RobotBehaviourPlugin).GetMethod(this.FrontMethod);
            _fMethodDel = (RobotBehaviourDel)mFrontMethodInfo.Invoke(_pluginManager.RobotBehaviourPlugin, null);

            // Init front left method
            MethodInfo mFrontLeftMethodInfo = typeof(IRobotBehaviour).GetMethod(this.FrontLeftMethod);
            _fLeftMethodDel = (RobotBehaviourDel)mFrontLeftMethodInfo.Invoke(_pluginManager.RobotBehaviourPlugin, null);

            // Init front right method
            MethodInfo mFrontRightMethodInfo = typeof(IRobotBehaviour).GetMethod(this.FrontRightMethod);
            _fRightMethodDel = (RobotBehaviourDel)mFrontRightMethodInfo.Invoke(_pluginManager.RobotBehaviourPlugin, null);
            
            // Init back method
            MethodInfo mBackMethodInfo = typeof(IRobotBehaviour).GetMethod(this.BackMethod);
            _bMethodDel = (RobotBehaviourDel)mBackMethodInfo.Invoke(_pluginManager.RobotBehaviourPlugin, null);
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

        public void OnObstacleFront()
        {
            _fMethodDel();
        }

        public void OnObstacleBack()
        {
            _bMethodDel();
        }

        public void OnObstacleFrontLeft()
        {
            _fLeftMethodDel();
        }

        public void OnObstacleFrontRight()
        {
            _fRightMethodDel();
        }

        public static bool ValidateMethod(string input)
        {
            if (GetAllMethods().Contains((string)input))
            {
                return true;
            }
            return false;
        }

        public static string Behaviours
        {
            get { return "FrontMethod-FrontLeftMethod-FrontRightMethod-BackMethod"; }
        }

        public string FrontMethod
        {
            get { return _frontMethod; }
            set { if (ValidateMethod(value)) _frontMethod = value; }
        }

        public string BackMethod
        {
            get { return _backMethod; }
            set { if (ValidateMethod(value))_backMethod = value; }
        }

        public string FrontLeftMethod
        {
            get { return _frontLeftMethod; }
            set { if (ValidateMethod(value))_frontLeftMethod = value; }
        }

        public string FrontRightMethod
        {
            get { return _frontRightMethod; }
            set { if (ValidateMethod(value))_frontRightMethod = value; }
        }
    }
}
