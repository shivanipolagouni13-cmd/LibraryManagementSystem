
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;

namespace LibraryManagementSystem.Services
{
    public interface IBorrowService
    {
        Task<BorrowRecord> BorrowBookAsync(string userId, string bookId);
        Task ReturnBookAsync(string recordId);
        Task<List<BorrowRecord>> GetUserBorrowHistoryAsync(string userId);
    }

    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRecordRepository _borrowRecordRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IUserRepository _userRepo;

        public BorrowService(
            IBorrowRecordRepository borrowRecordRepo,
            IBookRepository bookRepo,
            IUserRepository userRepo)
        {
            _borrowRecordRepo = borrowRecordRepo;
            _bookRepo = bookRepo;
            _userRepo = userRepo;
        }

        public async Task<BorrowRecord> BorrowBookAsync(string userId, string bookId)
        {
            // Check if book exists and is available
            var book = await _bookRepo.GetByIdAsync(bookId);
            if (book == null || !book.IsAvailable)
                throw new Exception("Book not available");

            // Check if user exists
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Create borrow record
            var record = new BorrowRecord
            {
                userId = userId,
                bookId = bookId,
                borrowDate = DateTime.UtcNow
            };

            await _borrowRecordRepo.BorrowBookRecord(bookId, userId);

            // Update book status
            book.IsAvailable = false;
            await _bookRepo.UpdateAsync(bookId, book);

            return record;
        }

        public async Task ReturnBookAsync(string recordId)
        {
            // Get the borrow record
            var record = await _borrowRecordRepo.GetRecordsById(recordId);
            if (record == null)
                throw new Exception("Borrow record not found");
            // Update book status
            var book = await _bookRepo.GetByIdAsync(record.bookId);
            if (book == null)
                throw new Exception("Book not found");
            book.IsAvailable = true;
            await _bookRepo.UpdateAsync(book.ID, book);
            // Mark the record as returned
            await _borrowRecordRepo.ReturnBorrowRecord(recordId);
        }

        public async Task<List<BorrowRecord>> GetUserBorrowHistoryAsync(string userId) =>
            await _borrowRecordRepo.GetOverDueRecords();
    }
}