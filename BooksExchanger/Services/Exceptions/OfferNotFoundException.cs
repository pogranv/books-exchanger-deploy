namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Оффер не найден.
/// </summary>
public class OfferNotFoundException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferNotFoundException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OfferNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}