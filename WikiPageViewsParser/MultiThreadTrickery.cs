using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WikiPageViewsParser
{
    static public class MultiThreadTrickery
    {
        static public void StartThreads(int k)
        {


            for (int i=0; i<k; i++)
            {
                Downloader downloader = new Downloader();
                Thread downloadThread = new Thread(downloader.DownloadStream2Update);
                downloadThread.Start();
                Common.downloaderThreads.Add(downloadThread);
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
