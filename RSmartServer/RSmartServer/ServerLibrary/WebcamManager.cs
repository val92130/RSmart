using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class WebcamManager
    {
        Thread downloader;
        public WebcamManager()
        {  
            downloader = new Thread(() =>
            {
                Stopwatch s = new Stopwatch();
                s.Start();
                while(true)
                {
                    if(s.ElapsedMilliseconds == 3000)
                    {
                        this.Update(null);
                        s.Restart();
                    }
                }
                
            });
        }

        public static string ImagePath
        {
            get { return "http://10.8.96.153/img.jpg"; }
        }

        public void Start()
        {
            downloader.Start();
        }

        private void Update(object state)
        {
            string localFilename = WebcamManager.Path;
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://www.tradeit.fr/Webcam/image_upload/img.jpg", localFilename);
                }
                catch 
                {
                    
                }
            }
        }

        public static string Path
        {
            get
            {
                return "../../../Images/cam.jpg";
            }
        }

    }
}
