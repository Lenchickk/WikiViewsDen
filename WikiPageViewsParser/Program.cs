using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageViewsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tasks.DoExtraction(new DateTime(2014, 5, 31), new DateTime(2014, 6, 3));
            Tasks.DoExtraction(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31));
        }
    }
}
