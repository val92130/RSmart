using System;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Server.Lib;
using MjpegProcessor;
using System.Windows.Media;

namespace Server.App
{

    public partial class MainWindow : Window
    {
        readonly MjpegDecoder _mjpeg;
        readonly RobotControl _robotControl;
        DispatcherTimer _loopTimer, _routineTimer;
        int _routeCount = 0;
        private bool _isRobotOnline = false;
        private bool _started, _startedBack;

        
        public MainWindow()
        {
            _robotControl = new RobotControl(Util.RobotIp);       
            InitializeComponent();
            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady;
            _mjpeg.Error += _mjpeg_Error;
            this.Initialize();
            //double d = Vector2.ArcDistance(new Vector2(1, 1), new Vector2(45, 1), new Vector2(0, 1));
            //double t = Vector2.TimeBetweenPoints(200, new Vector2(1, 1), new Vector2(45, 1), new Vector2(0, 1));

        }
        public void Initialize()
        {
            labelIpTitle.Content = "Server IP : " + Util.GetIp();
            this.InitializeTimers();
            _routeCount = _robotControl.WebServer.Routes.Count;
            UpdateRoutesTextBox();
            _mjpeg.ParseStream(new Uri("http://192.168.1.110:8080/video.mjpg"));
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            pictureWebcam.Source = e.BitmapImage;
        }

        void _mjpeg_Error(object sender, ErrorEventArgs e)
        {
            _robotControl.DebugLog.Write(e.Message, EMessageCategory.Error);
        }
        public void InitializeTimers()
        {
            _loopTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 50)};

            _loopTimer.Tick += new EventHandler(T_Loop);
            _loopTimer.Start();

            _routineTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 4000)};

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
            if( _isRobotOnline )
            {
                //labelSpeed.Content = "Robot speed : " + _robotControl.SendRequestRobot( "GetSpeed=true" );
            }
            
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
        public void UpdateLog()
        {
            if (_robotControl.DebugLog.Count > 0)
            {
                LogMessage log = _robotControl.DebugLog.Get;
                DrawTextColor(textBoxLog, DateTime.Now.ToString(), Colors.Coral);
                DrawTextColor(textBoxLog, " : " + log + "\n", log.Color);
                textBoxLog.ScrollToEnd();
            }
        }

        public void DrawTextColor(RichTextBox textBox, string text, Color color)
        {
            TextRange rangeOfText1 = new TextRange(textBox.Document.ContentEnd, textBox.Document.ContentEnd);
            rangeOfText1.Text = text;
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
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
            _robotControl.DebugLog.Write( "Trying to link with the robot...", EMessageCategory.Information );

            if( _robotControl.SendRequestRobot( "Synchronize=" + Util.GetIp() ) == null )
            {
                _robotControl.DebugLog.Write( "Error : cannot link with the robot", EMessageCategory.Error );
            }
        }

        private void unsynchronizeButton_Click(object sender, RoutedEventArgs e)
        {
            _robotControl.DebugLog.Write( "Trying to unlink with the robot...", EMessageCategory.Information );
            _robotControl.SendRequestRobot("Desynchronize=" + Util.GetIp());
        }



        private void pingRobotButton_Click( object sender, RoutedEventArgs e )
        {
            MessageBox.Show(_robotControl.PingRobot() ? "Robot is online" : "Robot is offline");
        }

        private void EditBehaviourButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            if(_isRobotOnline)
            {
                BehaviourControl b = new BehaviourControl( _robotControl ) { Owner = this };
                b.ShowDialog();
            }
            else
            {
                MessageBox.Show( "The robot seems to be offline, cannot open behavior control" );
                _robotControl.DebugLog.Write( "The robot seems to be offline, cannot open behavior control", EMessageCategory.Error );
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _robotControl.SendRequestRobot("Start=true");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _robotControl.SendRequestRobot("Stop=true");
        }

        private void ShowMapButton_OnClickButton_Click(object sender, RoutedEventArgs e)
        {
            Map m= new Map(_robotControl) { Owner = this };
            m.ShowDialog();
        }

        private void ClearLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            textBoxLog.Document.Blocks.Clear();
        }

        private void ControlManager_OnClickButton(object sender, RoutedEventArgs e)
        {
            ControlManagerWindow m = new ControlManagerWindow(_robotControl) { Owner = this };
            m.ShowDialog();
        }
    }
}
