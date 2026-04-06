using Yaba.Domain.Models;

namespace Yaba.Data.Repositories.Sqlite
{
    public interface ICollectionRepository
    {
        bool AddItem(CollectionItem item);

        bool RemoveItem(string userId, BeverageType spiritType, string spiritId);

        CollectionItem[] GetCollection(string userId);

        CollectionItem[] GetCollectionByType(string userId, BeverageType spiritType);

        bool HasItem(string userId, BeverageType spiritType, string spiritId);
    }
}
