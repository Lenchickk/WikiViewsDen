using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WikiPageViewsParser
{
    public class Downloader
    {
        public String task;
        public int mynumber;
        static volatile public int countofus = -1;
        public static volatile int rawFiles = 0;
        public static volatile int compressedFiles = 0;
        static volatile char[] delimiterChars = { '-', ' ' };
        static public volatile List<String> startedDownload = new List<string>();
        static public volatile List<String> downloaded = new List<string>();
        static public Boolean hasOneDownloadedFile = false;
        public Downloader()
        {
            countofus++;
            mynumber = countofus;
        }

        public void DownloadStream2Update()
        {
            WebClient w = new WebClient();
            Int16 counter = -1;

            while ((task = GetTask())!="done")
            {
                /*while (Common.pendingCondition())
                {
                    ;
                }
                */
                String[] buff = task.Split('/');
                String myname = buff[buff.Length - 1];
                String[] check = myname.Split(delimiterChars);

                String to = Common.pile + myname;

                if (startedDownload.Contains(myname)) continue;

                startedDownload.Add(myname);

                rawFiles++;

               if (System.IO.File.Exists(to) || DecompressedFileIntheFolder(myname) && !downloaded.Contains(myname)) goto alreadyHaveFile;

                Console.WriteLine("I am " + mynumber.ToString() + " and I am downloading " + myname);


            trymore:
                try
                {
                    w.DownloadFile(task,  to);
                }
                catch (System.Net.WebException ex)
                {
                    System.Threading.Thread.Sleep(10000);
                    goto trymore;
                }
            alreadyHaveFile:;
                      
                downloaded.Add(myname);
                rawFiles--;
                compressedFiles++;
            }

            Console.WriteLine("I am " + mynumber.ToString() + "and I am done.");
        }

        static bool DecompressedFileIntheFolder(String name)
        {
            String[] viewsFiles = System.IO.Directory.GetFiles(Common.pile, "*.out");

            foreach (String file in viewsFiles)
            {
                if (file.Contains(name)) return true;
            }

            return false;
        }

        static bool CompressedFileIntheFolder(String name)
        {
            String[] viewsFiles = System.IO.Directory.GetFiles(Common.pile, "*.gz");

            foreach (String file in viewsFiles)
            {
                if (file==name) return true;
            }

            return false;
        }

        public static volatile Int32 totalcount = 0;
        public static void DownloadStream2UpdateNoTask(String task)
        {
            WebClient w = new WebClient();
            //Int16 counter = -1;

            while (task  != "done")
            {
                /*while (Common.pendingCondition())
                {
                    ;
                }
                */
                String[] buff = task.Split('/');
                String myname = buff[buff.Length - 1];
                String[] check = myname.Split(delimiterChars);

                String to = Common.pile + myname;

                if (startedDownload.Contains(myname)) continue;

                if (CompressedFileIntheFolder(to) && !startedDownload.Contains(myname))
                    goto alreadyHaveDecompressedFile;

                if (DecompressedFileIntheFolder(myname) && !downloaded.Contains(myname))
                {
                    DecompressionTrickery.decompressedFiles++;
                    DecompressionTrickery.unwrapped.Add(to+".out");
                    goto alreadyHaveFile2;
                }


                startedDownload.Add(myname);

                rawFiles++;
                totalcount++;

                Console.WriteLine(totalcount.ToString() + " I am downloading " + myname);

            trymore:
                try
                {
                    w.DownloadFile(task, to);
                }
                catch (System.Net.WebException ex)
                {
                    System.Threading.Thread.Sleep(10000);
                    goto trymore;
                }
                rawFiles--;
            alreadyHaveDecompressedFile:;
                downloaded.Add(myname);
                compressedFiles++;
            alreadyHaveFile2:;
                return;
            }

            //Console.WriteLine("I am " + mynumber.ToString() + "and I am done.");
        }



        public static string GetTask()
        {
            if (Common.links.Count == 0) return "done";
            String myTask = Common.links[0];
            Common.links.RemoveAt(0);
            return myTask;
        }

    }
}
