using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WikiDigger;

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
    }
}
