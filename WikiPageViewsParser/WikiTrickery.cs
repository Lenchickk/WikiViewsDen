using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WikiPageViewsParser
{
    public static class WikiTrickery
    {
        public static List<String> GetPageRange(DateTime start, DateTime end)
        {
            List<String> buf = new List<string>();
            List<String> links = new List<String>();
               DateTime cursor = new DateTime(start.Ticks);
            String link;
            
            while (cursor.Ticks<=end.Ticks)
            {
                link = Common.baseLink + cursor.Year.ToString() + "/" + cursor.Year.ToString() + "-";
                if (cursor.Month < 10) link += "0";
                link += cursor.Month.ToString() + "/";
                buf.Add(link);
                cursor = PlusMonth(cursor);
            }

            WebClient w = new WebClient();
            foreach (String page in buf)
            {
                String s = w.DownloadString(page);

                foreach (LinkItem l in LinkFinder.Find(s))
                {
                    if (l.Href[0] != 'p') continue;
                    links.Add(page + l.Href);
                }

            }
            return links;
        }

        static DateTime PlusMonth(DateTime dt)
        {
            Int32 month = dt.Month;
            if (month == 12)
            {
                return (new DateTime(dt.Year + 1, 1, 1));
            }

            return (new DateTime(dt.Year, month + 1, 1));
        }
    }
}
