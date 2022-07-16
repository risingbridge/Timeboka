using MongoDB.Bson;

namespace ConsoleMongoApp;
public class HourEntry
{
    public HourEntry(DateTime startTime, DateTime endTime, bool complete, string comment)
    {
        StartTime = startTime;
        EndTime = endTime;
        Span = endTime - startTime;
        Complete = complete;
        Comment = comment;
    }

    public override string ToString()
    {
        string returnString = $"Start: {StartTime}\nEnd: {EndTime}\nDuration: {Span}\nComplete: {Complete}";
        return returnString;
    }

    public ObjectId _id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Span { get; set; }
    public bool Complete { get; set; }
    public string Comment { get; set; }
}