namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Нельзя удалить жанр.
/// </summary>
public class RemoveGenreNotAllowedException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveGenreNotAllowedException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveGenreNotAllowedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveGenreNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}