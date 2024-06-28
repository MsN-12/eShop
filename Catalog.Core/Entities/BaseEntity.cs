using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public int Id { get; set; }
}