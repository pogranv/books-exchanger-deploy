namespace BooksExchanger.Entities;

/// <summary>
/// Объект отзыва 
/// </summary>
public partial class Feedback
{
    /// <summary>
    /// id отзыва.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// id книги.
    /// </summary>
    public long? BookId { get; set; }

    /// <summary>
    /// Содержание отзыва.
    /// </summary>
    public string Feedback1 { get; set; } = null!;

    /// <summary>
    /// id автора отзыва.
    /// </summary>
    public long? GivenByUserId { get; set; }

    /// <summary>
    /// Оценка.
    /// </summary>
    public int? Estimation { get; set; }

    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время удаления.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Связанная книга.
    /// </summary>
    public virtual Book? Book { get; set; }

    /// <summary>
    /// Информация лб авторе отзыва.
    /// </summary>
    public virtual User? GivenByUser { get; set; }
}
