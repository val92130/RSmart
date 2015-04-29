using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSmartServer
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        WebServer server;
        bool right;
        bool left;
        bool up;
        bool down;
        public Form1()
        {
            
            
            Thread t = new Thread(() =>
            {
                server = new WebServer( "http://"+ GetIp() + ":80/" );
                server.Run();
            });
            t.Start();

            
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler( T_loop );
            timer.Start();

            InitializeComponent();
            labelIp.Text = "Server IP : " + GetIp();
        }

        private void T_loop( object sender, EventArgs e )
        {
            string request = "";
            for (int i = 0; i < server.RequestList.Count; i++)
            {
                request = request + server.RequestList[i] + "\n";
            }
            
        }

        public string GetIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry( Dns.GetHostName() );
            foreach( IPAddress ip in host.AddressList )
            {
                if( ip.AddressFamily == AddressFamily.InterNetwork )
                {
                    localIP = ip.ToString();
                }
            }
            
            return localIP;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("ok");
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z) { up = false; SendRequest("10.8.110.204", "Stop=true"); }
            if (e.KeyCode == Keys.Q) { left = false; SendRequest("10.8.110.204", "Stop=true"); }
            if (e.KeyCode == Keys.S) { down = false; SendRequest("10.8.110.204", "Stop=true"); }
            if (e.KeyCode == Keys.D) { right = false; SendRequest("10.8.110.204", "Stop=true"); }
        }

        private void startButton_Click( object sender, EventArgs e )
        {
            SendRequest( "10.8.110.204", "Start=true" );
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            SendRequest("10.8.110.204", "Stop=true");
        }

        public void SendRequest(string ip, string req)
        {
            string url = "http://" + ip + "/?" + req + "&robot=true";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Form1_Load( object sender, EventArgs e )
        {

        }

        private void Form1_KeyDown_1( object sender, KeyEventArgs e )
        {
            MessageBox.Show("ee");
        }

        

        
    }
}
