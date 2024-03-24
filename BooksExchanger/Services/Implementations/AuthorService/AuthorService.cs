using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.AuthorService;

/// <summary>
/// Сервис для работы с авторами.
/// </summary>
public class AuthorService : IAuthorService
{
    private IAuthorRepository _authorRepository;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса авторов.
    /// </summary>
    /// <param name="authorRepository">Репозиторий авторов.</param>
    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    /// <summary>
    /// Добавляет автора.
    /// </summary>
    /// <param name="name">Имя автора.</param>
    /// <returns>Идентификатор добавленного автора.</returns>
    public long AddAuthor(string name)
    {
        return _authorRepository.InsertAuthor(name);
    }

    /// <summary>
    /// Обновляет данные об авторе.
    /// </summary>
    /// <param name="id">Идентификатор автора.</param>
    /// <param name="name">Новое имя автора.</param>
    /// <returns>Идентификатор обновленного автора.</returns>
    public long UpdateAuthor(long id, string name)
    {
        try
        {
            return _authorRepository.UpdateAuthor(id, name);
        }
        catch (AuthorNotFoundException ex)
        {
            throw new Exceptions.AuthorNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Получает список всех авторов.
    /// </summary>
    /// <returns>Коллекция авторов.</returns>
    public IEnumerable<Author> GetAuthors()
    {
        return _authorRepository.GetAuthors();
    }

    /// <summary>
    /// Поиск авторов по имени.
    /// </summary>
    /// <param name="name">Имя или его часть для поиска авторов.</param>
    /// <returns>Коллекция авторов, соответствующих условию поиска.</returns>
    public IEnumerable<Author> SearchAuthors(string name)
    {
        var filter = (string authorName) => { return authorName.ToLower().StartsWith(name.ToLower()); };
        return _authorRepository.GetAuthors(filter);
    }

    /// <summary>
    /// Удаляет автора по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор автора для удаления.</param>
    public void RemoveAuthor(long id)
    {
        try
        {
            _authorRepository.RemoveAuthor(id);
        }
        catch (AuthorNotFoundException ex)
        {
            throw new Exceptions.AuthorNotFoundException(ex.Message);
        }
        catch (RemoveAuthorNotAllowedException ex)
        {
            throw new Exceptions.RemoveAuthorNotAllowedException(ex.Message);
        }
    }
}