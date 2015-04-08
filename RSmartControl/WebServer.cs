using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using Microsoft.SPOT.Hardware;
using System.Net;
using System.Text;
using System.Threading;
using SecretLabs.NETMF.Hardware.Netduino;


namespace RSmartControl
{
    using System.Collections;
    using Json.NETMF;

    public class WebServer : IDisposable
    {
        Communication _com;
        private Socket socket = null;
        private OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
        public WebServer(Communication Com)
        {
            IPAddress ip = IPAddress.Parse("192.168.100.3");
            _com = Com;
            //Initialize Socket class
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Request and bind to an IP from DHCP server
            socket.Bind(new IPEndPoint(IPAddress.Any, 80));
            //Debug print our IP address
            Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);
            //Start listen for web requests
            socket.Listen(10);
            ListenForRequest();
        }

        public void ListenForRequest()
        {
            while (true)
            {
                using (Socket clientSocket = socket.Accept())
                {
                    //Get clients IP
                    IPEndPoint clientIP = clientSocket.RemoteEndPoint as IPEndPoint;
                    EndPoint clientEndPoint = clientSocket.RemoteEndPoint;
                    int bytesReceived = clientSocket.Available;
                    if (bytesReceived > 0)
                    {
                        //Get request
                        byte[] buffer = new byte[bytesReceived];
                        int byteCount = clientSocket.Receive(buffer, bytesReceived, SocketFlags.None);
                        string request = new string(Encoding.UTF8.GetChars(buffer));
                       
                        //Debug.Print(request);
                        _com.AddMessage( request );
                       

                        //Analyze the message then send a response
                        MessageAnalyse(Utility.ParseQueryString(request), clientSocket);

                        //Blink the onboard LED                  
                        led.Write(true);
                        Thread.Sleep(150);
                        led.Write(false);                    
                    }
                }
            }
        }
        #region IDisposable Members
        ~WebServer()
        {
            Dispose();
        }

        public void MessageAnalyse(Hashtable nvc, Socket clientSocket)
        {
            if (nvc == null)
            {
                const string response = "Welcome to RSAMART VAL AND RAMI SAY HELLO TO YOU";
                SendResponse( clientSocket, response );
                return;
            }

            foreach (DictionaryEntry entry in nvc)
            {
                string response;
                switch ((string)entry.Key)
                {
                    case "GetDirection":
                        response = _com.MotorLeft.DirectionString;
                        SendResponse(clientSocket, response);
                        break;

                    case "GetObstacles" :
                        response = JsonSerializer.SerializeObject(_com.ObstacleList);
                        if(response.Length > 0)
                        SendResponse( clientSocket, response );
                        break;

                    case "GetPosition":
                        response = _com.MainLoop.Robot.Position.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetPositionX":
                        response = _com.MainLoop.Robot.Position.X.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetPositionY":
                        response = _com.MainLoop.Robot.Position.Y.ToString();
                        SendResponse( clientSocket, response );
                        break;

                    case "GetRobotDirection":
                        response = _com.MainLoop.Robot.Direction.ToString();
                        SendResponse( clientSocket, response );
                        break;

                    case "GetSpeedLeft":
                        response = _com.MotorLeft.DutyCycle.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetSpeedRight":
                        response = _com.MotorRight.DutyCycle.ToString();
                        SendResponse( clientSocket, response );
                        break;

                    case "GetMotorStatusRight":
                        response = _com.MotorRight.IsStarted.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetMotorStatusLeft":
                        response = _com.MotorLeft.IsStarted.ToString();
                        SendResponse( clientSocket, response );
                        break;

                    default :                                               
                        response = "Welcome to RSAMART VAL AND RAMI SAY HELLO TO YOU";
                        SendResponse( clientSocket, response );
                        break;
                }
            }
        }

        public string CreateHeader(string response)
        {
            return "HTTP/1.0 200 OK\r\nContent-Type: text charset=utf-8\r\nAccess-Control-Allow-Origin: *\r\nContent-Length: " + response.Length.ToString() + "\r\nConnection: close\r\n\r\n";
        }

        public void SendResponse(Socket clientSocket, string response)
        {
            string header = CreateHeader( response );
            clientSocket.Send( Encoding.UTF8.GetBytes( header ), header.Length, SocketFlags.None );
            clientSocket.Send( Encoding.UTF8.GetBytes( response ), response.Length, SocketFlags.None );
        }

        public void Dispose()
        {
            if( socket != null )
                socket.Close();
        }
        #endregion
    }
}
