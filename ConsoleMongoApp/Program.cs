using MongoDB.Driver;
using MongoDB.Bson;
using ConsoleMongoApp;

MongoClient client = new MongoClient("mongodb://192.168.10.64:27017/");
List<string> databases = client.ListDatabaseNames().ToList();

IMongoCollection<HourEntry> hourCollection = client.GetDatabase("Timeboka").GetCollection<HourEntry>("entries");

HourEntry entry = new HourEntry(DateTime.UtcNow, DateTime.Now, true, "No comment");
Console.WriteLine(entry);
hourCollection.InsertOne(entry);