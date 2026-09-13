using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public interface IWhiskyRepository
    {
        bool CreateEntry(Whisky whisky);

        Whisky[] ListEntries();

        Whisky FindEntryById(string id);

        bool UpdateEntryById(string id, Whisky whisky);

        bool DeleteEntryById(string id);
    }
}
