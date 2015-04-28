using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Form1()
        {
            
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler(T_loop);
            timer.Start();
            Thread t = new Thread(() =>
            {
                server = new WebServer( "http://"+ GetIp() + ":8080/test/" );
                server.Run();
            });
            t.Start();
            InitializeComponent();
        }

        private void T_loop( object sender, EventArgs e )
        {
            string request = "";
            for (int i = 0; i < server.RequestList.Count; i++)
            {
                request = request + server.RequestList[i] + "\n";
            }
            textBoxRequest.Text = request;
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
    }
}
