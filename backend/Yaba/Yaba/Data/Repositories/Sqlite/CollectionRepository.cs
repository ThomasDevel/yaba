using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly SQLiteConnection _connection;

        public CollectionRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public bool AddItem(CollectionItem item)
        {
            try
            {
                var command = new SQLiteCommand(
                    "INSERT INTO collection_items(user_id, spirit_type, spirit_id, added) " +
                    "VALUES(@userId, @spiritType, @spiritId, @added)", _connection);

                command.Parameters.AddWithValue("@userId", item.UserId);
                command.Parameters.AddWithValue("@spiritType", item.SpiritType.ToString());
                command.Parameters.AddWithValue("@spiritId", item.SpiritId);
                command.Parameters.AddWithValue("@added", item.Added.ToString("o"));

                command.Prepare();

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveItem(string userId, BeverageType spiritType, string spiritId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM collection_items WHERE user_id = @userId AND spirit_type = @spiritType AND spirit_id = @spiritId";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@spiritType", spiritType.ToString());
            command.Parameters.AddWithValue("@spiritId", spiritId);

            return command.ExecuteNonQuery() > 0;
        }

        public CollectionItem[] GetCollection(string userId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT user_id, spirit_type, spirit_id, added FROM collection_items WHERE user_id = @userId ORDER BY added DESC";
            command.Parameters.AddWithValue("@userId", userId);

            return ReadItems(command);
        }

        public CollectionItem[] GetCollectionByType(string userId, BeverageType spiritType)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT user_id, spirit_type, spirit_id, added FROM collection_items WHERE user_id = @userId AND spirit_type = @spiritType ORDER BY added DESC";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@spiritType", spiritType.ToString());

            return ReadItems(command);
        }

        public bool HasItem(string userId, BeverageType spiritType, string spiritId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM collection_items WHERE user_id = @userId AND spirit_type = @spiritType AND spirit_id = @spiritId";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@spiritType", spiritType.ToString());
            command.Parameters.AddWithValue("@spiritId", spiritId);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        private CollectionItem[] ReadItems(SQLiteCommand command)
        {
            var items = new List<CollectionItem>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new CollectionItem
                {
                    UserId = reader.GetString(0),
                    SpiritType = Enum.Parse<BeverageType>(reader.GetString(1), true),
                    SpiritId = reader.GetString(2),
                    Added = DateTimeOffset.Parse(reader.GetString(3))
                });
            }

            return items.ToArray();
        }
    }
}
