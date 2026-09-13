using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Yaba.Domain.Models;
using Yaba.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public class WhiskyRepository : IWhiskyRepository
    {
        private const string SelectColumns =
            "id, type, name, strength, size, created, category, distillery, bottled, age, caskType, bottlingSeries, naturalColor, nonChillFiltered";

        private readonly SQLiteConnection _connection;

        public WhiskyRepository(SQLiteConnection sqliteConnection)
        {
            _connection = sqliteConnection;
        }

        public bool CreateEntry(Whisky whisky)
        {
            try
            {
                var command = new SQLiteCommand(
                    "INSERT INTO whisky(id, type, name, strength, size, created, category, distillery, bottled, age, caskType, bottlingSeries, naturalColor, nonChillFiltered) " +
                    "VALUES(@id, @type, @name, @strength, @size, @created, @category, @distillery, @bottled, @age, @caskType, @bottlingSeries, @naturalColor, @nonChillFiltered)",
                    _connection);

                BindWhiskyParameters(command, whisky.ToEntity(), whisky.Id);
                command.Prepare();
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Whisky[] ListEntries()
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT {SelectColumns} FROM whisky ORDER BY name";

            return ReadAll(command);
        }

        public Whisky FindEntryById(string id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT {SelectColumns} FROM whisky WHERE id = @id;";
            command.Parameters.AddWithValue("@id", id);

            var results = ReadAll(command);
            return results.Length == 0 ? null : results[0];
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
                    "nonChillFiltered=@nonChillFiltered, size=@size, strength=@strength, type=@type",
                    _connection);

                BindWhiskyParameters(command, whisky.ToEntity(), id);
                command.Prepare();
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteEntryById(string id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM whisky WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }

        private static void BindWhiskyParameters(SQLiteCommand command, WhiskyEntity data, string id)
        {
            command.Parameters.AddWithValue("@age", data.Age);
            command.Parameters.AddWithValue("@bottled", data.Bottled);
            command.Parameters.AddWithValue("@bottlingSeries", data.BottlingSeries);
            command.Parameters.AddWithValue("@caskType", data.CaskType);
            command.Parameters.AddWithValue("@category", data.Category);
            command.Parameters.AddWithValue("@created", data.Created.ToString("o"));
            command.Parameters.AddWithValue("@nonChillFiltered", data.NonChillFiltered);
            command.Parameters.AddWithValue("@distillery", data.Distillery);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", data.Name);
            command.Parameters.AddWithValue("@naturalColor", data.NaturalColor);
            command.Parameters.AddWithValue("@size", data.Size);
            command.Parameters.AddWithValue("@strength", data.Strength);
            command.Parameters.AddWithValue("@type", data.Type);
        }

        private static Whisky[] ReadAll(SQLiteCommand command)
        {
            var items = new List<Whisky>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(ReadEntity(reader).ToDomain());
            }

            return items.ToArray();
        }

        private static WhiskyEntity ReadEntity(SQLiteDataReader reader)
        {
            return new WhiskyEntity
            {
                Id = reader.GetString(0),
                Name = reader.GetString(2),
                Strength = reader.GetFloat(3),
                Size = reader.GetInt32(4),
                Created = DateTimeOffset.Parse(reader.GetString(5)),
                Category = reader.GetString(6),
                Distillery = reader.GetString(7),
                Bottled = reader.GetInt32(8),
                Age = reader.GetInt32(9),
                CaskType = reader.GetString(10),
                BottlingSeries = reader.GetString(11),
                NaturalColor = ReadNullableBool(reader, 12),
                NonChillFiltered = ReadNullableBool(reader, 13)
            };
        }

        private static bool? ReadNullableBool(SQLiteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return Convert.ToInt32(reader.GetValue(ordinal)) != 0;
        }
    }
}
