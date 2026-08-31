using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(string id,User user);
        Task<bool> DeleteUserAsync(string id);
    }
}
