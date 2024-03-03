using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Interfaces;


namespace BooksExchanger.Services.Implementations.GenreService;

public class GenreService : IGenreService
{
    private IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
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

    public IEnumerable<Genre> GetGenres()
    {
        return _genreRepository.GetGenres();
    }

    public IEnumerable<Genre> SearchGenres(string name)
    {
         var filter = (string genreName) => { return genreName.ToLower().StartsWith(name.ToLower()); };
         return _genreRepository.GetGenres(filter);
    }

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