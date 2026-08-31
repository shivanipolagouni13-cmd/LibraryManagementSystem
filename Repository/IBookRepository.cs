using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(string id);
        Task<Book> CreateAsync(Book book);
        Task UpdateAsync(string id, Book book);
        Task<bool> DeleteAsync(string id);
        Task<List<Book>> SearchByTitleAsync(string title);
        Task<List<Book>> GetAvailableBooksAsync();
    }
}
