using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using WikiDigger;



namespace WikiPageViewsParser
{
    public static class Tasks
    {
        public static DateTime s;
        public static DateTime e;

        public static void DoExtraction(DateTime start, DateTime end)
        {
            s = start;
            e = end;

            System.Threading.Thread mainThread = new System.Threading.Thread(FileKitchen);
            mainThread.Start();

            Common.links = WikiTrickery.GetPageRange(s, e);
            DateTime today = s;

          
            while (Common.links != null)
            {
                //s = new DateTime(2013, 1, 31);
                List<String> thisDay = new List<string>();

                Downloader.GetTask();
                Downloader.GetTask();
                //if (s.Ticks < new DateTime(2013, 1, 31).Ticks) continue;

                for (int i=0; i<Common.countperDay[today]; i++)
                {
                    thisDay.Add(Downloader.GetTask());
                }

                Console.WriteLine(today.ToLongDateString());
                Console.WriteLine();
                Parallel.ForEach<string>(
                    thisDay,
                    new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.999) * 1.0)) },
                         item => Downloader.DownloadStream2UpdateNoTask(item)
                                  );
                today = new DateTime(today.Ticks + TimeSpan.TicksPerDay);
                
            }
            /* Parallel.foreach (var item in Common.interestPages)
             {
                 ;
             }*/

            /*

            MultiThreadTrickery.StartThreads(3);

            while (Downloader.downloaded.Count<2)
            {
                ;
            }

            for (int i = 0; i < 3; i++)
            {
                DecompressionTrickery unwrapper = new DecompressionTrickery();
                System.Threading.Thread unwrapStream = new System.Threading.Thread(unwrapper.DecompressionStream);
                unwrapStream.Start();
            }

            while (DecompressionTrickery.unwrapped.Count < 1)
            {
                ;
            }
            
            DumpParser.DoParsing(start);*/

        }

        public static void FileKitchen()
        {
            
            Common.interestPages = DataTrickery.DataTableToHashSet(WikiDigger.PostGrePlugIn.getTablePostGre(Common.getPagesSQL));

            while (Downloader.downloaded.Count < 2)
            {
                ;
            }

            for (int i = 0; i < 1; i++)
            {
                DecompressionTrickery unwrapper = new DecompressionTrickery();
                System.Threading.Thread unwrapStream = new System.Threading.Thread(unwrapper.DecompressionStream);
                unwrapStream.Start();
            }

            while (DecompressionTrickery.unwrapped.Count < 2)
            {
                ;
            }

            DumpParser.DoParsing(s);
        }

        public static void PrepareData()
        {
            Common.links = WikiTrickery.GetPageRange(new DateTime(2012, 1, 1), new DateTime(2016, 5, 1));
            Common.interestPages = DataTrickery.DataTableToHashSet(WikiDigger.PostGrePlugIn.getTablePostGre(Common.getPagesSQL));
        }
 
    }
}
