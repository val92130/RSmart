using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Server.Lib
{
    public class Util
    {
        /// <summary>
        /// Returns the robot IP
        /// </summary>
        public static string RobotIp
        {
            get
            {
                return "192.168.1.132";
            }
        }

        /// <summary>
        /// Check if a url exists
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool RemoteFileExists(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp
                              || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        /// <summary>
        /// Returns the local IP of the server
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Sends a GET request and returns the response
        /// </summary>
        /// <param name="debugLog">DebugLog object</param>
        /// <param name="url">Destination url</param>
        /// <returns></returns>
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
