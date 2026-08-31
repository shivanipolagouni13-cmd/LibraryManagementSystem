using MongoDB.Driver;
using LibraryManagementSystem.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
namespace LibraryManagementSystem.Repository
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly IMongoCollection<BorrowRecord> _records;
        private readonly IMongoCollection<Book> _books;
        public BorrowRecordRepository(IMongoClient client,IOptions<MongoDbSettings> mongoDBSettings)
        {
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _records = database.GetCollection<BorrowRecord>("BorrowRecords");
            _books = database.GetCollection<Book>("Books-123");
        }
        public async Task<BorrowRecord> BorrowBookRecord(string bookId, string userId)
        {
            var book = await _books.Find(b => b.ID == bookId).FirstOrDefaultAsync();
            if (book == null)
                throw new Exception($"Book with ID {bookId} not found in the database");
                
            if (!book.IsAvailable)
                throw new Exception($"Book with ID {bookId} is currently not available for borrowing. IsAvailable: {book.IsAvailable}");

            var record = new BorrowRecord
            {
                bookId = bookId,
                userId = userId,
                borrowDate = DateTime.UtcNow,
                dueDate = DateTime.UtcNow.AddDays(14),
                isReturned = false
            };

            await _records.InsertOneAsync(record);

            // Update book status
            var updateResult = await _books.UpdateOneAsync(
                b => b.ID == bookId,
                Builders<Book>.Update.Set(b => b.IsAvailable, false));

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
                throw new Exception($"Failed to update book availability for book ID {bookId}");

            return record;
        }
        public async Task ReturnBorrowRecord(string recordId)
        {
            var record = await _records.Find(x=>x.recordId == recordId).FirstOrDefaultAsync();
            if (record == null)
                throw new Exception($"Record Not Found!");
            


        }
        public async Task<List<BorrowRecord>> GetRecordsByUser(string userId)
        {
            var record = await _records.Find(x=>x.userId == userId).FirstOrDefaultAsync();
            if (record == null)
                throw new Exception("$User Not Found");
            return await _records.Find(x=> x.userId == userId).ToListAsync();
        }
        public async Task<List<BorrowRecord>> GetRecordsByBooks(string bookId)
        {
            if (string.IsNullOrEmpty(bookId))
                throw new ArgumentException("Book ID cannot be null or empty", nameof(bookId));

            return await _records.Find(x => x.bookId == bookId && !x.isReturned).ToListAsync();
        }
        public async Task<List<BorrowRecord>> GetOverDueRecords()
        {
            var overdueDate= DateTime.UtcNow;
            return await _records.Find(x => x.dueDate < overdueDate && !x.isReturned).ToListAsync();
        }
        public async Task<BorrowRecord> GetRecordsById(string recordId)
            {
            if (string.IsNullOrEmpty(recordId))
                throw new ArgumentException("Record ID cannot be null or empty", nameof(recordId));
            var record = await _records.Find(x => x.recordId == recordId).FirstOrDefaultAsync();
            if (record == null)
                throw new Exception($"Record with ID {recordId} not found.");
            return record;
        }
    }
}
