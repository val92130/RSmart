using ServerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UserInterfaceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RobotControl _robotControl;
        DispatcherTimer _loopTimer, _cameraTimer, _routineTimer;
        int _routeCount = 0;
        private bool _isRobotOnline = false;
        private bool _started, _startedBack;
        public MainWindow()
        {
            _robotControl = new RobotControl(Util.RobotIp);       
            InitializeComponent();
            this.Initialize();
        }
        public void Initialize()
        {
            _robotControl.WebCamManager.Start();
            labelIpTitle.Content = "Server IP : " + Util.GetIp();
            this.InitializeTimers();
            _routeCount = _robotControl.WebServer.Routes.Count;
            UpdateRoutesTextBox();
        }

        public void InitializeTimers()
        {
            _loopTimer = new DispatcherTimer();
            _loopTimer.Interval = new TimeSpan(0, 0, 0, 0, 50); 
            _loopTimer.Tick += new EventHandler(T_Loop);
            _loopTimer.Start();

            _cameraTimer = new DispatcherTimer();
            _cameraTimer.Interval = new TimeSpan(0, 0, 0, 0, 6000); 
            _cameraTimer.Tick += new EventHandler(T_Camera_Update);
            _cameraTimer.Start();

            _routineTimer = new DispatcherTimer();
            _routineTimer.Interval = new TimeSpan(0, 0, 0, 0, 4000); 
            _routineTimer.Tick += new EventHandler(Routine);
            _routineTimer.Start();
        }


        private void Routine(object sender, EventArgs e)
        {
            if (_routeCount != _robotControl.WebServer.Routes.Count)
            {
                _routeCount = _robotControl.WebServer.Routes.Count;
                UpdateRoutesTextBox();
            }
            _isRobotOnline = _robotControl.PingRobot();
        }

        public void UpdateRoutesTextBox()
        {
            string txt = "";
            for (int i = 0; i < _robotControl.WebServer.Routes.Count; i++)
            {
                if (i > 0)
                {
                    txt += "\nRoute n° " + i + "\n" + "Key : " + _robotControl.WebServer.Routes[i].Key + "\nValue : " + _robotControl.WebServer.Routes[i].Value + "\nResponse : " + _robotControl.WebServer.Routes[i].Response + "\n";
                }
                else
                {
                    txt += "Route n° " + i + "\n" + "Key : " + _robotControl.WebServer.Routes[i].Key + "\nValue : " + _robotControl.WebServer.Routes[i].Value + "\nResponse : " + _robotControl.WebServer.Routes[i].Response + "\n";
                }

            }
            textBoxRoutes.Document.Blocks.Clear();
            textBoxRoutes.Document.Blocks.Add(new Paragraph(new Run(txt)));

        }

        private void T_Camera_Update(object sender, EventArgs e)
        {
            pictureWebcam.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(WebcamManager.Path);
        }

        public void UpdateLog()
        {
            if (_robotControl.DebugLog.Count > 0)
            {
                textBoxLog.AppendText(DateTime.Now.ToString() + " : " + _robotControl.DebugLog.Get + "\n");
                textBoxLog.ScrollToEnd();
            }
        }

        private void T_Loop(object sender, EventArgs e)
        {
            this.UpdateLog();
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (!_started)
                    {
                        _started = true;
                        _robotControl.SendRequestRobot("Forward=true");
                        _robotControl.SendRequestRobot("Start=true");

                    }
                    break;
                case Key.Down:
                    if (!_startedBack)
                    {
                        _startedBack = true;
                        _robotControl.SendRequestRobot("Backward=true");
                        _robotControl.SendRequestRobot("Start=true");
                    }

                    break;
                case Key.Left:
                    _robotControl.SendRequestRobot("Start=true");
                    _robotControl.SendRequestRobot("Left=true");
                    break;
                case Key.Right:
                    _robotControl.SendRequestRobot("Start=true");
                    _robotControl.SendRequestRobot("Right=true");
                    break;

            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _started = false;
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case Key.Down:
                    _startedBack = false;
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case Key.Left:
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case Key.Right:
                    _robotControl.SendRequestRobot("Stop=true");
                    break;

            }
        }

        private void synchronizeButton_Click(object sender, RoutedEventArgs e)
        {
            _robotControl.SendRequestRobot("Synchronize=" + Util.GetIp());
        }

        private void unsynchronizeButton_Click(object sender, RoutedEventArgs e)
        {
            _robotControl.SendRequestRobot("Desynchronize=" + Util.GetIp());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(
        responseTextBox.Document.ContentStart,
        responseTextBox.Document.ContentEnd
    );

            if (!string.IsNullOrWhiteSpace(keyTextBox.Text) || !string.IsNullOrWhiteSpace(valueTextBox.Text) || !string.IsNullOrWhiteSpace(textRange.Text))
            {
                _robotControl.WebServer.AddRoute(keyTextBox.Text, valueTextBox.Text, textRange.Text);
                valueTextBox.Text = "";
                keyTextBox.Text = "";
                responseTextBox.Document.Blocks.Clear();
            }
        }
    }
}
