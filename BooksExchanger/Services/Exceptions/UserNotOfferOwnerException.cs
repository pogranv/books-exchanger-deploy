namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Пользователь не является владельцем объявления.
/// </summary>
public class UserNotOfferOwnerException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserNotOfferOwnerException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserNotOfferOwnerException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UserNotOfferOwnerException(string message, Exception inner) : base(message, inner)
    {
    }
}
