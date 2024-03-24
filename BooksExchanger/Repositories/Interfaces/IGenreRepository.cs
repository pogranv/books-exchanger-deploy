namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Предоставляет интерфейс для работы с жанрами в репозитории.
/// </summary>
public interface IGenreRepository
{
    /// <summary>
    /// Добавляет новый жанр в репозиторий.
    /// </summary>
    /// <param name="name">Название жанра.</param>
    /// <returns>Идентификатор добавленного жанра.</returns>
    int InsertGenre(string name);
    
    /// <summary>
    /// Обновляет информацию о жанре.
    /// </summary>
    /// <param name="id">Идентификатор жанра, который необходимо обновить.</param>
    /// <param name="name">Новое название жанра.</param>
    /// <returns>Идентификатор обновленного жанра.</returns>
    int UpdateGenre(int id, string name);

    /// <summary>
    /// Получает все жанры из репозитория.
    /// </summary>
    /// <returns>Перечисление жанров.</returns>
    IEnumerable<Models.Genre> GetGenres();

    /// <summary>
    /// Получает жанры, удовлетворяющие заданному условию фильтра.
    /// </summary>
    /// <param name="filter">Функция фильтрации жанров по названию.</param>
    /// <returns>Перечисление жанров, удовлетворяющих условию фильтра.</returns>
    IEnumerable<Models.Genre> GetGenres(Func<string, bool> filter);

    /// <summary>
    /// Удаляет жанр из репозитория по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор жанра, который необходимо удалить.</param>
    void RemoveGenre(int id);
}