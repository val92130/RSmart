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
            Communication _communication = new Communication();
            // write your code here
           // Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].EnableDhcp();

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (interfaces.Length > 0)
            {
                NetworkInterface myInterface = interfaces[0];
                myInterface.EnableStaticIP("192.168.100.3", "255.255.255.0", "192.168.100.1");​
            }
            

            Thread server = new Thread(() =>
            {
                WebServer webServer = new WebServer(_communication);
                webServer.ListenForRequest();
            });
            server.Start();
            
            MainLoop loop = new MainLoop(2, _communication);
            //loop.Run();
        }



    }
}
