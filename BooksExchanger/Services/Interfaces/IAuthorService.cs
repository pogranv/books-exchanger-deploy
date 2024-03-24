using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с авторами.
/// </summary>
public interface IAuthorService
{
    /// <summary>
    /// Добавляет нового автора.
    /// </summary>
    /// <param name="name">Имя автора.</param>
    /// <returns>Идентификатор добавленного автора.</returns>
    long AddAuthor(string name);

    /// <summary>
    /// Обновляет информацию об авторе.
    /// </summary>
    /// <param name="id">Идентификатор автора.</param>
    /// <param name="name">Новое имя автора.</param>
    /// <returns>Идентификатор обновленного автора.</returns>
    long UpdateAuthor(long id, string name);

    /// <summary>
    /// Получает список всех авторов.
    /// </summary>
    /// <returns>Коллекция авторов.</returns>
    IEnumerable<Author> GetAuthors();

    /// <summary>
    /// Поиск авторов по имени.
    /// </summary>
    /// <param name="name">Имя автора для поиска.</param>
    /// <returns>Коллекция авторов, чьи имена соответствуют заданному условию.</returns>
    IEnumerable<Author> SearchAuthors(string name);

    /// <summary>
    /// Удаляет автора по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    void RemoveAuthor(long id);
}