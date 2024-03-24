namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Оффер уже подтвержден.
/// </summary>
public class OfferAlreadyApprovedException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyApprovedException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyApprovedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferAlreadyApprovedException(string message, Exception inner) : base(message, inner)
    {
    }
}