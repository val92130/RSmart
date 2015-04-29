using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Net.NetworkInformation;


namespace RSmartControl
{
    public class Program
    {
        public static void Main()
        {
            // Initializing the communication module
            Communication _communication = new Communication();

            // Enabling DHCP
            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].EnableDhcp();

            // Starting the Server
            Thread server = new Thread( () =>
            {
                WebServer webServer = new WebServer( _communication );
                webServer.ListenForRequest();
            } );
            server.Start();
            HTTPRequest _http = new HTTPRequest();
            _http.Send("http://10.8.111.132/?caca=true");
            MainLoop loop = new MainLoop(_communication );
            loop.Run();
        }



    }
}
