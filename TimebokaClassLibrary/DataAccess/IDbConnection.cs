using MongoDB.Driver;

namespace TimebokaClassLibrary.DataAccess;
public interface IDbConnection
{
    MongoClient Client { get; set; }
    string DbName { get; set; }
    
    string HourEntryCollectionName { get; }
    IMongoCollection<HourEntryModel> HourEntryCollection { get; }
}