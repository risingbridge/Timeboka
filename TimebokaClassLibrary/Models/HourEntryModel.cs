namespace TimebokaClassLibrary.Models;
public class HourEntryModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Span { get; set; }
    public bool Complete { get; set; }
    public string Comment { get; set; }
}
