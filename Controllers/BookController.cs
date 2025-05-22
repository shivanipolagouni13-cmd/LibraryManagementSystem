using LibraryManagementSystem.Models;
using LibraryManagementSystem.MongoDBService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<Book>>> Get() => Ok(await _bookService.GetAllBooks());

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _bookService.GetBookById(id);
            return book == null ? NotFound() : Ok(book);
        }
        [HttpPost("create")]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            var createBook = await _bookService.CreateBook(book);
            return CreatedAtAction(nameof(Get), new { id = createBook.ID }, createBook);
        }
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book book)
        {
            await _bookService.UpdateBook(id,book);
            return Ok();
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _bookService.DeleteBook(id);
            return Ok();
        }

    }
}
