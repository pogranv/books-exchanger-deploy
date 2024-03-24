namespace BooksExchanger.Services.Exceptions;

/// <summary>
/// Не найден отзыв.
/// </summary>
public class FeedbackNotFoundException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public FeedbackNotFoundException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FeedbackNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FeedbackNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}