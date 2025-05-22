using LibraryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryManagementSystem.MongoDBService
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        public BookService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _books = database.GetCollection<Book>("books");
        }
        public async Task<List<Book>> GetAllBooks() =>
        await _books.Find(_ => true).ToListAsync();

        public async Task<Book> GetBookById(string id) =>
            await _books.Find(b => b.ID == id).FirstOrDefaultAsync();

        public async Task<Book> CreateBook(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task UpdateBook(string id, Book book) =>
            await _books.ReplaceOneAsync(b => b.ID == id, book);

        public async Task DeleteBook(string id) =>
            await _books.DeleteOneAsync(b => b.ID == id);
    }
}
