using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.GenreService;

/// <summary>
/// Предоставляет сервис для работы с жанрами.
/// </summary>
public class GenreService : IGenreService
{
    private IGenreRepository _genreRepository;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса жанров.
    /// </summary>
    /// <param name="genreRepository">Репозиторий для работы с жанрами.</param>
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    /// <summary>
    /// Добавляет новый жанр.
    /// </summary>
    /// <param name="name">Название жанра.</param>
    /// <returns>Идентификатор добавленного жанра.</returns>
    public int AddGenre(string name)
    {
        try
        {
            return _genreRepository.InsertGenre(name);
        }
        catch (GenreAlreadyExistsException ex)
        {
            throw new Services.Exceptions.GenreAlreadyExistsException(ex.Message);
        }
    }

    /// <summary>
    /// Обновляет сведения о жанре.
    /// </summary>
    /// <param name="id">Идентификатор обновляемого жанра.</param>
    /// <param name="name">Новое название жанра.</param>
    /// <returns>Идентификатор обновленного жанра.</returns>
    public int UpdateGenre(int id, string name)
    {
        try
        {
            return _genreRepository.UpdateGenre(id, name);
        }
        catch (GenreNotFoundException ex)
        {
            throw new Exceptions.GenreNotFoundException(ex.Message);
        }
        catch (GenreAlreadyExistsException ex)
        {
            throw new Services.Exceptions.GenreAlreadyExistsException(ex.Message);
        }
    }

    /// <summary>
    /// Возвращает перечень всех жанров.
    /// </summary>
    /// <returns>Коллекцию жанров.</returns>
    public IEnumerable<Genre> GetGenres()
    {
        return _genreRepository.GetGenres();
    }

    public IEnumerable<Genre> SearchGenres(string name)
    {
         var filter = (string genreName) => { return genreName.ToLower().StartsWith(name.ToLower()); };
         return _genreRepository.GetGenres(filter);
    }

    /// <summary>
    /// Ищет жанры, соответствующие заданному имени.
    /// </summary>
    /// <param name="name">Название жанра для поиска.</param>
    /// <returns>Коллекцию найденных жанров.</returns>
    public void RemoveGenre(int id)
    {
        try
        {
            _genreRepository.RemoveGenre(id);
        }
        catch (GenreNotFoundException ex)
        {
            throw new Exceptions.GenreNotFoundException(ex.Message);
        }
        catch (RemoveGenreNotAllowedException ex)
        {
            throw new Exceptions.RemoveGenreNotAllowedException(ex.Message);
        }
    }
}