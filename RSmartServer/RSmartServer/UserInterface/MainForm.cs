﻿using System;
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
        WebServer _server;
        string _ip;
        DebugLog _debugLog;
        System.Windows.Forms.Timer _loopTimer;
        System.Windows.Forms.Timer _cameraTimer;
        System.Windows.Forms.Timer _routineTimer;
        RouteCreationForm _routeCreationForm;
        private bool _started, _startedBack;
        private string _robotIp;
        int _routeCount = 0;
        private bool _isRobotOnline = false;
   
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

            _robotIp = "10.8.110.204";
            _debugLog = new DebugLog();
            _ip = GetIp();


            InitializeThreads();

            
            InitializeComponent();
            Application.ApplicationExit += new EventHandler( this.OnApplicationExit ); 

            labelIp.Text = _ip + " - " + _server.Status;

            InitializeTimer();

            webBrowser.Url = new Uri("http://" + _ip + ":80/");

            _debugLog.Write("App started");

            _routeCount = _server.Routes.Count;
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
            if( _routeCount != null )
            {
                if( _routeCount != _server.Routes.Count )
                {
                    _debugLog.Write( "changeeed" );
                    _routeCount = _server.Routes.Count;
                    UpdateRoutesTextBox();
                }
            }

            _isRobotOnline = PingRobot();
        }

        private void InitializeThreads()
        {
            Thread serverThread = new Thread( () =>
            {
                _server = new WebServer( "http://" + _ip + ":80/", _debugLog );
                _server.Run();
            } );

            serverThread.Start();

        }


        private void OnApplicationExit( object sender, EventArgs e )
        {
            SendRequestRobot( "Desynchronize=" + this.GetIp() );
        }

        public bool PingRobot()
        {
            var ping = new Ping();

            PingReply reply = ping.Send( IPAddress.Parse( _robotIp ), 50 );

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;

        }

        public void UpdateRoutesTextBox()
        {
            string txt = "";
            for(int i = 0; i < _server.Routes.Count; i++)
            {
                if(i > 0)
                {
                    txt += "\nRoute n° " + i + "\n" + "Key : " + _server.Routes[i].Key + "\nValue : " + _server.Routes[i].Value + "\nResponse : " + _server.Routes[i].Response + "\n";
                }
                else
                {
                    txt += "Route n° " + i + "\n" + "Key : " + _server.Routes[i].Key + "\nValue : " + _server.Routes[i].Value + "\nResponse : " + _server.Routes[i].Response + "\n";
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

                _debugLog.Write( ex.ToString() );
            }
        }

        private void T_Loop(object sender, EventArgs e)
        {
            labelIp.Text = _ip + " - " + _server.Status;
            UpdateLog();
        }

        public void UpdateLog()
        {
            if(_debugLog.Count > 0)
            {
                logTextBox.AppendText(DateTime.Now.ToString() + " : " + _debugLog.Get + "\n");
                logTextBox.SelectionStart = logTextBox.Text.Length;
                logTextBox.ScrollToCaret();
            }
        }

        public string GetIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _server.Stop();
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
                if (RemoteFileExists(url))
                {
                    webBrowser.Url = new Uri(url);
                }
                else
                {
                    MessageBox.Show("Bad Url");
                }
            }
        }
       

        private bool RemoteFileExists(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp
                              || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private void pauseServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _server.Pause();
        }

        private void resumeServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _server.Resume();
        }

        private void addARouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _routeCreationForm = new RouteCreationForm(_server);
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
                        SendRequestRobot( "Forward=true" );
                        SendRequestRobot( "Start=true" );

                    }
                    break;
                case Keys.S:
                    if( !_startedBack )
                    {
                        _startedBack = true;
                        SendRequestRobot( "Backward=true" );
                        SendRequestRobot( "Start=true" );
                    }
                    
                    break;
                case Keys.Q:
                    SendRequestRobot( "Start=true" );
                    SendRequestRobot( "Left=true" );
                    break;
                case Keys.D:
                    SendRequestRobot( "Start=true" );
                    SendRequestRobot( "Right=true" );
                    break;

            }
        }

        private void RSmartServer_KeyUp( object sender, KeyEventArgs e )
        {
            switch( e.KeyCode )
            {
                case Keys.Z:
                    _started = false;
                    SendRequestRobot( "Stop=true" );
                    break;
                case Keys.S:
                    _startedBack = false;
                    SendRequestRobot( "Stop=true" );
                    break;
                case Keys.Q:
                    SendRequestRobot( "Stop=true" );
                    break;
                case Keys.D:
                    SendRequestRobot( "Stop=true" );
                    break;

            }
        }

        public string SendRequestRobot(  string req )
        {
            if(_isRobotOnline)
            {
                return SendRequest( _robotIp, req + "&robot=true" );
            }
            return null;


        }

        public string SendRequest(string ip, string req)
        {
            
            string url = "http://" + ip + "/?" + req;
            string rep = GET(url);
            _debugLog.Write("Response from " + ip + " : " + rep);
            return rep;
        }

        public string GET( string url )
        {
            try
            {
                _debugLog.Write("Sending request : " + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader( stream );

                string data = reader.ReadToEnd();

                reader.Close();
                stream.Close();

                return data;
            }
            catch( Exception e )
            {
                _debugLog.Write(e.ToString());
            }

            return null;
        }



        private void pingButton_Click( object sender, EventArgs e )
        {
            if (PingRobot())
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
            SendRequestRobot( "Synchronize=" + this.GetIp() );
        }

        private void unsynchronizeToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SendRequestRobot( "Desynchronize=" + this.GetIp() );
        }

        private void startButton_Click( object sender, EventArgs e )
        {
            this.SendRequestRobot( "Start=true" );
        }

        private void buttonStopMotor_Click( object sender, EventArgs e )
        {
            this.SendRequestRobot( "Stop=true" );
        }

    }
}
