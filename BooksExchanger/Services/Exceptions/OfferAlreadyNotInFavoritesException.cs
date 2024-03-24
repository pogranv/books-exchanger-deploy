namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Оффер уже удален из избранного пользователя.
/// </summary>
public class OfferAlreadyNotInFavoritesException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyNotInFavoritesException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyNotInFavoritesException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyNotInFavoritesException(string message, Exception inner) : base(message, inner)
    {
    }
}