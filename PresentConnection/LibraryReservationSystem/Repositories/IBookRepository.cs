using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryReservationSystem.Models;

public interface IBookRepository
{   Task <List<Book>> Get();
    Task <Book?> GetBookById (int id);
    Task AddBook (Book book);
    Task UpdateBook (Book book);
    Task DeleteBook (Book book);

    Task<List<Book>> GetFilteredBooks(string searchTerm, string searchYear, bool showAudiobooks, bool showPhysicalBooks);
}
