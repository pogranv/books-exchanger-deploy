using Microsoft.EntityFrameworkCore;

using Npgsql;

using BooksExchanger.Context;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;

using Genre = BooksExchanger.Entities.Genre;

namespace BooksExchanger.Repositories.Implementations;


/// <summary>
/// Репозиторий для управления жанрами в базе данных.
/// </summary>
public class GenreRepository : IGenreRepository
{
    private ResponseMapper _responseMapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GenreRepository"/>.
    /// </summary>
    public GenreRepository()
    {
        _responseMapper = new();
    }
    
    /// <summary>
    /// Вставляет новый жанр в базу данных.
    /// </summary>
    /// <param name="name">Название жанра для вставки.</param>
    /// <returns>Сгенерированный идентификатор для вставленного жанра.</returns>
    /// <exception cref="GenreAlreadyExistsException">Исключение, возникающее, когда жанр с таким именем уже существует.</exception>
    public int InsertGenre(string name)
    {
        try
        {
            using (DbCtx db = new DbCtx())
            {
                Genre genre = new Genre { Name = name };
                db.Genres.Add(genre);
                db.SaveChanges();
                return genre.Id;
            }
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            if (innerException is PostgresException postgresException &&
                postgresException.ConstraintName == Constants.GenresNameKey)
            {
                throw new GenreAlreadyExistsException(
                    $"Жанр с таким названием уже существует");
            }

            throw;
        }
    }

    /// <summary>
    /// Обновляет название существующего жанра в базе данных.
    /// </summary>
    /// <param name="id">Идентификатор жанра для обновления.</param>
    /// <param name="name">Новое название для жанра.</param>
    /// <returns>Идентификатор обновленного жанра.</returns>
    /// <exception cref="GenreNotFoundException">Исключение, возникающее, когда жанр с указанным идентификатором не существует.</exception>
    /// <exception cref="GenreAlreadyExistsException">Исключение, возникающее, когда жанр с обновленным названием уже существует.</exception>
    public int UpdateGenre(int id, string name)
    {
        try
        {
            using (DbCtx db = new DbCtx())
            {
                var genre = db.Genres.FirstOrDefault(genre => genre.Id == id);
                if (genre == null)
                {
                    throw new GenreNotFoundException($"Жанра с id = {id} не существует");
                }

                genre.Name = name;
                db.SaveChanges();
                return genre.Id;
            }
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            if (innerException is PostgresException postgresException &&
                postgresException.ConstraintName == Constants.GenresNameKey)
            {
                throw new GenreAlreadyExistsException(
                    $"Жанр с таким названием уже существует");
            }

            throw;
        }
    }

    /// <summary>
    /// Извлекает коллекцию всех жанров из базы данных.
    /// </summary>
    /// <returns>Коллекция <see cref="Models.Genre"/>.</returns>
    public IEnumerable<Models.Genre> GetGenres()
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Genres.ToList().ConvertAll(_responseMapper.MapGenre);
        }
    }

    // <summary>
    /// Извлекает коллекцию жанров, соответствующих указанному фильтру.
    /// </summary>
    /// <param name="filter">Функция фильтрации названий жанров.</param>
    /// <returns>Коллекция <see cref="Models.Genre"/>.</returns>
    public IEnumerable<Models.Genre> GetGenres(Func<string, bool> filter)
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Genres.AsEnumerable().Where(genre => filter(genre.Name)).ToList().ConvertAll(_responseMapper.MapGenre);
        }
    }

    /// <summary>
    /// Удаляет жанр из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого жанра.</param>
    /// <exception cref="GenreNotFoundException">Исключение, возникающее, когда жанр с указанным идентификатором не существует.</exception>
    /// <exception cref="RemoveGenreNotAllowedException">Исключение, возникающее, когда невозможно удалить жанр, поскольку к нему привязаны книги.</exception>
    public void RemoveGenre(int id)
    {
        try
        {
            using (DbCtx db = new DbCtx())
            {
                var genre = db.Genres.FirstOrDefault(genre => genre.Id == id);
                if (genre == null)
                {
                    throw new GenreNotFoundException($"Жанра с id={id} не существует");
                }
    
                db.Genres.Remove(genre);
                db.SaveChanges();
            }
        } 
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            if (innerException is PostgresException postgresException && postgresException.ConstraintName == Constants.BooksGenreFK)
            {
                throw new RemoveGenreNotAllowedException(
                    $"Нельзя удалить жанр с id={id}, так как к нему привязаны книги");
            }
            throw;
        }
    }
}