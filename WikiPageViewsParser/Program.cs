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
            Tasks.DoExtraction(new DateTime(2013, 4, 1), new DateTime(2015, 12, 1));
        }
    }
}
