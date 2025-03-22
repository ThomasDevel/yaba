using System;
using System.Data.SQLite;
using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public class WhiskyRepository : IWhiskyRepository
    {
        private readonly SQLiteConnection _connection;


        public WhiskyRepository(SQLiteConnection sqliteConnection)
        {
            _connection = sqliteConnection;
        }

        public bool CreateEntry(Whisky whisky, SQLiteTransaction transaction)
        {
            try
            {
                var command = new SQLiteCommand("INSERT INTO cars(name, price) VALUES(@name, @price)", _connection, transaction);
                //command = _connection.CreateCommand();
                //command.CommandText = "INSERT INTO Whiskies (Col1, Col2) VALUES('Test Text ', 1); ";
                //command.CommandText = "INSERT INTO cars(name, price) VALUES(@name, @price)";

                //command.Parameters.AddWithValue("@name", "BMW");
                //command.Parameters.AddWithValue("@price", 36600);
                //command.Prepare();
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Whisky FindEntryById(string id)
        {
            SQLiteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM beverages OUTER JOIN whiskies ON whiskies.Id = beverages.Id;";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string myreader = reader.GetString(0);
            }

            return null;
        }

        public bool DeleteEntryById(string id, SQLiteTransaction transaction)
        {
            throw new NotImplementedException();
        }



        public bool UpdateEntryById(Whisky whisky, SQLiteTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
