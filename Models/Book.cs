using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }
        [BsonElement("title")]
        public string Title {  get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("isbn")]
        public string ISBN {  get; set; }

        [BsonElement("quantity")]
        public int Quantity {  get; set; }
        [BsonElement("availabile")]
        public int Available {  get; set; }

    }
}
