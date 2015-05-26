using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Server.Lib
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
            get { return "http://192.168.1.137/rsmart/image_upload/img.jpg"; }
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
