using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private List<string> _requestList;

        public WebServer( string prefixes )
        {
            _requestList = new List<string>();
            if( !HttpListener.IsSupported )
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later." );

            if( prefixes == null || prefixes.Length == 0 )
                throw new ArgumentException( "prefixes" );


            _listener.Prefixes.Add( prefixes );

            _listener.Start();
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
                Console.WriteLine( "Webserver running..." );
                try
                {
                    while( _listener.IsListening )
                    {
                        ThreadPool.QueueUserWorkItem( ( c ) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = SendResponse( ctx.Request );
                                
                                byte[] buf = Encoding.UTF8.GetBytes( rstr );
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write( buf, 0, buf.Length );
                            }
                            catch { }
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

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        public string SendResponse( HttpListenerRequest request )
        {
            AnalyzeQuery( request.QueryString );
            string resp = "";
            for( int i = 0; i < request.QueryString.AllKeys.Length; i++ )
            {
                resp = resp + request.QueryString.AllKeys[i] + "<br/>";
            }
            _requestList.Add(request.RawUrl);
            return string.Format( "<HTML><BODY>My web page.<br>{0}</BODY></HTML>", request.RawUrl );
        }

        public void AnalyzeQuery(NameValueCollection request)
        {
            foreach( string key in request )
            {
                switch (key)
                {
                    case "Test" :
                        switch (request[key])
                        {
                            case "true" :
                                break;
                            case "false" :
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}