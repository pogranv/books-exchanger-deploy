using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.BookService;

public class BookService : IBookService
{
    private IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public long AddBook(string title, int genreId, List<long> authorIds)
    {
        try
        {
            return _bookRepository.InsertBook(title, genreId, authorIds);
        }
        catch (Repositories.Exeptions.AuthorNotFoundException ex)
        {
            throw new AuthorNotFoundException(ex.Message);
        }
        catch (Repositories.Exeptions.GenreNotFoundException ex)
        {
            throw new GenreNotFoundException(ex.Message);
        }
    }

    public long UpdateBook(long bookId, string title, int genreId, List<long> authorIds)
    {
        try
        {
            return _bookRepository.UpdateBook(bookId, title, genreId, authorIds);
        }
        catch (Repositories.Exeptions.BookNotFoundException ex)
        {
            throw new BookNotFoundException(ex.Message);
        }
        catch (Repositories.Exeptions.AuthorNotFoundException ex)
        {
            throw new AuthorNotFoundException(ex.Message);
        }
        catch (Repositories.Exeptions.GenreNotFoundException ex)
        {
            throw new GenreNotFoundException(ex.Message);
        }
    }

    public List<Book> GetBooks(long? bookId)
    {
        var filter = (long id) =>
        {
            if (bookId == null)
            {
                return true;
            }

            return bookId.Value == id;
        };
        return _bookRepository.GetBooks(filter);
    }
    
    public IEnumerable<Book> SearchBooks(string title)
    {
        var filter = (string bookTitle) => { return bookTitle.ToLower().StartsWith(title.ToLower()); };
        return _bookRepository.GetBooks(filter);
    }

    public void RemoveBook(long bookId)
    {
        try
        {
            _bookRepository.RemoveBook(bookId);
        }
        catch (Repositories.Exeptions.BookNotFoundException ex)
        {
            throw new BookNotFoundException(ex.Message);
        }
        catch (Repositories.Exeptions.RemoveBookNotAllowedException ex)
        {
            throw new RemoveBookNotAllowedException(ex.Message);
        }
    }
}