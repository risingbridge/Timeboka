using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimebokaClassLibrary.DataAccess;
public class MongoHourEntry : IHourEntry
{
    private readonly IDbConnection _db;
    private IMongoCollection<HourEntryModel> _entries;

    public MongoHourEntry(IDbConnection db)
    {
        _db = db;
        _entries = db.HourEntryCollection;
    }

    public async Task<List<HourEntryModel>> GetAllHourEntries()
    {
        return await _entries.Find(e => true).ToListAsync();
    }

    public async Task<List<HourEntryModel>> GetUserEntries()
    {
        //TODO: Add actual user lookup
        var result = _entries.FindAsync(e => e.UserId == "user1");
        return await result.Result.ToListAsync();
    }

    public async Task CreateEntry(HourEntryModel entry)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_db.DbName);
            var entriesInTransaction = db.GetCollection<HourEntryModel>(_db.HourEntryCollectionName);
            await entriesInTransaction.InsertOneAsync(session, entry);
            await session.CommitTransactionAsync();
        }
        catch(Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task<bool> CheckIfIncomplete()
    {
        bool incomplete = false;
        //TODO: User Lookup
        var result = await _entries.FindAsync(e => e.UserId == "user1" && e.Complete == false);
        if (result.ToList().Count > 0)
        {
            incomplete = true;
        }
        return incomplete;
    }
}
