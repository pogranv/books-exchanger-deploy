namespace BooksExchanger.Repositories.Interfaces;

public interface IGenreRepository
{
    int InsertGenre(string name);
    
    int UpdateGenre(int id, string name);

    IEnumerable<Models.Genre> GetGenres();

    IEnumerable<Models.Genre> GetGenres(Func<string, bool> filter);

    void RemoveGenre(int id);
}