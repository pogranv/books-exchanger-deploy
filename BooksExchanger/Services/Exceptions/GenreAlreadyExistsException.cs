namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Жанр уже существует.
/// </summary>
public class GenreAlreadyExistsException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public GenreAlreadyExistsException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GenreAlreadyExistsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GenreAlreadyExistsException(string message, Exception inner) : base(message, inner)
    {
    }
}