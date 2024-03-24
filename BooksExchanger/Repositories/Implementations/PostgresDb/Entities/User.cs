using System.ComponentModel.DataAnnotations.Schema;

namespace BooksExchanger.Entities;

/// <summary>
/// Объект пользователя в БД.
/// </summary>
public partial class User
{
    /// <summary>
    /// id пользователя.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Почта.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; } = null!;
    
    /// <summary>
    /// Роль.
    /// </summary>
    [Column("role")]
     public UserRole Role { get; set; }

    /// <summary>
    /// Чаты пользователя.
    /// </summary>
    public virtual ICollection<Chat> ChatUser1s { get; set; } = new List<Chat>();

    /// <summary>
    /// Чаты пользователя.
    /// </summary>
    public virtual ICollection<Chat> ChatUser2s { get; set; } = new List<Chat>();

    /// <summary>
    /// Отзывы пользователя.
    /// </summary>
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    /// <summary>
    /// Сообщения пользователя.
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    /// <summary>
    /// Офферы пользователя.
    /// </summary>
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    /// <summary>
    /// Офферы пользователя на модерации.
    /// </summary>
    public virtual ICollection<OffersCollector> OffersCollectors { get; set; } = new List<OffersCollector>();

    /// <summary>
    /// Избранные офферы пользователя.
    /// </summary>
    public virtual ICollection<Offer> OffersNavigation { get; set; } = new List<Offer>();
}
