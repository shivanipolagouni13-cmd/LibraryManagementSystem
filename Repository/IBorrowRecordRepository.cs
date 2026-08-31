using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repository
{
    public interface IBorrowRecordRepository
    {
        Task<BorrowRecord> BorrowBookRecord(string bookId, string userId);
        Task ReturnBorrowRecord(string recordId);
        Task<List<BorrowRecord>> GetRecordsByUser(string userId);
        Task<List<BorrowRecord>> GetRecordsByBooks(string bookId);
        Task<List<BorrowRecord>> GetOverDueRecords();
        Task<BorrowRecord> GetRecordsById(string recordId);
    }
}
