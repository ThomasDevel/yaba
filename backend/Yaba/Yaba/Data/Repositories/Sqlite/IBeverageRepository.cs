using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public interface IBeverageRepository
    {
        ///CRUD
        bool CreateEntry(Beverage beverage);

        Whisky FindEntryById(string id);

        bool UpdateEntryById(string id, Beverage beverage);

        bool DeleteEntryById(string id);

        ///List
        Beverage[] GetBeveragesPaginated(int limit, int offset);
    }
}
