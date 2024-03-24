using Npgsql;

using Microsoft.EntityFrameworkCore;

using BooksExchanger.Context;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;

using Author = BooksExchanger.Entities.Author;

namespace BooksExchanger.Repositories.Implementations;

/// <summary>
/// Хранилице авторов.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private ResponseMapper _responseMapper;

    /// <summary>
    /// Конструктор хранилища авторов.
    /// </summary>
    public AuthorRepository()
    {
        _responseMapper = new();
    }
    
    /// <summary>
    /// Добавление автора.
    /// </summary>
    /// <param name="name">Имя автора.</param>
    /// <returns>id атора.</returns>
    public long InsertAuthor(string name)
    {
        using (DbCtx db = new DbCtx())
        {
            var author = new Author { Name = name };
            db.Authors.Add(author);
            db.SaveChanges();
            return author.Id;
        }
    }

    /// <summary>
    /// Обновляет автора по id.
    /// </summary>
    /// <param name="id">id автора.</param>
    /// <param name="name">Имя автора.</param>
    /// <returns>id автора.</returns>
    public long UpdateAuthor(long id, string name)
    {
        using (DbCtx db = new DbCtx())
        {
            var author = db.Authors.FirstOrDefault(author => author.Id == id);
            if (author == null)
            {
                throw new AuthorNotFoundException($"Автора с id = {id} не существует");
            }

            author.Name = name;
            db.SaveChanges();
            return author.Id;
        }
    }

    /// <summary>
    /// Получение авторов.
    /// </summary>
    /// <returns>Авторы.</returns>
    public IEnumerable<Models.Author> GetAuthors()
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Authors.ToList().ConvertAll(_responseMapper.MapAuthor);
        }
    }

    /// <summary>
    /// Получение авторов с фильтрами.
    /// </summary>
    /// <param name="filter">Фильтры.</param>
    /// <returns>Авторы.</returns>
    public IEnumerable<Models.Author> GetAuthors(Func<string, bool> filter)
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Authors.AsEnumerable().Where(author => filter(author.Name)).ToList().ConvertAll(_responseMapper.MapAuthor);
        }
    }

    /// <summary>
    /// Удаление автора по id.
    /// </summary>
    /// <param name="id">id автора.</param>
    public void RemoveAuthor(long id)
    {
        try
        {
            using (DbCtx db = new DbCtx())
            {
                var author = db.Authors.FirstOrDefault(author => author.Id == id);
                if (author == null)
                {
                    throw new AuthorNotFoundException($"Автора с id={id} не существует");
                }
                
                db.Authors.Remove(author);
                db.SaveChanges();
            }
        } 
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            if (innerException is PostgresException postgresException && postgresException.ConstraintName == Constants.AuthorsBooksAuthorIdFK)
            {
                throw new RemoveAuthorNotAllowedException(
                    $"Нельзя удалить автора с id={id}, так как к нему привязаны книги");
            }
            throw;
        }
    }
}