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
