using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        [BsonId]
        [BsonElement("_userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? userId { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        [Required]
        public string name { get; set; } = null!;

        [BsonElement("email")]
        [JsonPropertyName("email")]
        [Required]
        public string email { get; set; } = null!;

        [BsonElement("role")]
        [JsonPropertyName("role")]
        [Required]
        public string role { get; set; } = "Member"; // Default role is Memeber

        [BsonElement("membersince")]
        [JsonPropertyName("membersince")]
        [Required]
        public DateTime membersince { get; set; } = DateTime.UtcNow;

    }
}
