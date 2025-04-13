using System;
using System.Data.SQLite;
using Yaba.Domain.Models;
using Yaba.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public class WhiskyRepository : IWhiskyRepository
    {
        private readonly SQLiteConnection _connection;


        public WhiskyRepository(SQLiteConnection sqliteConnection)
        {
            _connection = sqliteConnection;
        }

        public WhiskyCategory Category { get; set; }

        public string Distillery { get; set; }

        public int Bottled { get; set; }

        public int Age { get; set; }

        public string CaskType { get; set; }

        public string BottlingSeries { get; set; }

        public bool? NaturalColor { get; set; }

        public bool? NonChillFiltered { get; set; }

        public bool CreateEntry(Whisky whisky)
        {
            try
            {
                var command = new SQLiteCommand("INSERT INTO whisky(id, type, name, strength, size, created, category, distillery, bottled, age, caskType, bottlingSeries, naturalColor, nonChillFiltered) " +
                    "VALUES(@id, @type, @name, @strength, @size, @created, @category, @distillery, @bottled, @age, @caskType, @bottlingSeries, @naturalColor, @nonChillFiltered)", _connection);

                var data = whisky.ToEntity();
                command.Parameters.AddWithValue("@id", data.Id);
                command.Parameters.AddWithValue("@type", data.Type);
                command.Parameters.AddWithValue("@name", data.Name);
                command.Parameters.AddWithValue("@strength", data.Strength);
                command.Parameters.AddWithValue("@size", data.Size);
                command.Parameters.AddWithValue("@created", data.Created);
                command.Prepare();

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
