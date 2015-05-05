using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class Util
    {
        public static string RobotIp
        {
            get
            {
                return "10.8.110.204";
            }
        }

        public static bool PingRobot()
        {
            var ping = new Ping();

            PingReply reply = ping.Send( IPAddress.Parse( RobotIp ), 50 );

            if( reply.Status == IPStatus.Success )
            {
                return true;
            }
            return false;
        }
        public static bool RemoteFileExists(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp
                              || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public static string GetIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        public static string GET(DebugLog debugLog, string url)
        {
            try
            {
                debugLog.Write("Sending request : " + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                string data = reader.ReadToEnd();

                reader.Close();
                stream.Close();

                return data;
            }
            catch (Exception e)
            {
                debugLog.Write(e.ToString());
            }

            return null;
        }

    }
}
