namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Автор не найден.
/// </summary>
public class AuthorNotFoundException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public AuthorNotFoundException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AuthorNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AuthorNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}