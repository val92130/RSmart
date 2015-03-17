using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RSmartControl
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].EnableDhcp();
            Thread server = new Thread(() =>
            {
                WebServer webServer = new WebServer();
                webServer.ListenForRequest();
            });
            server.Start();
            
            MainLoop loop = new MainLoop(2);
            loop.Run();
        }



    }
}
