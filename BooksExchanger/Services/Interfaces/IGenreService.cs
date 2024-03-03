using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IGenreService
{
    int AddGenre(string name);

    int UpdateGenre(int id, string name);

    IEnumerable<Genre> GetGenres();

    IEnumerable<Genre> SearchGenres(string name);

    void RemoveGenre(int id);
}