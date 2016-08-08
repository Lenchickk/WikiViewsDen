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

            for (DateTime dt = start; dt.Ticks <= new DateTime(end.Year,end.Month, DateTime.DaysInMonth(end.Year,end.Month)).Ticks; dt = new DateTime(dt.Ticks + TimeSpan.TicksPerDay))
                Common.countperDay.Add(dt, 0);

            WebClient w = new WebClient();
            cursor  = new DateTime(start.Ticks);
            foreach (String page in buf)
            {
                String s = w.DownloadString(page);

                foreach (LinkItem l in LinkFinder.Find(s))
                {
                    if (l.Href[0] != 'p') continue;
                    links.Add(page + l.Href);
                    Common.countperDay[DateTime.ParseExact(l.Href.Split('-')[1], "yyyyMMdd", null)]++;
                   
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
