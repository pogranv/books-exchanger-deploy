namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Пользователь не найден.
/// </summary>
public class UserNotFoundException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserNotFoundException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}