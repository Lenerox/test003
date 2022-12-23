//Wywala się na nazwach ze spacją...
using System.Data.SqlClient;
using System.Data;
using System.Timers;
using System.Threading.Tasks;

namespace ConnectingToSQLServer
{
    class Program
    {
       
       
        static void Main(string[] args)
        {
            string connString = "Server= localhost; Database= master; Integrated Security=SSPI;"; //Database 
            string queryString = "SELECT OrderID, CustomerID, OrderDate FROM dbo.Orders;";//kolumny z bazy danych
            string querStr00;
            string querStr01;

            string[] nazwy0 = new string[255];
            int num0 = 0;

            try
            {
                //ConTDB(connString);//test połączenia - zbędne
                SqlConnection connection = new SqlConnection(connString);
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                DataTable metaDataTable = connection.GetSchema("MetaDataCollections");
                //ShowDataTable(metaDataTable, 25);
                //ReadOrderData(connString, command);
                //ListOfDatabase(connString, connection);
                Console.WriteLine("?");
                DataTable allColumnsSchemaTable = connection.GetSchema("Columns");

                Console.WriteLine("Schema Information of All Columns:");
                nazwy0 = Test_tab(allColumnsSchemaTable);
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Select RED number to display data");
                Console.ForegroundColor = ConsoleColor.White;
                num0 = OdczytNum();
                querStr00 = Disp_tab(allColumnsSchemaTable, nazwy0[num0]);
                querStr01 = "SELECT " + querStr00 + " FROM dbo.[" + nazwy0[num0] + "]; ";

                Console.WriteLine(".............................................................");
                SqlCommand command0 = new SqlCommand(querStr01, connection);//Wywala się na nazwach ze spacją...
                ReadOrderData(connString, command0);//Wypisuje wszystkie dane!!
                Console.WriteLine();


                EndOfMain();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static void ReadOrderData(string connectionString, SqlCommand command0)
        {
            SqlDataReader reader = command0.ExecuteReader();

            // Call Read before accessing data.
            while (reader.Read())
            {
                ReadSingleRow((IDataRecord)reader);
            }
            // Call Close when done reading.
            reader.Close();
        }

        private static void ReadSingleRow(IDataRecord dataRecord)
        {
            //Console.Write("{0,-15}", dataRecord.FieldCount);
            for (int i = 0; i < dataRecord.FieldCount-1; i++)
            {
                Console.Write(String.Format("{0,-10}, ", dataRecord[i]));
            }
            Console.WriteLine("{0}", dataRecord[dataRecord.FieldCount - 1]);
        }

        private static void ListOfDatabase(string connectionString, SqlConnection conn0)
        {
            int ii=0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", conn0))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine(dr[0].ToString());
                            
                            ii++;
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine("ERROR");
                ii=1;
            }
        }


        static void ConTDB(string connStr)
        {
            SqlConnection conn = new SqlConnection(connStr);
            Console.WriteLine("Getting Connection ...");
            try
            {
                Console.WriteLine("Openning Connection ...");
                conn.Open();
                Console.WriteLine("Connection successful!");
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine(".: {0} :.", 3-i);
                }
                Console.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private static void ShowDataTable(DataTable table, Int32 length)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write("{0,-" + length + "}", col.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.DataType.Equals(typeof(DateTime)))
                        Console.Write("{0,-" + length + ":d}", row[col]);
                    else if (col.DataType.Equals(typeof(Decimal)))
                        Console.Write("{0,-" + length + ":C}", row[col]);
                    else
                        Console.Write("{0,-" + length + "}", row[col]);
                }
                Console.WriteLine();
            }
        }

        private static void ShowDataTable(DataTable table)
        {
            ShowDataTable(table, 14);
        }

        private static void ShowColumns(DataTable columnsTable)
        {
            var selectedRows = from info in columnsTable.AsEnumerable()
                               select new
                               {
                                   TableCatalog = info["TABLE_CATALOG"],
                                   TableSchema = info["TABLE_SCHEMA"],
                                   TableName = info["TABLE_NAME"],
                                   ColumnName = info["COLUMN_NAME"],
                                   DataType = info["DATA_TYPE"]
                               };

            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}", "TableCatalog", "TABLE_SCHEMA",
                "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE");
            foreach (var row in selectedRows)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}", row.TableCatalog,
                    row.TableSchema, row.TableName, row.ColumnName, row.DataType);
            }
        }

        private static string[] Test_tab(DataTable columnsTable)
        {
            var selectedRows = from info in columnsTable.AsEnumerable()
                               select new
                               {
                                   TableCatalog = info["TABLE_CATALOG"],
                                   TableSchema = info["TABLE_SCHEMA"],
                                   TableName = info["TABLE_NAME"],
                                   ColumnName = info["COLUMN_NAME"],
                                   DataType = info["DATA_TYPE"]
                               };

            Console.WriteLine("{2,-15}", "TableCatalog", "TABLE_SCHEMA", "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE");
            int i = 0;
            string[] t_name = new string[2];
            t_name[0] = "0";
            string t_name0 = "0";
            foreach (var row in selectedRows)
            {

                //Console.WriteLine("{2,-15}", row.TableCatalog, row.TableSchema, row.TableName, row.ColumnName, row.DataType);
                t_name0 = row.TableName.ToString()+"";
                if (i == 0)
                {
                    t_name[i] = t_name0;
                    i++;
                }
                else
                {
                    if (t_name0 != t_name[i - 1])
                    {
                        Array.Resize(ref t_name, t_name.Length + 1);
                        t_name[i] = t_name0;
                        i++;
                    }
                }
            }
            for (int j=0; j < i; j++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0,-5}", j);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", t_name[j]);
            }
            return t_name;
        }

        private static string Disp_tab(DataTable columnsTable, string datbas0)
        {
            string querStr0 = "";
            int i = 0;
            string[] t_name = new string[2];

            var selectedRows = from info in columnsTable.AsEnumerable()
                               select new
                               {
                                   TableCatalog = info["TABLE_CATALOG"],
                                   TableSchema = info["TABLE_SCHEMA"],
                                   TableName = info["TABLE_NAME"],
                                   ColumnName = info["COLUMN_NAME"],
                                   DataType = info["DATA_TYPE"]
                               };

            Console.WriteLine("{3,-15}", "TableCatalog", "TABLE_SCHEMA", "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE");
            
            t_name[0] = "0";
            foreach (var row in selectedRows)
            {
                if (row.TableName.ToString()==datbas0)
                {
                    Array.Resize(ref t_name, t_name.Length + 1);
                    t_name[i] = row.ColumnName.ToString()+"";
                    i++;
                }
                            }
            for (int j = 0; j < i; j++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0,-5}", j);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", t_name[j]);
                if (i-1 == j)
                {
                    querStr0 = querStr0 + t_name[j];
                }
                else
                {
                    querStr0 = querStr0 + t_name[j] + ", ";
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}",querStr0);
            Console.ForegroundColor = ConsoleColor.White;

            return querStr0;
        }

        private static int OdczytNum()
        {
            int lp = 0;
            try
            {
                lp = Int16.Parse(Console.ReadLine()+"");
            }
            catch
            {
                Console.WriteLine("Error!");
                lp = OdczytNum();
            }
            return lp;
        }

        static void EndOfMain()
        {
            Console.Read();
            Console.Clear();
        }
    }
}
