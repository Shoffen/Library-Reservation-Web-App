using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryReservationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryReservationSystem.Data;

namespace LibraryReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BookController(LibraryContext context)
        {
            _context = context;

            // Ensure we have some seed data if the database is empty
            if (!_context.Books.Any())
            {
                _context.Books.AddRange(new List<Book>
                {
                    new Book("The Great Gatsby", 1925, true, false, "https://example.com/gatsby.jpg"),
                    new Book("1984", 1949, false, true, "https://example.com/1984.jpg"),
                    new Book("To Kill a Mockingbird", 1960, true, true, "https://example.com/mockingbird.jpg")
                });
                _context.SaveChanges();
            }
        }

        // GET: api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
            [FromQuery] string searchTerm = "", 
            [FromQuery] string searchYear = "", 
            [FromQuery] bool showAudiobooks = true, 
            [FromQuery] bool showPhysicalBooks = true)
        {
            var booksQuery = _context.Books.AsQueryable();

            // Apply search filters based on user input
            if (!string.IsNullOrEmpty(searchTerm))
            {
                booksQuery = booksQuery.Where(b => b.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchYear))
            {
                if (int.TryParse(searchYear, out int year))
                {
                    booksQuery = booksQuery.Where(b => b.Year.ToString().Contains(searchYear));
                }
                else
                {
                    return BadRequest("Invalid year format.");
                }
            }

            // Filter logic for audiobooks and physical books
    if (showAudiobooks && showPhysicalBooks)
    {
        // If both are true, do not filter out any books
        // booksQuery remains unchanged
    }
    else if (showAudiobooks)
    {
        // Include books that are audiobooks, which can also be physical
        booksQuery = booksQuery.Where(b => b.Audiobook);
    }
    else if (showPhysicalBooks)
    {
        // Include books that are physical books, which can also be audiobooks
        booksQuery = booksQuery.Where(b => b.PhysicalBook);
    }
    else
    {
        // If both are false, return an empty list
        return Ok(new List<Book>());
    }

            var books = await booksQuery.ToListAsync();

            return Ok(books);
        }


        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

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
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

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

            _context.Entry(updatedBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(b => b.Id == id))
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
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
