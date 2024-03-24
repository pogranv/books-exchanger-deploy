namespace BooksExchanger.Entities;

/// <summary>
/// Объект книги в БД.
/// </summary>
public partial class Book
{
    /// <summary>
    /// id книги.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Заголовок книги.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// id жанра
    /// </summary>
    public int? GenreId { get; set; }

    /// <summary>
    /// Сумма оценок иниги.
    /// </summary>
    public long? SumRating { get; set; }

    /// <summary>
    /// Количество оценок книги.
    /// </summary>
    public long? CountRating { get; set; }

    /// <summary>
    /// Дата и время создания книги.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время удаления книги.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Отзывы на книгу.
    /// </summary>
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    /// <summary>
    /// Информация о жанре.
    /// </summary>
    public virtual Genre? Genre { get; set; }

    /// <summary>
    /// Связанные с книгой офферы.
    /// </summary>
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    /// <summary>
    /// Связанные с книгой авторы.
    /// </summary>
    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
