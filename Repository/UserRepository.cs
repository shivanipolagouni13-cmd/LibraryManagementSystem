using LibraryManagementSystem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryManagementSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _user;

        public UserRepository(IOptions<MongoDbSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _user = database.GetCollection<User>(mongoDBSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAllAsync() =>
            await _user.Find(x => true).ToListAsync();

        public async Task<User> GetByIdAsync(string id) =>
            await _user.Find(x => x.userId == id).FirstOrDefaultAsync();

        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await _user.Find(x => x.userId == user.userId).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new InvalidOperationException($"A User with {user.userId} already exists");
            }
            await _user.InsertOneAsync(user);
            return user;
        }
       
        public async Task<User> UpdateUserAsync(string id, User user)
        {
            var result = await _user.ReplaceOneAsync(x => x.userId == id, user);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return user;
            }
            throw new InvalidOperationException($"Failed to update user with ID {id}");
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var result = await _user.DeleteOneAsync(x => x.userId == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
