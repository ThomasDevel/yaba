using System;
using System.Data.SQLite;
using Yaba.Data.Entities;
using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public class BeverageRepository : IBeverageRepository
    {
        private readonly SQLiteConnection _connection;
        private readonly IWhiskyRepository _whiskies;

        public BeverageRepository(SQLiteConnection connection, IWhiskyRepository whiskies)
        {
            _connection = connection;
            _whiskies = whiskies;
        }

        /// <summary>
        /// Creating an entry in the beverage table along its subclass table should alawys happen in a transaction
        /// to sync these.
        /// </summary>
        /// <param name="beverage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CreateEntry(Beverage beverage)
        {
            using var transaction = _connection.BeginTransaction();
            try
            {
                var command = new SQLiteCommand("INSERT INTO beverages(id, class, name, strength, size, created) VALUES(@id, @class, @name, @strength, @size, @created)", _connection, transaction);

                var data = BeverageEntity.ToEntity(beverage);
                command.Parameters.AddWithValue("@id", data.Id);
                command.Parameters.AddWithValue("@class", data.Class);
                command.Parameters.AddWithValue("@name", data.Name);
                command.Parameters.AddWithValue("@strength", data.Strength);
                command.Parameters.AddWithValue("@size", data.Size);
                command.Parameters.AddWithValue("@created", data.Created);
                command.Prepare();

                command.ExecuteNonQuery();

                _ = beverage switch
                {
                    Whisky whisky => _whiskies.CreateEntry(whisky, transaction),
                    _ => throw new NotImplementedException($"Type {beverage.GetType()} is not yet supported.")
                };
            }
            catch (SQLiteException ex)
            {
                // Roll back the transaction
                transaction.Rollback();
                Console.WriteLine(ex.ToString());
                return false;
            }

            transaction.Commit();
            return true;
        }

        public bool DeleteEntryById(string id)
        {
            throw new NotImplementedException();
        }

        public Whisky FindEntryById(string id)
        {
            throw new NotImplementedException();
        }

        public Beverage[] GetBeveragesPaginated(int limit, int offset)
        {
            // Command -> SELECT * FROM table_name LIMIT number_of_rows OFFSET starting_row;
            throw new NotImplementedException();
        }

        public bool UpdateEntryById(string id, Beverage beverage)
        {
            throw new NotImplementedException();
        }
    }
}
