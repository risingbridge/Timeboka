using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace TimebokaClassLibrary.DataAccess;
public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private string _connectionId = "MongoDB";

    public string HourEntryCollectionName { get; private set; } = "entries";
    public string UserCollectionName { get; private set; } = "users";
    public IMongoCollection<HourEntryModel> HourEntryCollection { get; set; }
    public IMongoCollection<UserModel> UserCollection { get; set; }

    public MongoClient Client { get; set; }

    public string DbName { get; set; }

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        HourEntryCollection = _db.GetCollection<HourEntryModel>(HourEntryCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
    }
}
