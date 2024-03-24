namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с авторами.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Добавляет нового автора в систему.
    /// </summary>
    /// <param name="name">Имя автора.</param>
    /// <returns>Идентификатор добавленного автора.</returns>
    long InsertAuthor(string name);
    
    /// <summary>
    /// Обновляет информацию об авторе.
    /// </summary>
    /// <param name="id">Идентификатор автора, информацию о котором необходимо обновить.</param>
    /// <param name="name">Новое имя автора.</param>
    /// <returns>Идентификатор обновленного автора.</returns>
    long UpdateAuthor(long id, string name);

    /// <summary>
    /// Получает список всех авторов.
    /// </summary>
    /// <returns>Коллекция авторов.</returns>
    IEnumerable<Models.Author> GetAuthors();

    /// <summary>
    /// Получает список авторов, соответствующих заданному условию.
    /// </summary>
    /// <param name="filter">Функция фильтрации авторов по их имени.</param>
    /// <returns>Коллекция отфильтрованных авторов.</returns>
    IEnumerable<Models.Author> GetAuthors(Func<string, bool> filter);

    /// <summary>
    /// Удаляет автора по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора, которого необходимо удалить.</param>
    void RemoveAuthor(long id);
}