using System.Data.SQLite;
using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public interface IWhiskyRepository
    {
        bool CreateEntry(Whisky whisky, SQLiteTransaction transaction);

        Whisky FindEntryById(string id);

        bool UpdateEntryById(Whisky whisky, SQLiteTransaction transaction);

        bool DeleteEntryById(string id, SQLiteTransaction transaction);
    }
}
