namespace BooksExchanger.Services.Implementations.UserService.Exceptions;

/// <summary>
/// Пароль неверный.
/// </summary>
public class IncorrectPasswordException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncorrectPasswordException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncorrectPasswordException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncorrectPasswordException(string message, Exception inner) : base(message, inner)
    {
    }
}