using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.AuthorService;

public class AuthorService : IAuthorService
{
    private IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public long AddAuthor(string name)
    {
        return _authorRepository.InsertAuthor(name);
    }

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

    public IEnumerable<Author> GetAuthors()
    {
        return _authorRepository.GetAuthors();
    }

    public IEnumerable<Author> SearchAuthors(string name)
    {
        var filter = (string authorName) => { return authorName.ToLower().StartsWith(name.ToLower()); };
        return _authorRepository.GetAuthors(filter);
    }

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