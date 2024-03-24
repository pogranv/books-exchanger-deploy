namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Оффер уже находится в избранном.
/// </summary>
public class OfferAlreadyFavoriteException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyFavoriteException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyFavoriteException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyFavoriteException(string message, Exception inner) : base(message, inner)
    {
    }
}