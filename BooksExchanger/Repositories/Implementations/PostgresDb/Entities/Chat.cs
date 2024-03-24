namespace BooksExchanger.Entities;

/// <summary>
/// Объект чата в БД.
/// </summary>
public partial class Chat
{
    /// <summary>
    /// id чата.
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// id первого юзера.
    /// </summary>
    public long User1Id { get; set; }

    /// <summary>
    /// id второго юзера.
    /// </summary>
    public long User2Id { get; set; }

    /// <summary>
    /// Дата и время создания чата.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Связанные с чатом сообщения.
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    /// <summary>
    /// Информация о первом пользователе.
    /// </summary>
    public virtual User User1 { get; set; } = null!;

    /// <summary>
    /// Информация о втором пользователе.
    /// </summary>
    public virtual User User2 { get; set; } = null!;
}
