using System;
using System.Collections;
using System.Net;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class SyncModule
    {
        private IPAddress _client;
        public SyncModule()
        {
            
        }


        public IPAddress Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        public ArrayList ParsePathList(string list)
        {
            ArrayList pathList = new ArrayList();
            string[] t = list.Split(';');
            foreach (String s in t)
            {
                if (s.Length == 0 || s.Length < 1)
                    continue;
                string angle, duration;
                string[] t2 = s.Split(':');
                if (t2[0] != null && t2[1] != null && t2[2] != null)
                {
                    angle = t2[0];
                    duration = t2[1];

                    Vector2 direction = new Vector2();
                    string[] dir = t2[2].Split(',');
                    var dirX = double.Parse(dir[0]);
                    var dirY = double.Parse(dir[1]);

                    direction.X = dirX;
                    direction.Y = dirY;

                    double angleD = double.Parse(angle);
                    int durationI = int.Parse(duration);

                    pathList.Add(new PathInformation(angleD, durationI, direction));
                }
            }

            return pathList;
        }

        public string SendRequest(string query)
        {
            if (_client != null)
            {
                lock (_client)
                {
                    string response = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://"+_client+"/?" + query);
                        //request.Method = "GET";
                        request.Method = "GET";
                        //request.Proxy = null;
                        //request.Proxy = null;
                        var result = request.GetResponse();
                        response = result.Headers.GetValues("value")[0];
                    }
                    catch (Exception e)
                    {
                        Debug.Print(e.ToString());
                    }
                    return response;

                }

            }
            return null;
        }

        public double GetRadius(Vector2 position, Vector2 destination, Vector2 orientation)
        {
            if (position == null || destination == null || orientation == null)
            {
                return -1;
            }
            string radius =
                SendRequest("GetRadius=" + position.X + ":" + position.Y + ";" + destination.X + ":" + destination.Y +
                            ";" + orientation.X + ":" + orientation.Y);

            double b;
            if (double.TryParse(radius, out b))
            {
                return b;
            }
            return -1;
        }

        public double GetAngle(Vector2 position, Vector2 destination)
        {
            if (position == null || destination == null)
            {
                return -1;
            }
            string angle =
                SendRequest("GetAngle=" + position.X + ":" + position.Y + ";" + destination.X + ":" + destination.Y);
            double b;
            if (double.TryParse(angle, out b))
            {
                return b;
            }
            return -1;
        }

        public double GetDistance(Vector2 position, Vector2 destination)
        {
            if (position == null || destination == null)
            {
                return -1;
            }
            string distance = SendRequest("GetDistance=" + position.X + ":" + position.Y + ";" + destination.X + ":" + destination.Y);
            double b;
            if (double.TryParse(distance, out b))
            {
                return b;
            }
            return -1;
        }
    }
}
