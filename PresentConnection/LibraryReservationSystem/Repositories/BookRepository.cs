using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryReservationSystem.Data;

namespace LibraryReservationSystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext _Context) =>
            _context = _Context;
        public async Task AddBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetFilteredBooks(string searchTerm, string searchYear, bool showAudiobooks, bool showPhysicalBooks)
        {
            var booksQuery = _context.Books.AsQueryable();

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
                    throw new ArgumentException("Invalid year format.");
                }
            }

            if (showAudiobooks && showPhysicalBooks)
            {
                
            }
            else if (showAudiobooks)
            {
                booksQuery = booksQuery.Where(b => b.Audiobook);
            }
            else if (showPhysicalBooks)
            {
                booksQuery = booksQuery.Where(b => b.PhysicalBook);
            }
            else
            {
                
                return new List<Book>();
            }

            return await booksQuery.ToListAsync();
        }

        public async Task UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}