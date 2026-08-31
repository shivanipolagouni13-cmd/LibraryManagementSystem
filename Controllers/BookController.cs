using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Book")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService) =>
        _bookService = bookService;

    [HttpGet]
    public async Task<List<Book>> Get() =>
        await _bookService.GetAllAsync();

    [HttpGet("{id}")]  
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _bookService.GetAsync(id);
        return book == null ? NotFound() : book;
    }

    [HttpGet("search")]  
    public async Task<ActionResult<Book>> GetByName([FromQuery] string bookName)
    {
        var book = await _bookService.GetByName(bookName);
        return book == null ? NotFound() : Ok(book);
    }

    [HttpGet("availability/{bookId}")]
    public async Task<IActionResult> CheckAvailability(string bookId)
    {
        var book = await _bookService.GetAsync(bookId);
        if (book == null)
            return NotFound($"Book with ID {bookId} not found");
        
        return Ok(new { 
            BookId = book.ID,
            Title = book.Title,
            IsAvailable = book.IsAvailable,
            Quantity = book.Quantity
        });
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdBook = await _bookService.CreateAsync(book);
        return CreatedAtAction(nameof(Get), new { id = createdBook.ID }, createdBook);
    }

    [HttpPut("{id}")]  
    public async Task<IActionResult> Update(string id, Book book)
    {
        await _bookService.UpdateAsync(id, book);
        return NoContent();
    }

    [HttpDelete("{id}")]  
    public async Task<IActionResult> Delete(string id)
    {
        await _bookService.DeleteAsync(id);
        return NoContent();
    }
}