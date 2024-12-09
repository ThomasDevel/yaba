namespace Yaba.Repositories
{
    public interface IWhiskyRepository
    {
        bool CreateWhiskyEntry(Whisky whisky);

        Whisky FindWhiskyEntryById(string id);

        bool UpdateWhiskyEntryById(Whisky whisky, string id);

        bool DeleteWhiskyEntryById(string id);
    }
}
