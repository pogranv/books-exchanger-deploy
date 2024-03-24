namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Не хватает прав.
/// </summary>
public class PermissionDenidedException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public PermissionDenidedException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PermissionDenidedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PermissionDenidedException(string message, Exception inner) : base(message, inner)
    {
    }
}