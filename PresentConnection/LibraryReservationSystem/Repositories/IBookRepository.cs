
public interface IBookRepository
{   Task <List<Book>> Get();
    Task <Book?> GetBookById (int id);
    Task AddBook (Book book);
    Task UpdateBook (Book book);
    Task DeleteBook (Book book);

    Task<List<Book>> GetFilteredBooks(string searchTerm, string searchYear, bool showAudiobooks, bool showPhysicalBooks);
}
