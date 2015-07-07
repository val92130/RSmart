using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Net.NetworkInformation;
using System.IO;


namespace RSmartControl
{
    public class Program
    {
        public static void Main()
        {

            // Plugins initialization
            PluginManager pluginManager = new PluginManager();

            //pluginManager.SdCardManager.Write("Helloooo", "config.txt");

            // Start speed detection
            pluginManager.SpeedDetectionModuleModule.Run();

            // Enabling DHCP
            //Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].EnableDhcp();

            // Starting the Server
            Thread server = new Thread( () =>
            {
                WebServer webServer = new WebServer( pluginManager );
                webServer.ListenForRequest();
            } );

            server.Start();

            MainLoop loop = new MainLoop( pluginManager );

            pluginManager.RobotBehaviourPlugin.Run(loop.Robot);

            loop.Run();
        }



    }
}
