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
            Tasks.DoExtraction(new DateTime(2014, 12, 25   ), new DateTime(2014, 12, 25));
        }
    }
}
