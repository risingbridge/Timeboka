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

    public async Task<string> FindIncomplete()
    {
        string incompleteId = string.Empty;
        //TODO: User Lookup
        var result = await _entries.FindAsync(e => e.UserId == "user1" && e.Complete == false);
        if (result.ToList().Count > 1)
        {
            incompleteId = result.ToList().Last().Id;
        }
        else
        {
            incompleteId = result.ToList()[0].Id;
        }
        return incompleteId;
    }

    public async Task<HourEntryModel?> GetLastUserEntry()
    {
        //TODO: User Lookup
        var result = await _entries.FindAsync(e => e.UserId == "user1");
        List<HourEntryModel> entries = await result.ToListAsync();
        if(entries.Count < 1)
        {
            return null;
        }
        else
        {
            return entries.Last();
        }
    }

    public async Task StartNewEntry(string userId)
    {
        DateTime startTime = DateTime.UtcNow;
        DateTime endTime = DateTime.UtcNow;
        
        HourEntryModel entry = new HourEntryModel();
        entry.UserId = userId;
        entry.StartTime = startTime;
        entry.EndTime = endTime;
        entry.Span = endTime - startTime;
        entry.Complete = false;
        entry.Comment = string.Empty;

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
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task StopEntry(string entryId)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var entriesInTransaction = db.GetCollection<HourEntryModel>(_db.HourEntryCollectionName);
            var entry = (await entriesInTransaction.FindAsync(e => e.Id == entryId)).First();
            entry.EndTime = DateTime.UtcNow;
            entry.Span = entry.EndTime - entry.StartTime;
            entry.Complete = true;
            await entriesInTransaction.ReplaceOneAsync(session, e => e.Id == entryId, entry);
            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}