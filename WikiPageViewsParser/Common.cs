﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WikiPageViewsParser
{
    public static class Common
    {
        public static String baseLink = "http://dumps.wikimedia.org/other/pagecounts-raw/";
        public static volatile List<List<String>> extractedData = new List<List<string>>();
        public static volatile HashSet<String> interestPages = new HashSet<string>();
        public static volatile Dictionary<string, Int32> interestPageAsDict = new Dictionary<string,Int32>();
        public static volatile List<String> links = new List<String>();
        public static volatile Dictionary<DateTime, byte> countperDay = new Dictionary<DateTime, byte>();
        public static String getPagesSQL_ukraine_sample = @"select distinct title from pages where _key<>'Славянск' and _key<>'Минск' and _key<>'Белорусси' and _key<>'Аваков' and _key<>'Аксенов' and nc=14;";
        public static String getPagesSQL = @"select title from allpagesMainNC;'";
        public static volatile List<Thread> downloaderThreads = new List<Thread>();
        public static volatile List<Thread> unwrapThreads = new List<Thread>();
        public static volatile String pile = @"C:\Users\elena\Source\Repos\WikiViewsDen\WikiPageViewsParser\bin\Debug\";
        public static volatile String outputFile = @"C:\Users\elena\Source\Repos\WikiViewsDen\WikiPageViewsParser\bin\Debug\pageViewsUpdated.txt";
        public static volatile String outputFile_details = @"C:\Users\elena\Source\Repos\WikiViewsDen\WikiPageViewsParser\bin\Debug\detailedpageViewsALlMainDomain.txt";
        public static String ListofPagesFile = @"C:\work\bdata\ruwiki-20160920-all-titles-in-ns0\ruwiki-20160920-all-titles-in-ns0.csv";
        static public Boolean pendingCondition()
        {
            return (Downloader.compressedFiles + DecompressionTrickery.decompressedFiles + Downloader.rawFiles > 9 );
        }
    }
}
