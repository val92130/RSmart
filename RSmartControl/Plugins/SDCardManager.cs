using System;
using System.IO;
using Microsoft.SPOT;
using RSmartControl.Plugins;

namespace RSmartControl
{
    public class SDCardManager : ISDCardManager
    {
        public SDCardManager()
        {

        }

        public string Read(String fileName)
        {
            if (!File.Exists(@"SD\" + fileName))
            {
                throw new Exception("File not found : " + fileName + " in method Read");
            }
            using( var filestream = new FileStream( @"SD\" + fileName, FileMode.Open ) )
            {
                StreamReader reader = new StreamReader( filestream );
                string cnt = reader.ReadToEnd();
                reader.Close();
                return cnt;
            }
        }

        public void Write(String content, String fileName)
        {
            using( var filestream = new FileStream( @"SD\" + fileName, FileMode.Create ) )
            {
                StreamWriter streamWriter = new StreamWriter( filestream );
                streamWriter.WriteLine( content );
                streamWriter.Close();
            }
        }

        public void Close()
        {

        }
    }
}
