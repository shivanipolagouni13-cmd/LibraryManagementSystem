using LibraryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryManagementSystem.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _booksCollection;
        public BookRepository(IMongoClient client, IOptions<MongoDbSettings> mongoDBSettings)
        {
            var mongoDatabase = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _booksCollection = mongoDatabase.GetCollection<Book>(mongoDBSettings.Value.BooksCollectionName);
        }
        public async Task<List<Book>> GetAllAsync() =>
            await _booksCollection.Find(x => true).ToListAsync();
        public async Task<Book?> GetByIdAsync(string id) =>
            await _booksCollection.Find(x => x.ID == id).FirstOrDefaultAsync();
        public async Task<Book> CreateAsync(Book book)
        {
            var existingBook = await _booksCollection.Find(x => x.ID == book.ID).FirstOrDefaultAsync();
            if (existingBook != null)
            {
                throw new InvalidOperationException($"A book with ID{book.ID} already exists.");
            }
            await _booksCollection.InsertOneAsync(book);
            return book;
        }
        public async Task UpdateAsync(string id, Book book) =>
            await _booksCollection.ReplaceOneAsync(x => x.ID == id, book);
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _booksCollection.DeleteOneAsync(x => x.ID == id);
            return result.DeletedCount > 0;
        }
        public async Task<List<Book>> SearchByTitleAsync(string title) =>
            await _booksCollection.Find(x => x.Title.Contains(title)).ToListAsync();
        public async Task<List<Book>> GetAvailableBooksAsync() =>
            await _booksCollection.Find(x => x.IsAvailable).ToListAsync();
        public async Task<Book> GetBookWithAvailability(string bookId)
        {
            var book = await _booksCollection.Find(b => b.ID == bookId).FirstOrDefaultAsync();
            if (book == null)
                throw new Exception($"Book with ID {bookId} not found");
                
            return book;
        }
    }
}
