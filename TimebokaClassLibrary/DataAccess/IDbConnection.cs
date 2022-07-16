using MongoDB.Driver;

namespace TimebokaClassLibrary.DataAccess;
public interface IDbConnection
{
    MongoClient Client { get; set; }
    string DbName { get; set; }
    
    string HourEntryCollectionName { get; }
    string UserCollectionName { get; }
    IMongoCollection<HourEntryModel> HourEntryCollection { get; }
    IMongoCollection<UserModel> UserCollection { get; }
}