using Server.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Server.App
{
    /// <summary>
    /// Interaction logic for ControlManagerWindow.xaml
    /// </summary>
    public partial class ControlManagerWindow : Window
    {
        RobotControl _robotControl;
        public ControlManagerWindow(RobotControl r)
        {
            _robotControl = r;
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelOffset.Content = slider.Value;
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if(startRobotCheck.IsChecked.Value)
            {
                _robotControl.SendRequestRobot("SetDirection=" + (int)slider.Value);
                _robotControl.SendRequestRobot("GoForwardTime=" + (int)sliderTime.Value);
            }
            else
            {
                _robotControl.SendRequestRobot("SetDirection=" + (int)slider.Value);
            }
        }

        private void sliderTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(valueTimeLabel != null)
                valueTimeLabel.Content = sliderTime.Value;
        }

        private void orientateButton_Click(object sender, RoutedEventArgs e)
        {
            Vector2 positionRobot = _robotControl.GetRobotPosition();
            Vector2 orientationRobot = _robotControl.GetRobotOrientation();
            Vector2 destination = new Vector2(Convert.ToInt32(xTextBox.Text), Convert.ToInt32(yTextBox.Text));

            if (positionRobot == null || orientationRobot == null || destination == null)
            {
                _robotControl.DebugLog.Write("Exception in method : orientate, one of the value was null",EMessageCategory.Error);
                return;
            }

            double rad = Server.Lib.Vector2.Radius(positionRobot,orientationRobot,destination);
            _robotControl.SendRequestRobot("SetDirection=" + Offset.GetClosestOffset(rad));
            double timeTodo = Vector2.TimeBetweenPoints(positionRobot, orientationRobot, destination);
            _robotControl.SendRequestRobot("GoForwardTime="+(int)timeTodo);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            string strX = _robotControl.SendRequestRobot("GetPositionX=true");
            string strY = _robotControl.SendRequestRobot("GetPositionY=true");

            currentPosLabel.Content = strX + ", " + strY;


            speedLabel.Content = "Speed : " + _robotControl.SendRequestRobot("GetSpeed=true");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _robotControl.SendRequestRobot("Stop=true");
        }
    }
}
