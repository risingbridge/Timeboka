namespace TimebokaClassLibrary.DataAccess;

public interface IHourEntry
{
    Task<List<HourEntryModel>> GetAllHourEntries();
    Task CreateEntry(HourEntryModel entry);

    Task<List<HourEntryModel>> GetUserEntries();
    Task<bool> CheckIfIncomplete();
    Task<string> FindIncomplete();
    Task<HourEntryModel> GetLastUserEntry();
    Task StartNewEntry(string userId);
    Task StopEntry(string entryId);
}