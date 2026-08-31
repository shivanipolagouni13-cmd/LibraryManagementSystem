using LibraryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetAsync(string id);
        Task<Book?> GetByName(string bookName);
        Task<Book> CreateAsync(Book book);
        Task UpdateAsync(string id, Book book);
        Task DeleteAsync(string id);
    }

    public class BookService : IBookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _books = database.GetCollection<Book>("Books-123");
        }

        public async Task<List<Book>> GetAllAsync() =>
            await _books.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) =>
            await _books.Find(x => x.ID == id).FirstOrDefaultAsync();

        public async Task<Book?> GetByName(string bookName)
        {
            var bookname = await _books.Find(x => x.Title.Contains(bookName)).ToListAsync();
            return bookname.FirstOrDefault();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            // Check if a book with the same ID already exists
            var existingBook = await _books.Find(x => x.ID == book.ID).FirstOrDefaultAsync();
            if (existingBook != null)
            {
                throw new InvalidOperationException($"A book with ID {book.ID} already exists.");
            }

            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task UpdateAsync(string id, Book book) =>
            await _books.ReplaceOneAsync(x => x.ID == id, book);

        public async Task DeleteAsync(string id) =>
            await _books.DeleteOneAsync(x => x.ID == id);
    }
}