using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryReservationSystem.Data;

namespace LibraryReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
            [FromQuery] string searchTerm = "", 
            [FromQuery] string searchYear = "", 
            [FromQuery] bool showAudiobooks = true, 
            [FromQuery] bool showPhysicalBooks = true)
        {
            try
            {
                var books = await _bookRepository.GetFilteredBooks(searchTerm, searchYear, showAudiobooks, showPhysicalBooks);
                return Ok(books);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/book
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book newBook)
        {
            await _bookRepository.AddBook(newBook);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest();
            }

            try
            {
                await _bookRepository.UpdateBook(updatedBook);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _bookRepository.GetBookById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteBook(book);

            return NoContent();
        }
    }
}
