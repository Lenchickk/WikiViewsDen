using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WikiPageViewsParser
{
    class SkippableStreamReader : StreamReader
    {
        public SkippableStreamReader(string path) : base(path) { }

        public void SkipLines(int linecount)
        {
            for (int i = 0; i < linecount; i++)
            {
                this.ReadLine();
            }
        }
    }


    public static class DumpParser
    {
        static public Int64 totalChangestoday = 0;
        static public Int64 totalBytesInChangedFilesToday = 0;
        public static Dictionary<String, List<Int64>> interestPageslocal = PagesLocalLoad();

        static public void DailyResultToFile(DateTime dt)
        {
            String str = dt.ToString("yyyy-MM-dd")+"\t";
            StreamWriter sw = new StreamWriter(Common.outputFile, true);
            foreach (String key in interestPageslocal.Keys)
            {
                str += key + "\t" + interestPageslocal[key][0].ToString() +  "\t" + interestPageslocal[key][1].ToString();
                sw.WriteLine(str);
            }
            str = dt.ToString("yyyy-MM-dd") + "\tother\t" + totalChangestoday.ToString() + "\t" + totalBytesInChangedFilesToday.ToString();
            sw.WriteLine(str);
            sw.Close();
        }

        public static void DoParsing(DateTime start)
        {
            String[] buf;
            String shortname;
            Int32 buffersize = 40000;//4095 * 256;// *16;
            char[] buffer = new char[buffersize];
            DateTime currentDate;
            DateTime previousDate = new DateTime(start.Ticks);
            char[] delimiterLast = { '-' };
            char[] delimitersmall = { '\\' };
            String[] viewsFiles = Directory.GetFiles(Common.pile, "*.out");
            Console.WriteLine("Parsing started!");
            String targetDomain = "ru";
            byte previousHour = 0;
            byte currentHour = 0;

            while (Common.links.Count>0)
            {
                foreach (String file in viewsFiles)
                {
                    buf = file.Split(delimitersmall);
                    shortname = buf[buf.Length - 1];

                    buf = file.Split(delimiterLast);
                    currentDate = DateTime.ParseExact(buf[1], "yyyyMMdd", null);

                    currentHour = Byte.Parse(buf[2].Substring(0, 2));
                    if (currentHour < previousHour)
                    {
                        DailyResultToFile(previousDate);
                        NullLocalPages();
                        previousHour = currentHour;
                        previousDate = new DateTime(currentDate.Ticks);
                    }

                    
                    String tail = "";
                    int a;
                    String[] items;
                    Int32 n = 100000;

                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                   
                    byte[] longbuffer = new byte[n];
                   
                    fs.Seek(fs.Length * 3 / 4, 0);

                    do
                    {
                        a = fs.Read(longbuffer, 0, n);
                        items = System.Text.Encoding.UTF8.GetString(longbuffer).ToString().Split('\n');
                    }  while ((items[items.Length-2].Split(' '))[0] != targetDomain);

                    for (int i=items.Length-1;i>0; i--)
                    {
                        if ((items[i].Split(' '))[0] == targetDomain)
                        {
                            tail = items[i] + tail;
                            continue;
                        }
                        break;
                    }

                    String currentstring = tail;
                    a = fs.Read(longbuffer, 0, n);
                    items = System.Text.Encoding.UTF8.GetString(longbuffer).ToString().Split('\n');
                    do
                    {
                        String rawstring = System.Text.Encoding.UTF8.GetString(longbuffer).ToString();
                        rawstring = tail+ rawstring.Substring(0, rawstring.Length - items[items.Length - 1].Length);
                        tail = items[items.Length - 1];
                        String[] records = rawstring.Split('\n');
                        foreach (String rec in records)
                        {
                            if (AnalyzeWikiString(rec) > 0) goto endFile;
                        }
                        a = fs.Read(longbuffer, 0, n);
                        items = rawstring.Split('\n');

                    } while ((items[0].Split(' '))[0] == targetDomain);

                    //russian tails
                endFile:;

                }
            }


        }

        static public byte AnalyzeWikiString(String s)
        {
            String[] items = s.Split(' ');
            if (items[0] != "ru") return 1;
            if (Common.interestPages.Contains(items[1]))
            {
                interestPageslocal[items[1]][0] += Int64.Parse(items[2]);
                interestPageslocal[items[1]][1] += Int64.Parse(items[3]);
                return 0;
            }  

            totalChangestoday+= Int64.Parse(items[2]);
            totalBytesInChangedFilesToday+= Int64.Parse(items[3]);
            return 0;
        }

        static void NullLocalPages()
        {
            foreach (List<Int64> l in interestPageslocal.Values)
            {
                l[0] = l[1] = 0;
            }
        }
        static Dictionary<String, List<Int64>> PagesLocalLoad()
        {
            Dictionary<String, List<Int64>> buf = new Dictionary<string, List<long>>();

            foreach (String str in Common.interestPages)
            {
                List<Int64> l = new List<long>();
                l.Add(0);
                l.Add(0);

                buf.Add(str, l);
            }

            return buf;
        }

    }
}
