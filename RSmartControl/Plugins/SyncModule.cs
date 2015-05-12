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

        public void SendRequest(string query)
        {
                if( _client != null )
                {
                    lock (_client)
                    {
                        HTTPRequest http = new HTTPRequest();
                        http.Send( "http://" + _client.ToString() + "/?" + query );
                    }
                    
                }
            }
    }
}
