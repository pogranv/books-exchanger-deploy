using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

// <summary>
// Определяет интерфейс сервиса для работы с книгами.
// </summary>
public interface IBookService
{
    // <summary>
    // Добавляет новую книгу в систему.
    // </summary>
    // <param name="title">Название книги.</param>
    // <param name="genreId">Идентификатор жанра книги.</param>
    // <param name="authorIds">Список идентификаторов авторов книги.</param>
    // <returns>Идентификатор добавленной книги.</returns>
    long AddBook(string title, int genreId, List<long> authorIds);
        
    // </summary>
    // <param name="bookId">Идентификатор книги для обновления.</param>
    // <param name="title">Новое название книги.</param>
    // <param name="genreId">Новый идентификатор жанра книги.</param>
    // <param name="authorIds">Новый список идентификаторов авторов книги.</param>
    // <returns>Идентификатор обновлённой книги.</returns>
    public long UpdateBook(long bookId, string title, int genreId, List<long> authorIds);

    // <summary>
    // Возвращает список книг. Можно получить как все книги, так и информацию о конкретной книге.
    // </summary>
    // <param name="bookId">Необязательный идентификатор книги. Если задан, возвращает информацию только об этой книге; если не задан, возвращает все книги.</param>
    // <returns>Список книг в зависимости от предоставленного идентификатора.</returns>
    public List<Book> GetBooks(long? bookId);

    // <summary>
    // Ищет книги по названию.
    // </summary>
    // <param name="title">Название книги для поиска.</param>
    // <returns>Коллекцию книг, соответствующих заданному названию.</returns>
    public IEnumerable<Book> SearchBooks(string title);

    
    // <summary>
    // Удаляет книгу из системы по её идентификатору.
    // </summary>
    // <param name="bookId">Идентификатор удаляемой книги.</param>
    public void RemoveBook(long bookId);
}