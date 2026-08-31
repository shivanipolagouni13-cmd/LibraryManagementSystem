using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowRecordController : ControllerBase
    {
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IBorrowService _borrowService;

        public BorrowRecordController(
            IBorrowRecordRepository borrowRecordRepository,
            IBorrowService borrowService)
        {
            _borrowRecordRepository = borrowRecordRepository;
            _borrowService = borrowService;
        }

        [HttpPost("BorrowRecord")]
        public async Task<IActionResult> BorrowBookRecord(string bookId, string userId)
        {
            if (string.IsNullOrEmpty(bookId) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("BookId and UserId are required");
            }
            var result = await _borrowService.BorrowBookAsync(userId, bookId);
            return Ok(result);
        }

        [HttpPost("ReturnBorrowRecord")]
        public async Task<IActionResult> ReturnBorrowRecord(string recordId)
        {
            if (string.IsNullOrEmpty(recordId))
            {
                return BadRequest("Record ID is required");
            }
            await _borrowService.ReturnBookAsync(recordId);
            return Ok("Record returned successfully");
        }

        [HttpGet("GetRecordsByUser")]
        public async Task<IActionResult> GetRecordsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }
            var result = await _borrowService.GetUserBorrowHistoryAsync(userId);
            if (result == null || !result.Any())
            {
                return NotFound("No records found for the specified user");
            }
            return Ok(result);
        }

        [HttpGet("GetRecordsByBooks")]
        public async Task<IActionResult> GetRecordsByBooks(string bookId)
        {
            if (string.IsNullOrEmpty(bookId))
            {
                return BadRequest("Book ID is required");
            }
            var records = await _borrowRecordRepository.GetRecordsByBooks(bookId);
            if (records == null || !records.Any())
            {
                return NotFound("No records found for the specified book");
            }
            return Ok(records);
        }

        [HttpGet("GetOverDueRecords")]
        public async Task<IActionResult> GetOverDueRecords()
        {
            var result = await _borrowRecordRepository.GetOverDueRecords();
            if (result == null || !result.Any())
            {
                return NotFound("No overdue records found");
            }
            return Ok(result);
        }

        [HttpGet("GetRecordsById")]
        public async Task<IActionResult> GetRecordsById(string recordId)
        {
            if (string.IsNullOrEmpty(recordId))
            {
                return BadRequest("Record ID is required");
            }
            var record = await _borrowRecordRepository.GetRecordsById(recordId);
            if (record == null)
            {
                return NotFound("Record not found");
            }
            return Ok(record);
        }
    }
}
