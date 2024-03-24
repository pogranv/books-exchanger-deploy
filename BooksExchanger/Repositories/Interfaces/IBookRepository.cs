namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Определяет методы для работы с книгами в хранилище.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Добавляет новую книгу в хранилище.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <param name="genreId">Идентификатор жанра книги.</param>
    /// <param name="authorIds">Список идентификаторов авторов книги.</param>
    /// <returns>Идентификатор добавленной книги.</returns>
    public long InsertBook(string title, int genreId, List<long> authorIds);

    /// <summary>
    /// Обновляет информацию о книге в хранилище.
    /// </summary>
    /// <param name="bookId">Идентификатор книги, информацию о которой нужно обновить.</param>
    /// <param name="title">Новое название книги.</param>
    /// <param name="genreId">Новый идентификатор жанра книги.</param>
    /// <param name="authorIds">Новый список идентификаторов авторов книги.</param>
    /// <returns>Идентификатор обновленной книги.</returns>
    public long UpdateBook(long bookId, string title, int genreId, List<long> authorIds);

    /// <summary>
    /// Получает книги из хранилища по заданному фильтру по идентификатору.
    /// </summary>
    /// <param name="filter">Функция фильтрации по идентификатору книги.</param>
    /// <returns>Список книг, удовлетворяющих условиям фильтра.</returns>
    public List<Models.Book> GetBooks(Func<long, bool> filter);
    
    /// <summary>
    /// Получает книги из хранилища по заданному фильтру по названию.
    /// </summary>
    /// <param name="filter">Функция фильтрации по названию книги.</param>
    /// <returns>Перечисление книг, удовлетворяющих условиям фильтра.</returns>
    IEnumerable<Models.Book> GetBooks(Func<string, bool> filter);

    /// <summary>
    /// Удаляет книгу из хранилища по её идентификатору.
    /// </summary>
    /// <param name="bookId">Идентификатор книги для удаления.</param>
    public void RemoveBook(long bookId);
}