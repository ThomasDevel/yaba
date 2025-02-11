using System;
using System.Data.SQLite;

namespace Yaba
{
    /// <summary>
    /// SQL decisions:
    /// - Single Table Inheritance (aka Table Per Hierarchy Inheritance)
    /// - Concrete Table Inheritance
    /// - Class Table Inheritance (aka Table Per Type Inheritance) <--
    /// 
    /// The names of these three models come from Martin Fowler's book Patterns of Enterprise Application Architecture.
    /// 
    /// Let's take the Class Table Inheritance
    /// - Base Spirits table with Id, ImageBlob, Alcohol Percentage, Year Etc.. all shared information
    /// - Dedicated Whisky, Rum, Cognac, Armagnac, Brandy.
    ///   Mandatory attributes can be enforced with NOT NULL
    ///   No need for the type attribute.
    ///   Join SPIRIT on ID
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            SQLiteConnection sqlite;
            sqlite = CreateConnection();
            CreateTable(sqlite);
            InsertData(sqlite);
            ReadData(sqlite);
        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite;
            // Create a new database connection:
            sqlite = new SQLiteConnection();
            // Open the connection:
            try
            {
                sqlite.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite;
        }

        static void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE SampleTable (Col1 VARCHAR(20), Col2 INT)";
            string Createsql1 = "CREATE TABLE SampleTable1 (Col1 VARCHAR(20), Col2 INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = Createsql1;
            sqlite_cmd.ExecuteNonQuery();

        }

        static void InsertData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1 ', 2); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();

        }

        static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            conn.Close();
        }
    }
}