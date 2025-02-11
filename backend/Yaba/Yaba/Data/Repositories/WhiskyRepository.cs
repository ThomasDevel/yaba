using System.Data.SQLite;
using Yaba.Domain.Models;

namespace Yaba.Data.Repositories
{
    public class WhiskyRepository : IWhiskyRepository
    {
        private readonly SQLiteConnection _connection;

        public WhiskyRepository(SQLiteConnection sqliteConnection)
        {
            _connection = sqliteConnection;
        }

        public bool CreateWhiskyEntry(Whisky whisky)
        {
            SQLiteCommand command;
            command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Whiskies (Col1, Col2) VALUES('Test Text ', 1); ";
            var result = command.ExecuteNonQuery();

            return result > 0;
        }

        public bool DeleteWhiskyEntryById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Whisky FindWhiskyEntryById(string id)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateWhiskyEntryById(Whisky whisky, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
