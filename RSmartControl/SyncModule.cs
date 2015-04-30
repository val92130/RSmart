using System;
using System.Net;
using Microsoft.SPOT;

namespace RSmartControl
{
    public class SyncModule
    {
        private MainLoop _mainLoop;
        private IPAddress _client;
        private HTTPRequest _request;
        private WebServer _server;
        public SyncModule(MainLoop mainloop, WebServer server)
        {
            _request = new HTTPRequest();
            _mainLoop = mainloop;
            _server = server;
        }

        public void Run()
        {
            
        }

        public IPAddress Client
        {
            get
            {
                lock (_client)
                {
                    return _client;
                }
            }
            set
            {
                lock (_client)
                {
                    _client = value;
                }
            }
        }

        public string SendRequest(string url)
        {
            return null;
        }
    }
}
