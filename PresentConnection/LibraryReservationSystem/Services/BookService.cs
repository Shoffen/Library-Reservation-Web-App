
namespace LibraryReservationSystem.Services
{
    public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetFilteredBooks(string searchTerm, string searchYear, bool showAudiobooks, bool showPhysicalBooks)
    {
        return await _bookRepository.GetFilteredBooks(searchTerm, searchYear, showAudiobooks, showPhysicalBooks);
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _bookRepository.GetBookById(id);
    }

    public async Task AddBookAsync(Book newBook)
    {
        await _bookRepository.AddBook(newBook);
    }

    public async Task UpdateBookAsync(Book updatedBook)
    {
        await _bookRepository.UpdateBook(updatedBook);
    }

    public async Task DeleteBookAsync(Book book)
    {
        await _bookRepository.DeleteBook(book);
    }
}

}