using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IBookService
{
    long AddBook(string title, int genreId, List<long> authorIds);

    public long UpdateBook(long bookId, string title, int genreId, List<long> authorIds);

    public List<Book> GetBooks(long? bookId);

    public IEnumerable<Book> SearchBooks(string title);

    public void RemoveBook(long bookId);
    
    
}