using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BorrowRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? recordId { get; set; }

        [BsonElement("BookId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string bookId { get; set; } = null!;

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string userId { get; set; } = null!;

        [BsonElement("BorrowDate")]
        [Required]
        public DateTime borrowDate { get; set; } = DateTime.UtcNow;

        [BsonElement("DueDate")]
        [Required]
        public DateTime dueDate { get; set; } = DateTime.UtcNow.AddDays(14);

        [BsonElement("ReturnDate")]
        public DateTime? returnDate { get; set; }

        [BsonElement("IsReturned")]
        public bool isReturned { get; set; } = false;
    }
}