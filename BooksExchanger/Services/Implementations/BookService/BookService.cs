using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.BookService;

/// <summary>
/// Предоставляет сервис для работы с книгами.
/// </summary>
public class BookService : IBookService
{
    private IBookRepository _bookRepository;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса <see cref="BookService"/>.
    /// </summary>
    /// <param name="bookRepository">Репозиторий книг, используемый сервисом.</param>
    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    /// <summary>
    /// Добавляет новую книгу.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <param name="genreId">Идентификатор жанра книги.</param>
    /// <param name="authorIds">Список идентификаторов авторов книги.</param>
    /// <returns>Идентификатор добавленной книги.</returns>
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

    /// <summary>
    /// Обновляет информацию о существующей книге.
    /// </summary>
    /// <param name="bookId">Идентификатор книги для обновления.</param>
    /// <param name="title">Новое название книги.</param>
    /// <param name="genreId">Идентификатор нового жанра книги.</param>
    /// <param name="authorIds">Список идентификаторов авторов книги.</param>
    /// <returns>Идентификатор обновленной книги.</returns>
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

    /// <summary>
    /// Получает список всех книг или книгу по её идентификатору.
    /// </summary>
    /// <param name="bookId">Необязательный идентификатор книги.</param>
    /// <returns>Список книг.</returns>
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
    
    /// <summary>
    /// Ищет книги по началу названия.
    /// </summary>
    /// <param name="title">Начало названия книги для поиска.</param>
    /// <returns>Перечисление книг, начинающихся с указанного названия.</returns>
    public IEnumerable<Book> SearchBooks(string title)
    {
        var filter = (string bookTitle) => { return bookTitle.ToLower().StartsWith(title.ToLower()); };
        return _bookRepository.GetBooks(filter);
    }

    /// <summary>
    /// Удаляет книгу по её идентификатору.
    /// </summary>
    /// <param name="bookId">Идентификатор книги.</param>
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