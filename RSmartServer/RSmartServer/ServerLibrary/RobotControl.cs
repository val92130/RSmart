using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace Server.Lib
{
    public class RobotControl
    {
        string _robotIp;
        WebServer _server;
        string _ip;
        DebugLog _debugLog;

        /// <summary>
        /// Creates a new instance of a RobotControl
        /// </summary>
        /// <param name="robotIp"></param>
        public RobotControl(string robotIp)
        {
            _robotIp = robotIp;
            _debugLog = new DebugLog();
            _ip = Util.GetIp();

            Thread serverThread = new Thread(() =>
            {
                _server = new WebServer("http://" + _ip + ":80/", _debugLog);
                _server.Run();
            });

            serverThread.Start();
        }


        public RobotControl()
        {
            _robotIp = Util.RobotIp;
            _debugLog = new DebugLog();
            _ip = Util.GetIp();
        }

        /// <summary>
        /// Returns the debug log
        /// </summary>
        public DebugLog DebugLog
        {
            get
            {
                return _debugLog;
            }
        }

        /// <summary>
        /// Returns the WebServer object
        /// </summary>
        public WebServer WebServer
        {
            get
            {
                return _server;
            }
        }


        /// <summary>
        /// Ping the robot, return true if the ping received a response, else returns false
        /// </summary>
        /// <returns></returns>
        public bool PingRobot()
        {
            var ping = new Ping();

            PingReply reply = ping.Send(IPAddress.Parse(_robotIp), 50);

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends a request to the robot and returns the response
        /// </summary>
        /// <param name="req">Request string</param>
        /// <returns></returns>

        public string SendRequestRobot(string req)
        {
            if (PingRobot())
            {
                return SendRequest(_robotIp, req + "&robot=true");
            }
            return null;
        }

        /// <summary>
        /// Sends a request to the specified IP address
        /// </summary>
        /// <param name="ip">Destination IP</param>
        /// <param name="req">Request string</param>
        /// <returns></returns>
        public string SendRequest(string ip, string req)
        {

            string url = "http://" + ip + "/?" + req;
            string rep = Util.GET(_debugLog, url);
            _debugLog.Write("Response from " + ip + " : " + rep, EMessageCategory.Information);
            return rep;
        }
    }
}
