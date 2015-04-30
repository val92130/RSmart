using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace UserInterface
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private List<string> _requestList;
        DebugLog _debugLog;
        bool _pause = false;
        List<Route> _routes;

        public WebServer( string prefixes, DebugLog debugLog )
        {
            _routes = new List<Route>();
            _debugLog = debugLog;
            InitRoutesConfiguration();
            _requestList = new List<string>();
            if( !HttpListener.IsSupported )
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later." );

            if( prefixes == null || prefixes.Length == 0 )
                throw new ArgumentException( "prefixes" );


            _listener.Prefixes.Add( prefixes );

            _listener.Start();
            _debugLog.Write("Server Started");
        }

        public void InitRoutesConfiguration()
        {
            AddRoute(new Route("test", "true", "Helloooo"));
            AddRoute(new Route("hello", "world", "Hello World"));
            AddRoute(new Route("hello", "rsmart", "Hello Rsmart"));
            _debugLog.Write("Routes initialized");
        }

        public void AddRoute(Route r)
        {
            if(r != null)
            {
                if (!CheckExistingRoute(r))
                {
                    _routes.Add(r);
                    _debugLog.Write("Route added");
                }
                else
                {
                    _debugLog.Write("Trying to write an already existing route");
                }
                
            }
        }



        public void AddRoute(string key, string value, string response)
        {
            Route r = new Route(key, value, response);
            if(!CheckExistingRoute(r))
            {
                _routes.Add(new Route(key, value, response));
                _debugLog.Write("Route added");
            }
            else
            {
                _debugLog.Write("Trying to write an already existing route");
            }
            
        }

        public void SerializeRoutes()
        {
            lock(_routes)
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Route>));
                using (StreamWriter wr = new StreamWriter("../../routes/routes.xml"))
                {
                    xs.Serialize(wr, _routes);
                }
            }

        }

        public bool CheckExistingRoute(Route route)
        {
            foreach (Route r in _routes)
            {
                if (r.Key == route.Key)
                {
                    if (r.Value == route.Value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Route> Routes
        {
            get
            {
                lock(_routes)
                {
                    return _routes;
                }
            }
        }


        public List<string> RequestList
        {
            
            get
            {
                lock (_requestList)
                {
                    return _requestList;
                }
                
            }
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem( ( o ) =>
            {
                _debugLog.Write("Webserver running...");
                try
                {
                    while( _listener.IsListening )
                    {
                        ThreadPool.QueueUserWorkItem( ( c ) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if(!_pause)
                                {
                                string rstr = SendResponse( ctx.Request );
                                _debugLog.Write("Request from : " + ctx.Request.RemoteEndPoint + " : " + ctx.Request.RawUrl);
                                
                                byte[] buf = Encoding.UTF8.GetBytes( rstr );
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.AppendHeader( "Value", rstr );
                                ctx.Response.OutputStream.Write( buf, 0, buf.Length );

                                }
                            }
                            catch(Exception e ) {
                                _debugLog.Write("Server Exception : " + e);
                            }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext() );
                    }
                }
                catch { }
            } );
        }

        public string Status
        {
            get
            {
                if(_pause)
                {
                    return "Paused";
                }
                else
                {
                    return "Online";
                }
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        public string SendResponse( HttpListenerRequest request )
        {
            
            _requestList.Add(request.RawUrl);
            string response = AnalyzeQuery(request.QueryString);
            //string response = ReadFile("../../../../../WebInterface/index.html");
            _debugLog.Write("Response sent to : " + request.RemoteEndPoint + " : " + response);
            return response;
        }

        public string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {

                _debugLog.Write(e.ToString());
            }
            return null;
        }

        public string AnalyzeQuery(NameValueCollection request)
        {
            foreach( string key in request )
            {
                string value = request[key];
                foreach(Route r in _routes)
                {
                    if(r.Key == key)
                    {
                        if(r.Value == value)
                        {
                            return r.Response;
                        }
                    }
                }

            }
            return "Welcome To the RSmart WebServer Interface";
            
        }

        public void Pause()
        {
            if(!_pause)
            {
                _pause = true;
                _debugLog.Write("Server Paused");
            }       

        }

        public void Resume()
        {
            if(_pause)
            {
                _pause = false;
                _debugLog.Write("Server Resumed");
            }
                
        }
    }
}