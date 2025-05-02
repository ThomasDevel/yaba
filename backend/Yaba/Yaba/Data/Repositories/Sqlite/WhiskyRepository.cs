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
                command.Parameters.AddWithValue("@age", data.Age);
                command.Parameters.AddWithValue("@bottled", data.Bottled);
                command.Parameters.AddWithValue("@bottlingSeries", data.BottlingSeries);
                command.Parameters.AddWithValue("@caskType", data.CaskType);
                command.Parameters.AddWithValue("@category", data.Category);
                command.Parameters.AddWithValue("@created", data.Created);
                command.Parameters.AddWithValue("@nonChillFiltered", data.NonChillFiltered);
                command.Parameters.AddWithValue("@distillery", data.Distillery);
                command.Parameters.AddWithValue("@id", data.Id);
                command.Parameters.AddWithValue("@name", data.Name);
                command.Parameters.AddWithValue("@naturalColor", data.NaturalColor);
                command.Parameters.AddWithValue("@size", data.Size);
                command.Parameters.AddWithValue("@strength", data.Strength);
                command.Parameters.AddWithValue("@type", data.Type);

                command.Prepare();

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Whisky FindEntryById(string id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM whisky WHERE id = @id;";
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var entity = new WhiskyEntity()
                {
                    Id = reader.GetString(0),
                    Name = reader.GetString(1),
                    Strength = reader.GetFloat(2),
                    Size = reader.GetInt32(3),
                    Created = reader.GetDateTime(4),
                    Category = reader.GetString(5),
                    Distillery = reader.GetString(6),
                    Bottled = reader.GetInt32(7),
                    Age = reader.GetInt32(8),
                    CaskType = reader.GetString(9),
                    BottlingSeries = reader.GetString(10),
                    NaturalColor = reader.GetBoolean(11),
                    NonChillFiltered = reader.GetBoolean(12)
                };

                return entity.ToDomain();
            }

            return null;
        }

        public bool UpdateEntryById(string id, Whisky whisky)
        {
            try
            {
                var command = new SQLiteCommand(
                    "INSERT INTO whisky(id, type, name, strength, size, created, category, distillery, bottled, age, caskType, bottlingSeries, naturalColor, nonChillFiltered) " +
                    "VALUES(@id, @type, @name, @strength, @size, @created, @category, @distillery, @bottled, @age, @caskType, @bottlingSeries, @naturalColor, @nonChillFiltered) " +
                    "ON CONFLICT(id) DO UPDATE SET " +
                    "age=@age, bottled=@bottled, bottlingSeries=@bottlingSeries, caskType=@caskType, category=@category, created=@created, distillery=@distillery, name=@name, naturalColor=@naturalColor, " +
                    "nonChillFiltered=@nonChillFiltered,size=@size,strength=@strength,type=@type", _connection);

                var data = whisky.ToEntity();
                command.Parameters.AddWithValue("@age", data.Age);
                command.Parameters.AddWithValue("@bottled", data.Bottled);
                command.Parameters.AddWithValue("@bottlingSeries", data.BottlingSeries);
                command.Parameters.AddWithValue("@caskType", data.CaskType);
                command.Parameters.AddWithValue("@category", data.Category);
                command.Parameters.AddWithValue("@created", data.Created);
                command.Parameters.AddWithValue("@distillery", data.Distillery);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", data.Name);
                command.Parameters.AddWithValue("@naturalColor", data.NaturalColor);
                command.Parameters.AddWithValue("@nonChillFiltered", data.NonChillFiltered);
                command.Parameters.AddWithValue("@size", data.Size);
                command.Parameters.AddWithValue("@strength", data.Strength);
                command.Parameters.AddWithValue("@type", data.Type);

                command.Prepare();

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteEntryById(string id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM whisky WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);

            // Execute the DELETE statement
            return command.ExecuteNonQuery() > 0;
        }
    }
}
