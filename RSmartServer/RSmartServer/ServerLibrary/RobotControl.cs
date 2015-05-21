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
        WebcamManager _webCamManager;
        public RobotControl(string robotIp)
        {
            _webCamManager = new WebcamManager();
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

        public DebugLog DebugLog
        {
            get
            {
                return _debugLog;
            }
        }

        public WebServer WebServer
        {
            get
            {
                return _server;
            }
        }

        public WebcamManager WebCamManager
        {
            get
            {
                return _webCamManager;
            }
        }

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

        public string SendRequestRobot(string req)
        {
            if (PingRobot())
            {
                return SendRequest(_robotIp, req + "&robot=true");
            }
            return null;
        }

        public string SendRequest(string ip, string req)
        {

            string url = "http://" + ip + "/?" + req;
            string rep = Util.GET(_debugLog, url);
            _debugLog.Write("Response from " + ip + " : " + rep);
            return rep;
        }
    }
}
