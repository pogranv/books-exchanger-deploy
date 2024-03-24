namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Удалить книгу невозможно.
/// </summary>
public class RemoveBookNotAllowedException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveBookNotAllowedException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveBookNotAllowedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoveBookNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}