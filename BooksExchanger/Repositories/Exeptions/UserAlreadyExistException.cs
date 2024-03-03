namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Пользователь уже существует.
/// </summary>
public class UserAlreadyExistException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserAlreadyExistException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserAlreadyExistException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserAlreadyExistException(string message, Exception inner) : base(message, inner)
    {
    }
}