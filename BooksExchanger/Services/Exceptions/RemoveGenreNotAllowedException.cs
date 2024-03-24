namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Удалить жанр нельзя.
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