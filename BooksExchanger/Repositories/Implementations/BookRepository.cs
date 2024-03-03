using System.Net;
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Books;
using BooksExchanger.Entities;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Book = BooksExchanger.Entities.Book;

namespace BooksExchanger.Repositories.Implementations;

/// <summary>
/// Хранилище книг.
/// </summary>
public class BookRepository : IBookRepository
{
    private ResponseMapper _responseMapper;

    public BookRepository()
    {
        _responseMapper = new();
    }
    
    /// <summary>
    /// Добавляет новую книгу.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <param name="genreId">Жанр.</param>
    /// <param name="authorIds">Авторы.</param>
    /// <returns>id книги.</returns>
    public long InsertBook(string title, int genreId, List<long> authorIds)
    {
        using (DbCtx db = new DbCtx())
        {
            try
            {
                var book = new Book { Title = title, GenreId = genreId };
                foreach (var authorId in authorIds)
                {
                    var author = db.Authors.FirstOrDefault(author => author.Id == authorId);
                    if (author == null)
                    {
                        throw new AuthorNotFoundException($"Автора с id={authorId} не существует");
                    }

                    book.Authors.Add(author);
                }
                db.Books.Add(book);
                db.SaveChanges();
                return book.Id;
            } 
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException is PostgresException postgresException && postgresException.ConstraintName == Constants.BooksGenreFK)
                {
                    throw new GenreNotFoundException($"Жанра с id={genreId} не существует");
                }

                throw;
            }
        }
    }

    /// <summary>
    /// Обновляет книгу.
    /// </summary>
    /// <param name="bookId">Книга.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="genreId">Жанр.</param>
    /// <param name="authorIds">Авторы.</param>
    /// <returns>id книги.</returns>
    public long UpdateBook(long bookId, string title, int genreId, List<long> authorIds)
    {
        using (DbCtx db = new DbCtx())
        {
            var book = db.Books.Include(book => book.Authors).FirstOrDefault(book => book.Id == bookId);
            if (book == null)
            {
                throw new BookNotFoundException($"Не найдено книги с id={bookId}");
            }

            try
            {
                book.GenreId = genreId;
                book.Title = title;
                book.Authors.Clear();
                foreach (var authorId in authorIds)
                {
                    var author = db.Authors.FirstOrDefault(author => author.Id == authorId);
                    if (author == null)
                    {
                        throw new AuthorNotFoundException($"Автора с id={authorId} не существует");
                    }
                    book.Authors.Add(author);
                }
                db.SaveChanges();

                return book.Id;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException is PostgresException postgresException && postgresException.ConstraintName == Constants.BooksGenreFK)
                {
                    throw new GenreNotFoundException($"Жанра с id={genreId} не существует");
                }

                throw;
            }
        }
    }

    /// <summary>
    /// Получение книг.
    /// </summary>
    /// <param name="filter">Фильтр.</param>
    /// <returns>Книги.</returns>
    public List<Models.Book> GetBooks(Func<long, bool> filter)
    {
        using (DbCtx db = new DbCtx())
        {
            var books = db.Books
                .Include(book => book.Genre)
                .Include(book => book.Authors)
                .AsEnumerable()
                .Where(book => filter(book.Id))
                .ToList()
                .ConvertAll(_responseMapper.MapBook);
            return books;
        }
    }

    /// <summary>
    /// Получение книг.
    /// </summary>
    /// <param name="filter">Фильтр.</param>
    /// <returns>Книги.</returns>
    public IEnumerable<Models.Book> GetBooks(Func<string, bool> filter)
    {
        using (DbCtx db = new DbCtx())
        {
            return db.Books
                .Include(book => book.Genre)
                .Include(book => book.Authors)
                .AsEnumerable()
                .Where(book => filter(book.Title))
                .ToList()
                .ConvertAll(_responseMapper.MapBook);
        }
    }

    /// <summary>
    /// Удаление книги по id.
    /// </summary>
    /// <param name="bookId">Книга.</param>
    public void RemoveBook(long bookId)
    {
        try
        {
            using (DbCtx db = new DbCtx())
            {
                var book = db.Books.FirstOrDefault(book => book.Id == bookId);
                if (book == null)
                {
                    throw new BookNotFoundException($"Книги с id={bookId} не существует");
                }
                db.Books.Remove(book);
                db.SaveChanges();
            }
        } 
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            if (innerException is PostgresException postgresException)
            {
                if (postgresException.ConstraintName == Constants.FeedbacksBookIdFK)
                {
                    throw new RemoveBookNotAllowedException("Невозможно удалить книгу, так как к ней привязаны отзывы");
                }
                if (postgresException.ConstraintName == Constants.OffersBookIdFK)
                {
                    throw new RemoveBookNotAllowedException("Невозможно удалить книгу, так как к ней привязаны офферы");
                }
            }

            throw;
        }
    }
}