using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WikiPageViewsParser
{
    public class DecompressionTrickery
    {
        public static volatile int decompressedFiles = 0;
        public static volatile int totalNumber = -1;
        public int myNumber;
        public static volatile char[] delimiterLast = { '\\' };
        static public volatile List<String> startedUnwrap = new List<String>();
        static public volatile List<String> unwrapped = new List<String>();


        public DecompressionTrickery()
        {
            totalNumber++;
            myNumber = totalNumber;
        }

        public void DecompressionStream()
        {
            String shortname = "";
            String[] helper;
            String[] viewsFiles = System.IO.Directory.GetFiles(Common.pile, "*.gz");

            Console.WriteLine("Unwrapper started" + myNumber);

            while (Downloader.compressedFiles>0)
            
            {
                foreach (String file in viewsFiles)
                {
                   /* while (Common.pendingCondition())
                    {
                        ;
                    }
                    */
                    helper = file.Split(delimiterLast);
                    shortname = helper[helper.Length - 1];
                    String outstringLong = file.Substring(0, file.Length - shortname.Length);

                    if (!Downloader.downloaded.Contains(shortname)) continue;
                    if (startedUnwrap.Contains(shortname)) continue;

                    startedUnwrap.Add(shortname);
                    string outname = shortname + ".out";
                    outstringLong+=outname;

                    Execute("gzip -dc  " + shortname + "  >  " + outname + " ");

                    while (true)
                    {
                        try { System.IO.File.Delete(file); goto exit; }
                        catch(Exception ex)
                        {

                        }
                    }
                exit:;
                    /*
                    while (true)
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Trying to delete " + file);
                        }
                    }*/

                    unwrapped.Add(outstringLong);
                    Downloader.compressedFiles--;
                    decompressedFiles++;




                }
                viewsFiles = System.IO.Directory.GetFiles(Common.pile, "*.gz");

            }

            Console.WriteLine(myNumber+ " unwrapper ended");

        }


        static public void Execute(String command)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            do {; } while (!proc.HasExited);
        }
    }
}
