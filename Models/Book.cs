using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        [BsonElement("Title")]
        [JsonPropertyName("title")]
        [Required]
        public string Title { get; set; } = "string";

        [BsonElement("Author")]
        [JsonPropertyName("author")]
        [Required]
        public string Author { get; set; } = "string";

        [BsonElement("Quantity")]
        [JsonPropertyName("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1; // Fixed CS0037 by assigning a valid default value  

        [BsonElement("Available")]
        [JsonPropertyName("Available")]
        public bool IsAvailable { get; set; } = true;

        [BsonElement("PublicationYear")]
        public int PublicationYear { get; set; }

        public string Genre { get; set; } = null!;
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        [BsonElement("ISBN")]
        [BsonIgnoreIfNull]
        [JsonIgnore]
        public string? ISBN { get; set; }
    }
}