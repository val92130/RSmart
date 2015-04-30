using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.SPOT;

namespace RSmartControl
{
    class HTTPException : Exception
    {
        int code;

        public HTTPException(int code)
            : base("HTTP error " + code.ToString())
        {
            this.code = code;
        }

        public HTTPException()
            : base("Invalid HTTP reply")
        {
            code = -1;
        }
    }

    class HTTPRequest : Stream
    {
        // socket used for HTTP connection
        protected Socket httpsocket;

        // content length
        protected long size;

        // current position
        protected long position;

        public HTTPRequest()
        {
            size = 0;
            position = 0;
        }

        /**
         * We need a custom readline function. Using a standard textreader could prevent usage of other kind of readers (XML etc.) on the stream.
         */
        protected string ReadLine()
        {
            string line = "";

            byte[] data = new byte[1];

            while (Read(data, 0, 1) != 0)
            {
                // checks for carriage return
                if (data[0] == 0x0d)
                {
                    // discards line feed
                    Read(data, 0, 1);

                    // returns the string
                    return line;
                }

                char[] chars = UTF8Encoding.UTF8.GetChars(data);

                line += chars[0];
            }
            return line; 
        }

        /**
         * Sends an HTTP reply and initializes the stream that may be used to read the reply
         */
        public void Send(string url)
        {
            // parses the url, splitting server and path
            int serverindex = url.IndexOf("//");

            if (serverindex == -1)
                throw new ArgumentException("Invalid URL, can't find server name.");

            string server;
            string path;

            int port = 80;

            int pathindex = url.IndexOf("/", serverindex + 2);

            if (pathindex == -1)
                throw new ArgumentException("Invalid URL, can't find resource path.");

            int portindex = url.IndexOf(":", serverindex + 2);

            // port is not mandatory, if no port is specified, port 80 is used
            if (portindex != -1)
            {
                string portstring = url.Substring(portindex + 1, pathindex - portindex - 1);

                server = url.Substring(serverindex + 2, portindex - serverindex - 2);
                path = url.Substring(pathindex + 1);
                port = Int32.Parse(portstring);
            }
            else
            {
                server = url.Substring(serverindex + 2, pathindex - serverindex - 2);
                path = url.Substring(pathindex);
            }

            // tries to solve server name
            IPHostEntry host = Dns.GetHostEntry(server);

            // connects to the server
            httpsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // builds up the HTTP request
            string requestheader = "GET " + path + " HTTP/1.1\r\nHost:" + server + "\r\nConnection: keep-alive\r\nAccept-Charset: utf-8;\r\nPragma:    no-cache\r\nCache-Control: no-cache\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n\r\n";

            httpsocket.Connect(new IPEndPoint(host.AddressList[0], port));

            Byte[] requestarray = Encoding.UTF8.GetBytes(requestheader);

            httpsocket.Send(requestarray);

            // parses the HTTP reply
            string line = ReadLine();

            // the first line contains the return code
            if (line.IndexOf("HTTP") != 0)
                throw new HTTPException();

            string codestring = line.Substring(line.IndexOf(" ") + 1, 3);

            int httpcode = Int32.Parse(codestring);

            if (httpcode != 200)
                throw new HTTPException(httpcode);

            while (line.Length != 0)
            {
                line = ReadLine();
                if (line.StartsWith("Value: "))
                {
                    line = line.Substring(7, line.Length - 7);
                    break;
                }

                //searches content lenght header
            }

            Debug.Print("Response : " + line);

            // position starts to count from here
            position = 0;

            // does not close the stream (there's no way to detach it from the socket)
        }

        public override void Close()
        {
            if (httpsocket != null)
                httpsocket.Close();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return size; }
        }

        public override long Position
        {
            get { return position; }
            set { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int readt = httpsocket.Receive(buffer, offset, count, SocketFlags.None);

            position += readt;
            return readt;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
    }
}