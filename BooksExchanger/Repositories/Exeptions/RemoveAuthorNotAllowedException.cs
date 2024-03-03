namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Нельзя удалить автора.
/// </summary>
public class RemoveAuthorNotAllowedException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveAuthorNotAllowedException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveAuthorNotAllowedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveAuthorNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}