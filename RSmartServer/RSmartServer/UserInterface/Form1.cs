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
        public RSmartServer()
        {
            _debugLog = new DebugLog();
            _ip = GetIp();
            Thread serverThread = new Thread(() =>
            {
                _server = new WebServer("http://" + _ip + ":80/", _debugLog);
                _server.Run();
            });
            serverThread.Start();
            InitializeComponent();

            labelIp.Text = _ip;

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
        }

        private void T_Camera_Update(object sender, EventArgs e)
        {
            pictureWebcam.Load("https://www.tradeit.fr/Webcam/image_upload/img.jpg");
        }

        private void T_Loop(object sender, EventArgs e)
        {
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


    }
}
