namespace BooksExchanger.Repositories.Interfaces;

public interface IBookRepository
{
    public long InsertBook(string title, int genreId, List<long> authorIds);

    public long UpdateBook(long bookId, string title, int genreId, List<long> authorIds);

    public List<Models.Book> GetBooks(Func<long, bool> filter);
    
    IEnumerable<Models.Book> GetBooks(Func<string, bool> filter);

    public void RemoveBook(long bookId);
}