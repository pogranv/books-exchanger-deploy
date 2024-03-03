namespace BooksExchanger.Repositories.Interfaces;

public interface IAuthorRepository
{
    long InsertAuthor(string name);
    
    long UpdateAuthor(long id, string name);

    IEnumerable<Models.Author> GetAuthors();

    IEnumerable<Models.Author> GetAuthors(Func<string, bool> filter);

    void RemoveAuthor(long id);
}