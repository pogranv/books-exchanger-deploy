namespace BooksExchanger.Models;

/// <summary>
/// Модель отзыва.
/// </summary>
public class Feedback
{
    /// <summary>
    /// Id отзыва.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Текст отзыва.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Id автора отзыва.
    /// </summary>
    public long? GivenByUserId { get; set; }

    /// <summary>
    /// Оценка книги пользователя.
    /// </summary>
    public int? Estimation { get; set; }
    
    /// <summary>
    /// Имя автора.
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Оценка книги.
    /// </summary>
    public BookEstimation BookEstimation { get; set; }

    /// <summary>
    /// Дата и время создания отзыва.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}