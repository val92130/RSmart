using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Server.Lib
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

            if( string.IsNullOrEmpty(prefixes) )
                throw new ArgumentException( "Prefixes must be specified" );

            _listener.Prefixes.Add( prefixes );

            _listener.Start();
            _debugLog.Write("Server Started", EMessageCategory.Success);

        }

        public void InitRoutesConfiguration()
        {
            this.DeserializeRoutes();
            AddRoute(new Route("GetInfo", "true", "This is the RSmart Web Server"));
            AddRoute(new Route("CalculateAverage", "values", "Returns the average value"));
            _debugLog.Write("Routes initialized", EMessageCategory.Success);
            this.SerializeRoutes();
        }

        public void AddRoute(Route r)
        {
            if(r != null)
            {
                if (!CheckExistingRoute(r))
                {
                    _routes.Add(r);
                    _debugLog.Write("Route added");
                    this.SerializeRoutes();
                    
                }
                else
                {
                    _debugLog.Write("Trying to write an already existing route", EMessageCategory.Error);
                }
                            
            }
        }


        public void AddRoute(string key, string value, string response)
        {
            Route r = new Route(key, value, response);
            this.AddRoute( r );           
        }

        public void SerializeRoutes()
        {
            lock(_routes)
            {
                try
                {
                    using( Stream stream = File.Open( "../../../Routes/routes.bin", FileMode.Create ) )
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize( stream, _routes );
                    }
                }
                catch( IOException ex )
                {
                    _debugLog.Write( ex.ToString(), EMessageCategory.Error );
                }
            }

        }
       

        public void DeserializeRoutes()
        {
            try
            {
                if (!File.Exists("../../../Routes/routes.bin"))
                    return;
                using (Stream stream = File.Open("../../../Routes/routes.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    this._routes =  (List<Route>)bin.Deserialize( stream );
                }
            }
            catch( IOException ex )
            {
                _debugLog.Write( ex.ToString() );
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
                _debugLog.Write("Webserver running...", EMessageCategory.Information);
                try
                {
                    while (_listener.IsListening)
                    {
                        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                        {
                            return;
                        }
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (!_pause)
                                {
                                    string rstr = SendResponse(ctx.Request);
                                    _debugLog.Write(
                                        "Request from : " + ctx.Request.RemoteEndPoint + " : " + ctx.Request.RawUrl,
                                        EMessageCategory.Information);

                                    byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                    ctx.Response.ContentLength64 = buf.Length;
                                    ctx.Response.AppendHeader("Value", rstr);
                                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);

                                }
                            }
                            catch (Exception e)
                            {
                                _debugLog.Write("Server Exception : " + e, EMessageCategory.Error);
                            }
                            finally
                            {
                                if (ctx != null) ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch(Exception e)
                {
                    _debugLog.Write(e.ToString(), EMessageCategory.Error);
                }
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
            _debugLog.Write("Response sent to : " + request.RemoteEndPoint + " : " + response, EMessageCategory.Success);
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
            string resp = "<html>Welcome To the RSmart WebServer Interface";
            foreach (Route r in _routes)
            {
                resp += "<br/> <a href=\"" + Util.GetIp() + "?" + r.Key + "=" + r.Value + "\">" + r.Key + "<a/>";
            }
            resp += "</html>";
            return resp;
            
        }

        public void Pause()
        {
            if(!_pause)
            {
                _pause = true;
                _debugLog.Write("Server Paused", EMessageCategory.Success);
            }       

        }

        public void Resume()
        {
            if(_pause)
            {
                _pause = false;
                _debugLog.Write("Server Resumed", EMessageCategory.Success);
            }
                
        }
    }
}