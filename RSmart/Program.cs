using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace RSmart
{
    public class Program
    {
        public static void Main()
        {
            // write your code here

            OutputPort onboardLED = new OutputPort( Pins.ONBOARD_LED, false );
    }

}
