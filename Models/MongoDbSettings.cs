namespace LibraryManagementSystem.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BooksCollectionName { get; set; }="Books-123"; // Default collection name
        public string UsersCollectionName { get; set; } = "Users"; // Default collection name for users
        public string BorrowRecordsCollectionName { get; set; } = "BorrowRecords"; // Default collection name for borrow records
    }
}
