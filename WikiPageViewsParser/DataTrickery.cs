using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WikiDigger;

using System.IO;

namespace WikiPageViewsParser
{
    public static class DataTrickery
    {
        public static HashSet<String> DataTableToHashSet(DataTable dt)
        {
            HashSet<String> hs = new HashSet<string>();

            foreach (DataRow dr in dt.Rows)
            {
                hs.Add(WikiDigger.QuotedPrintable.EncodeQuotedPrintable(dr[0].ToString().Trim()));
            }
            return hs;

        }

        public static new HashSet<string> GetInterestPagesFromFile(String file)
        {
            HashSet<string> names = new HashSet<string>();
            StreamReader sr = new StreamReader(file, Encoding.UTF8);

            String str = "";

            while ((str=sr.ReadLine())!=null)
            {
                names.Add(WikiDigger.QuotedPrintable.EncodeQuotedPrintable(str));
            }

            return names;
        }

        public static new Dictionary<string,Int32> GetInterestPagesFromFileToDictionary(String file)
        {
            Dictionary<string, Int32> names = new Dictionary<string, int>();
            StreamWriter sw = new StreamWriter(@"C:\Users\elena\Source\Repos\WikiViewsDen\WikiPageViewsParser\bin\Debug\dictionaryPageViews.txt", false, Encoding.UTF8);
            StreamReader sr = new StreamReader(file, Encoding.UTF8);
            int code = 0;
            String str = "";

            while ((str = sr.ReadLine()) != null)
            {
                if (names.ContainsKey(str)) continue;
                sw.WriteLine(code.ToString() + "," + str);
                names.Add(WikiDigger.QuotedPrintable.EncodeQuotedPrintable(str),code);
                code++;
            }

            sr.Close();
            sw.Close();

            return names;
        }


    }
}
