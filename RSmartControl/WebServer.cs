using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using Microsoft.SPOT.Hardware;
using System.Net;
using System.Text;
using System.Threading;
using RSmartControl.Robot_Behaviours;
using SecretLabs.NETMF.Hardware.Netduino;


namespace RSmartControl
{
    using System.Collections;

    public class WebServer : IDisposable
    {
        Communication _com;
        private Socket socket = null;
        private OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
        private IPAddress _synchronizedClientIp;
        private SyncModule _syncModule;
        private PluginManager _pluginManager;
        public WebServer(PluginManager pluginManager)
        {
            _pluginManager = pluginManager;
            _syncModule = pluginManager.SyncModule;
            IPAddress ip = IPAddress.Parse("192.168.1.131");
            _com = pluginManager.CommunicationModule;
            //Initialize Socket class
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Request and bind to an IP from DHCP server
            socket.Bind(new IPEndPoint(ip, 80));
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
                    IPEndPoint clientIp = clientSocket.RemoteEndPoint as IPEndPoint;
                    EndPoint clientEndPoint = clientSocket.RemoteEndPoint;
                    int bytesReceived = clientSocket.Available;
                    if (bytesReceived > 0)
                    {
                        //Get request
                        byte[] buffer = new byte[bytesReceived];
                        int byteCount = clientSocket.Receive(buffer, bytesReceived, SocketFlags.None);
                        string request = new string(Encoding.UTF8.GetChars(buffer));

                        if (clientIp != null && (_synchronizedClientIp != null && clientIp.Address.ToString() == _synchronizedClientIp.ToString()))
                        {
                            //Analyze the message then send a response
                            MessageAnalyse( Utility.ParseQueryString( request ), clientSocket, clientIp );                           
                        } else if (_synchronizedClientIp == null)
                        {
                            MessageAnalyse( Utility.ParseQueryString( request ), clientSocket, clientIp );    
                        }
                        else
                        {
                            const string response = "Someone is already synchronized";
                            SendResponse( clientSocket, response );
                        }
                        
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

        public void MessageAnalyse( Hashtable nvc, Socket clientSocket, IPEndPoint clientIp )
        {
            // if the robot is not yet initialized, we leave
            if (_com.Robot == null)
            {
                return;
            }

            if (nvc == null)
            {
                const string response = "Welcome to RSAMART VAL AND RAMI SAY HELLO TO YOU";
                SendResponse( clientSocket, response );
                return;
            }

            if (_synchronizedClientIp != null && _synchronizedClientIp.ToString() != clientIp.Address.ToString() )
            {
                string response = "Someone is already synchronized";
                SendResponse( clientSocket, response );
                return;
            }

            foreach (DictionaryEntry entry in nvc)
            {
                string response;
                switch ((string)entry.Key)
                {
                    case "FrontMethod" :
                        response = "Front Method ok";
                        SendResponse( clientSocket, response );
                        _pluginManager.BehaviourControl.FrontMethod = (string) entry.Value;
                        break;
                    case "FrontLeftMethod":
                        response = "Front Left Method ok";
                        SendResponse( clientSocket, response );
                        _pluginManager.BehaviourControl.FrontLeftMethod = (string)entry.Value;
                        break;
                    case "FrontRightMethod":
                        response = "Front Right Method ok";
                        SendResponse( clientSocket, response );
                        _pluginManager.BehaviourControl.FrontRightMethod = (string)entry.Value;
                        break;
                    case "BackMethod":
                        response = "Back Method ok";
                        SendResponse( clientSocket, response );
                        _pluginManager.BehaviourControl.BackMethod = (string)entry.Value;
                        break;
                    case "GetBehaviours" :
                        response = BehaviourControl.Behaviours;
                        SendResponse(clientSocket, response);
                        break;
                    case "AddObstacle" :
                        try
                        {
                            string[] str = ((string) entry.Value).Split(';');
                            if (str[0] != null && str[1] != null)
                            {
                                double x = Double.Parse(str[0]);
                                double y = Double.Parse(str[1]);
                                _com.AddObstacle(new Vector2(x,y));
                            }
                            SendResponse(clientSocket, "Obstacle added ");
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.ToString());
                        }
                        break;

                    case "GetMethods" :
                        response = "";
                        foreach (string s in BehaviourControl.GetAllMethods())
                        {
                            response += s + "-";
                        }
                        response = response.Substring(0, response.Length - 1);
                        SendResponse(clientSocket, response);
                        break;

                    case "SendPoints":
                        string val = (string)entry.Value;

                        if (val != null && val.Length != 0)
                        {
                            ArrayList pts = Vector2.FetchPointsQuery(val);
                            ArrayList path = Utility.GetPathList(_com.Robot, pts);
                            if (path != null)
                            {
                                _com.Robot.FollowPath(path);
                                SendResponse(clientSocket, "Sending points to robot..");
                                break;
                            }
                            SendResponse(clientSocket, "Unexpected error");

                            break;
                        }
                        SendResponse(clientSocket, "Invalid points");
                        break;

                    case "GetDirectionString":
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
                    case "GoForwardTime" :
                        try
                        {
                            double time = double.Parse( (string)entry.Value );
                            SendResponse(clientSocket, "Going forward for " + time + " seconds ");
                            _com.Robot.GoForward(time);
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.ToString());
                        }
                        break;

                    case "SetDirection" :
                        try
                        {
                            int direction = int.Parse( (string)entry.Value );
                            _com.Robot.DirectionOffset = direction;
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.ToString());
                        }
                        break;

                    case "Synchronize":
                        IPAddress ip = null;
                        try
                        {
                            ip = IPAddress.Parse((string) entry.Value);
                        }
                        catch (Exception e)
                        {
                            Debug.Print("Exception in ip synchronization : " + e.ToString());
                        }

                        if (ip != null)
                        {
                            _syncModule.Client = ip;
                            _synchronizedClientIp = ip;
                        }
                        response = "Client synchronized : " + _synchronizedClientIp.ToString();
                        SendResponse( clientSocket, response );
                        break;

                    case "Desynchronize" :
                        if (_synchronizedClientIp == null)
                        {
                            SendResponse(clientSocket,"You are not linked with the robot");
                            break;
                        }
                        string tmp = _synchronizedClientIp.ToString();
                        if ((string) entry.Value == _synchronizedClientIp.ToString())
                        {
                            _syncModule.Client = null;
                            _synchronizedClientIp = null;
                        }
                        response = "Client unsynchronized : " + tmp;
                        SendResponse( clientSocket, response );
                        break;

                    case "GetOrientation":
                        response = _com.MainLoop.Robot.Orientation.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetOrientationX":
                        response = _com.MainLoop.Robot.Orientation.X.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetOrientationY":
                        response = _com.MainLoop.Robot.Orientation.Y.ToString();
                        SendResponse( clientSocket, response );
                        break;

                    case "GetDutyCycleLeft":
                        response = _com.MotorLeft.DutyCycle.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetDutyCycleRight":
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
                    case "Start":
                        if( (string)entry.Value == "true" )
                        {
                           _com.MotorLeft.Start();
                           _com.MotorRight.Start();
                           response = "Motors started";
                           SendResponse( clientSocket, response );
                        }
                        
                        break;
                    case "Stop":
                        if( (string)entry.Value == "true" )
                        {
                            _com.MotorLeft.Stop();
                            _com.MotorRight.Stop();
                            response = "Motors stopped";
                            SendResponse( clientSocket, response );
                        }
                        break;

                    case "TurnRightSecond" :
                        try
                        {
                            int time = int.Parse( (string)entry.Value );
                            _com.Robot.TurnRight(time);
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.ToString());
                        }
                        break;

                    case "TurnLeftSecond":
                        try
                        {
                            int time = int.Parse((string)entry.Value);
                            _com.Robot.TurnLeft(time);
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.ToString());
                        }
                        break;


                    case "Rotate":

                        if( (string)entry.Value == "left" )
                        {
                            _com.MotorLeft.Stop( Motor.TimeAngleRotation( _pluginManager.SpeedDetectionModuleModule.Speed, 90 ) );
                            response = "Turning left";
                            SendResponse( clientSocket, response );
                            break;
                        }

                        if( (string)entry.Value == "right" )
                        {
                            _com.MotorRight.Stop(Motor.TimeAngleRotation(_pluginManager.SpeedDetectionModuleModule.Speed, 90));
                            response = "Turning right";
                            SendResponse( clientSocket, response );
                            break;
                        }
                        
                        //try
                        //{
                        //    int t = int.Parse((string)(entry.Value));
                        //}
                        //catch(Exception e)
                        //{
                        //    Debug.Print(e.ToString());
                        //}

                        break;

                    case "RotateAngle" :
                        try
                        {
                            int angle = int.Parse((string)entry.Value);
                            _com.Robot.TurnAngle(angle);
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.ToString());
                        }
                        break;

                    case "TestPath" :
                        ArrayList arr = new ArrayList();
                        arr.Add(new PathInformation(90, 4000, new Vector2(0,0)));
                        arr.Add(new PathInformation(45, 2000, new Vector2(0, 0)));
                        arr.Add(new PathInformation(180, 3000, new Vector2(0, 0)));
                        _com.Robot.FollowPath(arr);
                        break;

                    case "FollowPath" :
                        ArrayList pathList = _pluginManager.SyncModule.ParsePathList((string) entry.Value);
                        if (pathList != null)
                        {
                            _com.Robot.FollowPath(pathList);
                            SendResponse(clientSocket, "Sending path to robot...");
                        }
                        else
                        {
                            SendResponse(clientSocket, "Incorrect path ! ");
                        }
                        break;

                    case "TestCom":
                        string t1 = _pluginManager.SyncModule.SendRequest("test=true");
                        double _angle = _pluginManager.SyncModule.GetAngle(new Vector2(1, 1), new Vector2(20, 50));
                        double _radius = _pluginManager.SyncModule.GetRadius(new Vector2(1, 1), new Vector2(20, 50), _com.Robot.Orientation);
                        double _length = _pluginManager.SyncModule.GetDistance(new Vector2(1, 1), new Vector2(20, 50));

                        SendResponse(clientSocket, t1 + " : " + _angle + " : " + _radius + " : " + _length );
                        break;  

                    case "Speed":
                        if( entry.Value == null )
                            return;
                        int speed;
                        try
                        {
                            speed = Convert.ToInt32( (string)entry.Value );
                        }
                        catch( Exception e )
                        {
                            Debug.Print( e.ToString() );
                            return;
                        }

                        if( speed < 0 || speed > 100 )
                            return;
                        _com.MotorLeft.DutyCycle = (double)((double)(speed) / (double)(100));
                        _com.MotorRight.DutyCycle = (double)((double)(speed) / (double)(100));
                        response = "Speed changed to : " + speed.ToString();
                        SendResponse( clientSocket, response );
                        break;
                    case "GetSpeed":
                        response = _pluginManager.SpeedDetectionModuleModule.Speed.ToString();
                        SendResponse(clientSocket, response);
                        break;
                    case "Forward":
                        if( (string)entry.Value == "true" )
                        {
                            _com.Robot.GoForward();
                            response = "Robot going forward";
                            SendResponse( clientSocket, response );
                        }
                        break;
                    case "Backward":
                        if( (string)entry.Value == "true" )
                        {
                            _com.Robot.GoBackward();
                            response = "Robot going backward";
                            SendResponse( clientSocket, response );
                        }
                        break;
                    case "Right":
                        if( (string)entry.Value == "true" )
                        {
                            _com.Robot.TurnRight();
                            response = "Robot turning right";
                            SendResponse( clientSocket, response );
                        }
                        break;
                    case "Left":
                        if( (string)entry.Value == "true" )
                        {
                            _com.Robot.TurnLeft();
                            response = "Robot turning left";
                            SendResponse( clientSocket, response );
                        }
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
