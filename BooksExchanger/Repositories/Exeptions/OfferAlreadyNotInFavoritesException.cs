namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Оффер не находистя в избранном пользвоателя.
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