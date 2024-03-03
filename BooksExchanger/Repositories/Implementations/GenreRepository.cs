using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs.Genres;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Genre = BooksExchanger.Entities.Genre;

namespace BooksExchanger.Repositories.Implementations;


public class GenreRepository : IGenreRepository
{
    private ResponseMapper _responseMapper;

    public GenreRepository()
    {
        _responseMapper = new();
    }
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

    public IEnumerable<Models.Genre> GetGenres()
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Genres.ToList().ConvertAll(_responseMapper.MapGenre);
        }
    }

    public IEnumerable<Models.Genre> GetGenres(Func<string, bool> filter)
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Genres.AsEnumerable().Where(genre => filter(genre.Name)).ToList().ConvertAll(_responseMapper.MapGenre);
        }
    }

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