using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BattleshipBackend.Models;

public class GameRoom
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("blueTeam")]
    public List<ObjectId> BlueTeam { get; set; } = [];
    [BsonElement("readTeam")]
    public List<ObjectId> ReadTeam { get; set; } = [];
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}