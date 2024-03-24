namespace BooksExchanger.Services.Implementations.UserService.Exceptions;

/// <summary>
/// Пользователь уже существует.
/// </summary>
public class UserAlreadyExistExeption : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserAlreadyExistExeption()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserAlreadyExistExeption(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserAlreadyExistExeption(string message, Exception inner) : base(message, inner)
    {
    }
}