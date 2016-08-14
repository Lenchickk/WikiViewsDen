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
            Tasks.DoExtraction(new DateTime(2013, 8, 5), new DateTime(2014, 5, 31));
        }
    }
}
