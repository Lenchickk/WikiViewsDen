using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace WikiPageViewsParser
{
    public static class PostGrePlugIn
    {
        public static String connstring = String.Format("Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=wikiproject;");
        public static String getKeysQuery = "select kid,title from public.keys;";
        public static String getCategoriesQuery = "select _key, title from categories;";
        public static IDataReader reader;


        //Index and name reverted. A table with 2 rows into a dictionary

        public static String ReturnIpQuery(String table, String ip, String fields)
        {
            return "select " + fields + " from " + table + " where '" + ip + "'::inet >= ip_start and '" + ip + "'::inet <= ip_end;";
        }

        public static HashSet<String> DataTableToHashSet(DataTable dt)
        {
            HashSet<String> hs = new HashSet<string>();

            foreach (DataRow dr in dt.Rows)
            {
                hs.Add(WikiDigger.QuotedPrintable.EncodeQuotedPrintable(dr[0].ToString().Trim()));
            }
            return hs;

        }
        public static List<String> DataTableToList(DataTable dt)
        {
            List<String> hs = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                hs.Add(dr[0].ToString().Trim());
            }
            if (dt.Rows.Count == 0) hs.Add("NA");
            return hs;

        }

        public static void GetTrollResults(DataTable dt, out byte troll, out double distance, out Int32 type)
        {
            type = 0;
            distance = -1;
            if (dt.Rows.Count == 0)
            {
                troll = 0;
                distance = -1;
                type = 0;
                return;
            }
            troll = 1;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < 4; i++) if (row[i].ToString() != "0") type = i + 1;
                distance = Double.Parse(row[4].ToString());
            }
            return;
        }

        static SortedDictionary<String, Int64> TableToDictionaryWithIndex(DataTable dt, string name, string indexName)
        {
            SortedDictionary<String, Int64> dict = new SortedDictionary<string, long>();

            foreach (DataRow dr in dt.Rows)
            {
                dict.Add(dr[name].ToString(), Int64.Parse(dr[indexName].ToString()));
            }

            return dict;
        }

        static SortedDictionary<String, String> TableToDictionaryWithIndexStringChild(DataTable dt, string name, string indexName)
        {
            SortedDictionary<String, String> dict = new SortedDictionary<string, String>();

            foreach (DataRow dr in dt.Rows)
            {
                if (dict.ContainsKey(dr[name].ToString())) continue;
                dict.Add(dr[name].ToString(), dr[indexName].ToString());
            }

            return dict;
        }

        static SortedDictionary<String, List<String>> TableToDictionaryWithIndex(DataTable dt)
        {
            SortedDictionary<String, List<String>> dict = new SortedDictionary<string, List<String>>();

            foreach (DataRow dr in dt.Rows)
            {
                List<String> buf = new List<String>();
                for (int i = dr.ItemArray.Length - 1; i > 0; i--)
                    buf.Add(dr[i].ToString());

                dict.Add(dr[0].ToString(), buf);
            }

            return dict;
        }


        static public SortedDictionary<String, Int64> getTableKeysAsDictionary()
        {
            return TableToDictionaryWithIndex(getTableKeys(), "title", "kid");
        }

        static public DataTable getTableKeys()
        {
            return getTablePostGre(getKeysQuery);
        }

        static public SortedDictionary<String, String> getTableCategoriesAsDictionary()
        {
            return TableToDictionaryWithIndexStringChild(getTableCategories(), "title", "_key");
        }

        static public DataTable getTableCategories()
        {
            return getTablePostGre(getCategoriesQuery);
        }

        static public NpgsqlConnection conn = new NpgsqlConnection(connstring);
        static public void openConnection()
        {

            conn.Open();
        }
        static public DataTable getTablePostGre(String sqlQuery)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                //conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlQuery, conn);
                da.SelectCommand.CommandTimeout = 5000;

                ds.Reset();
                // filling DataSet with result from NpgsqlDataAdapter
                da.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        static public void RunSQLQuery(String sqlQuery)
        {
            try
            {
                // PostgeSQL-style connection string
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                // quite complex sql statement
                // Making connection with Npgsql provider
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn);

                reader = command.ExecuteReader();
                reader.Close();


                conn.Close();


            }

            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
               // Console.WriteLine("something is wrong!");
                //throw;
            }
        }
    }
}
