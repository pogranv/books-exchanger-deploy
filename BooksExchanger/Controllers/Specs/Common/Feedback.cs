namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель отзыва.
/// </summary>
public class Feedback
{
    /// <summary>
    /// id отзыва.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Имя автора отзыва.
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Дата и время создания отзыва.
    /// </summary>
    public string CreatedAt { get; set; }
    
    /// <summary>
    /// Текс отзыва.
    /// </summary>
    public string? Value { get; set; }
    
    /// <summary>
    /// Оценка книги по мнению пользователя (от 1 до 5).
    /// </summary>
    public int? Estimation { get; set; }
}