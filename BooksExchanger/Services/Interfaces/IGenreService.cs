using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

// <summary>
// Определяет интерфейс сервиса для работы с жанрами.
// </summary>
public interface IGenreService
{
    /// <summary>
    /// Добавляет новый жанр.
    /// </summary>
    /// <param name="name">Название жанра.</param>
    /// <returns>Идентификатор добавленного жанра.</returns>
    int AddGenre(string name);

    /// <summary>
    /// Обновляет информацию по существующему жанру.
    /// </summary>
    /// <param name="id">Идентификатор жанра.</param>
    /// <param name="name">Новое название жанра.</param>
    /// <returns>Идентификатор обновленного жанра.</returns>
    int UpdateGenre(int id, string name);

    /// <summary>
    /// Возвращает список всех жанров.
    /// </summary>
    /// <returns>Коллекцию всех жанров.</returns>
    IEnumerable<Genre> GetGenres();

    /// <summary>
    /// Ищет жанры по названию.
    /// </summary>
    /// <param name="name">Название жанра для поиска.</param>
    /// <returns>Коллекцию жанров, соответствующих заданному названию.</returns>
    IEnumerable<Genre> SearchGenres(string name);

    /// <summary>
    /// Удаляет жанр по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого жанра.</param>
    void RemoveGenre(int id);
}