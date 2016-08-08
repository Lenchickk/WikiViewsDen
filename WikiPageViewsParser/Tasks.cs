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
        public static void DoExtraction(DateTime start, DateTime end)
        {
            Common.links = WikiTrickery.GetPageRange(new DateTime(2012, 1, 1), new DateTime(2016, 5, 1));
            Common.interestPages = DataTrickery.DataTableToHashSet(WikiDigger.PostGrePlugIn.getTablePostGre(Common.getPagesSQL));

            
            
 
            MultiThreadTrickery.StartThreads(5);

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
            
            DumpParser.DoParsing(start);

        }

        public static void PrepareData()
        {
            Common.links = WikiTrickery.GetPageRange(new DateTime(2012, 1, 1), new DateTime(2016, 5, 1));
            Common.interestPages = DataTrickery.DataTableToHashSet(WikiDigger.PostGrePlugIn.getTablePostGre(Common.getPagesSQL));
        }
 
    }
}
