using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        RouteCreationForm _routeCreationForm;
        private bool _started;
        private string _robotIp;
   
        public RSmartServer()
        {
            _robotIp = "10.8.110.204";
            _debugLog = new DebugLog();
            _ip = GetIp();
            Thread serverThread = new Thread(() =>
            {
                _server = new WebServer("http://" + _ip + ":80/", _debugLog);
                _server.Run();
            });

            serverThread.Start();
            InitializeComponent();

            labelIp.Text = _ip + " - " + _server.Status;

            _loopTimer = new System.Windows.Forms.Timer();
            _loopTimer.Interval = 10;
            _loopTimer.Tick += new EventHandler(T_Loop);
            _loopTimer.Start();

            _cameraTimer = new System.Windows.Forms.Timer();
            _cameraTimer.Interval = 1000;
            _cameraTimer.Tick += new EventHandler(T_Camera_Update);
            _cameraTimer.Start();

            webBrowser.Url = new Uri("http://" + _ip + ":80/");

            _debugLog.Write("App started");

            UpdateRoutesTextBox();

            SendRequest("10.8.109.101", "test=true");
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
            pictureWebcam.Load("https://www.tradeit.fr/Webcam/image_upload/img.jpg");
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
                    SendRequestRobot( "Backward=true" );
                    break;
                case Keys.Q:
                    SendRequestRobot( "Left=true" );
                    break;
                case Keys.D:
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

        public void SendRequestRobot(  string req )
        {
            string url = "http://" + _robotIp + "/?" + req + "&robot=true";

            GET( url );

        }

        public void SendRequest(string ip, string req)
        {
            string url = "http://" + ip + "/?" + req;
            GET(url);
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


    }
}
