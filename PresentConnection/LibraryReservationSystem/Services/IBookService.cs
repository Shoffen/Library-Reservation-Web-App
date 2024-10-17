
namespace LibraryReservationSystem.Services
{
    public interface IBookService
{
    Task<List<Book>> GetFilteredBooks(string searchTerm, string searchYear, bool showAudiobooks, bool showPhysicalBooks);
    Task<Book?> GetBookByIdAsync(int id);
    Task AddBookAsync(Book newBook);
    Task UpdateBookAsync(Book updatedBook);
    Task DeleteBookAsync(Book book);
}

}