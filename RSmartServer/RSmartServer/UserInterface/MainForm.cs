using ServerLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    public partial class RSmartServer : Form
    {
        string _ip;
        System.Windows.Forms.Timer _loopTimer;
        System.Windows.Forms.Timer _cameraTimer;
        System.Windows.Forms.Timer _routineTimer;
        RouteCreationForm _routeCreationForm;
        private bool _started, _startedBack;
        private string _robotIp;
        int _routeCount = 0;
        private bool _isRobotOnline = false;

        RobotControl _robotControl;
   
        public RSmartServer()
        {
            if( !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() )
            {
                MessageBox.Show( "Error : No network available" );
                Application.Exit();
                try
                {
                    this.Close();
                }
                catch
                { }
                return;
            }

            _robotIp = Util.RobotIp;
            _ip = Util.GetIp();

            _robotControl = new RobotControl(_robotIp);

            InitializeComponent();
            Application.ApplicationExit += new EventHandler( this.OnApplicationExit );

            labelIp.Text = _ip + " - " + _robotControl.WebServer.Status;

            InitializeTimer();

            webBrowser.Url = new Uri("http://" + _ip + ":80/");

            _robotControl.DebugLog.Write("App started");

            _routeCount = _robotControl.WebServer.Routes.Count;
            UpdateRoutesTextBox();
            

        }

        private void InitializeTimer()
        {
            _loopTimer = new System.Windows.Forms.Timer();
            _loopTimer.Interval = 10;
            _loopTimer.Tick += new EventHandler( T_Loop );
            _loopTimer.Start();

            _cameraTimer = new System.Windows.Forms.Timer();
            _cameraTimer.Interval = 1000;
            _cameraTimer.Tick += new EventHandler( T_Camera_Update );
            _cameraTimer.Start();

            _routineTimer = new System.Windows.Forms.Timer();
            _routineTimer.Interval = 1000;
            _routineTimer.Tick += new EventHandler( Routine );
            _routineTimer.Start();
        }

        private void Routine( object sender, EventArgs e )
        {
            if (_routeCount != _robotControl.WebServer.Routes.Count)
            {
                _routeCount = _robotControl.WebServer.Routes.Count;
                UpdateRoutesTextBox();
            }
            _isRobotOnline = _robotControl.PingRobot();
        }



        private void OnApplicationExit( object sender, EventArgs e )
        {
            _robotControl.SendRequestRobot( "Desynchronize=" + Util.GetIp() );
        }

        

        public void UpdateRoutesTextBox()
        {
            string txt = "";
            for (int i = 0; i < _robotControl.WebServer.Routes.Count; i++)
            {
                if(i > 0)
                {
                    txt += "\nRoute n° " + i + "\n" + "Key : " + _robotControl.WebServer.Routes[i].Key + "\nValue : " + _robotControl.WebServer.Routes[i].Value + "\nResponse : " + _robotControl.WebServer.Routes[i].Response + "\n";
                }
                else
                {
                    txt += "Route n° " + i + "\n" + "Key : " + _robotControl.WebServer.Routes[i].Key + "\nValue : " + _robotControl.WebServer.Routes[i].Value + "\nResponse : " + _robotControl.WebServer.Routes[i].Response + "\n";
                }
                
            }
            routesTextBox.Text = txt;

        }

        private void T_Camera_Update(object sender, EventArgs e)
        {
            if( !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() )
            {
                return;
            }
            try
            {
                pictureWebcam.Load( "https://www.tradeit.fr/Webcam/image_upload/img.jpg" );
            }
            catch( Exception ex )
            {

                _robotControl.DebugLog.Write(ex.ToString());
            }
        }

        private void T_Loop(object sender, EventArgs e)
        {
            labelIp.Text = _ip + " - " + _robotControl.WebServer.Status;
            UpdateLog();
        }

        public void UpdateLog()
        {
            if (_robotControl.DebugLog.Count > 0)
            {
                logTextBox.AppendText(DateTime.Now.ToString() + " : " + _robotControl.DebugLog.Get + "\n");
                logTextBox.SelectionStart = logTextBox.Text.Length;
                logTextBox.ScrollToCaret();
            }
        }

        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _robotControl.WebServer.Stop();
            this.Close();     
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            string url = textBoxUrl.Text;
            if (!String.IsNullOrWhiteSpace(url))
            {
                if(!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                        url = "http://" + url;
                }
                if (Util.RemoteFileExists(url))
                {
                    webBrowser.Url = new Uri(url);
                }
                else
                {
                    MessageBox.Show("Bad Url");
                }
            }
        }
       
        private void pauseServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _robotControl.WebServer.Pause();
        }

        private void resumeServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _robotControl.WebServer.Resume();
        }

        private void addARouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _routeCreationForm = new RouteCreationForm(_robotControl.WebServer);
            _routeCreationForm.ShowDialog(this);
        }

        private void RSmartServer_KeyDown( object sender, KeyEventArgs e )
        {
            switch (e.KeyCode )
            {
                case Keys.Z :
                    if (!_started)
                    {
                        _started = true;
                        _robotControl.SendRequestRobot("Forward=true");
                        _robotControl.SendRequestRobot("Start=true");

                    }
                    break;
                case Keys.S:
                    if( !_startedBack )
                    {
                        _startedBack = true;
                        _robotControl.SendRequestRobot("Backward=true");
                        _robotControl.SendRequestRobot("Start=true");
                    }
                    
                    break;
                case Keys.Q:
                    _robotControl.SendRequestRobot("Start=true");
                    _robotControl.SendRequestRobot("Left=true");
                    break;
                case Keys.D:
                    _robotControl.SendRequestRobot("Start=true");
                    _robotControl.SendRequestRobot("Right=true");
                    break;

            }
        }

        private void RSmartServer_KeyUp( object sender, KeyEventArgs e )
        {
            switch( e.KeyCode )
            {
                case Keys.Z:
                    _started = false;
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case Keys.S:
                    _startedBack = false;
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case Keys.Q:
                    _robotControl.SendRequestRobot("Stop=true");
                    break;
                case Keys.D:
                    _robotControl.SendRequestRobot("Stop=true");
                    break;

            }
        }

        private void pingButton_Click( object sender, EventArgs e )
        {
            if (_robotControl.PingRobot())
            {
                MessageBox.Show("Robot is online");
            }
            else
            {
                MessageBox.Show( "Robot is offline" );
            }
        }

        private void synchronizeToolStripMenuItem_Click( object sender, EventArgs e )
        {
            _robotControl.SendRequestRobot("Synchronize=" + Util.GetIp());
        }

        private void unsynchronizeToolStripMenuItem_Click( object sender, EventArgs e )
        {
            _robotControl.SendRequestRobot("Desynchronize=" + Util.GetIp());
        }

        private void startButton_Click( object sender, EventArgs e )
        {
            _robotControl.SendRequestRobot("Start=true");
        }

        private void buttonStopMotor_Click( object sender, EventArgs e )
        {
            _robotControl.SendRequestRobot("Stop=true");
        }

    }
}
