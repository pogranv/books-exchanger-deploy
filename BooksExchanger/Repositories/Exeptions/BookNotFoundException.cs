namespace BooksExchanger.Repositories.Exeptions;

/// <summary>
/// Книга не найдена.
/// </summary>
public class BookNotFoundException : Exception
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public BookNotFoundException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BookNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BookNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}