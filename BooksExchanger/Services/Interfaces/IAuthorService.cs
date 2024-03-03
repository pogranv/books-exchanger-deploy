using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IAuthorService
{
    long AddAuthor(string name);

    long UpdateAuthor(long id, string name);

    IEnumerable<Author> GetAuthors();

    IEnumerable<Author> SearchAuthors(string name);

    void RemoveAuthor(long id);
}