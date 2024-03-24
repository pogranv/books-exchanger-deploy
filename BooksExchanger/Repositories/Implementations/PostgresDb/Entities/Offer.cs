namespace BooksExchanger.Entities;

/// <summary>
/// Объект оффера в БД.
/// </summary>
public partial class Offer
{
    /// <summary>
    /// id оффера.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// id книги.
    /// </summary>
    public long? BookId { get; set; }

    /// <summary>
    /// id владельца.
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Город размещения оффера.
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// Ссылка на картинку.
    /// </summary>
    public string? Picture { get; set; }

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
    /// Информация о владльце.
    /// </summary>
    public virtual User? Owner { get; set; }

    /// <summary>
    /// Пользователи, у которых оффер в избранном.
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
