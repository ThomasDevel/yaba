using System;
using System.Data.SQLite;

namespace Yaba.Seeding
{
    class Program
    {

        static void Main(string[] args)
        {

            using SQLiteConnection sqlite = new SQLiteConnection("Data Source=C:\\Dev\\Sqlite\\yaba.db;UseUTF16Encoding=False;");
            sqlite.Open();

            // Create Whiskies Table
            //string createTable = "CREATE TABLE Whiskies (Col1 VARCHAR(32), Col2 TEXT, Col3 INT, Col4 TEXT, Col5 INT, Col6 INT, Col7 TEXT, Col8 REAL, Col9 INT, Col10 INT, Col11 INT, Col12 TEXT)";
            //var createTableCommand = sqlite.CreateCommand();
            //createTableCommand.CommandText = createTable;
            //createTableCommand.ExecuteNonQuery();

            // Insert first meaningfull row
            var insertWhiskyCommand = sqlite.CreateCommand();
            insertWhiskyCommand.CommandText = $"INSERT INTO Whiskies (Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9, Col10, Col11, Col12) " +
                $"VALUES('{Guid.NewGuid().ToString("N")}', 'Arran 10', 1, 'Lochranza', 2022, 10, 'Ex-Bourbon and Ex-Sherry', 46.0, 70, 1, 1, '{DateTimeOffset.UtcNow}');";
            insertWhiskyCommand.ExecuteNonQuery();

            // Read created row
            var selectCommand = sqlite.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM Whiskies";
            var dataReader = selectCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string myreader = dataReader.GetString(0);
                Console.WriteLine(myreader);
            }
            sqlite.Close();
        }
    }
}