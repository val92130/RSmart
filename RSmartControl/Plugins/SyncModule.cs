using System;
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

        public string SendRequest( string query )
        {
            if( _client != null )
            {
                lock( _client )
                {
                    HTTPRequest http = new HTTPRequest();
                    return http.Send( "http://" + _client.ToString() + "/?" + query );
                }

            }
            return null;
        }

        public double GetRadius( Vector2 position, Vector2 destination, Vector2 orientation )
        {
            return double.Parse(SendRequest("GetRadius="+position.X+","+position.Y+";"+destination.X+","+destination.Y+";"+orientation.X+","+orientation.Y));
        }

        public double GetAngle( Vector2 position, Vector2 destination)
        {
            return double.Parse( SendRequest( "GetRadius=" + position.X + "," + position.Y + ";" + destination.X + "," + destination.Y ) );
        }

        public double GetDistance( Vector2 position, Vector2 destination )
        {
            return double.Parse( SendRequest( "GetDistance=" + position.X + "," + position.Y + ";" + destination.X + "," + destination.Y ) );
        }
    }
}
