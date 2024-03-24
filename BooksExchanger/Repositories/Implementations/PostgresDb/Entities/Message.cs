namespace BooksExchanger.Entities;

/// <summary>
/// Объект сообщения в БД.
/// </summary>
public partial class Message
{
    /// <summary>
    /// id сообщения.
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// id чата.
    /// </summary>
    public long? ChatId { get; set; }

    /// <summary>
    /// id пользователя.
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Дата и время отправки.
    /// </summary>
    public DateTime SentAt { get; set; }

    /// <summary>
    /// Связанный чат.
    /// </summary>
    public virtual Chat? Chat { get; set; }

    /// <summary>
    /// Информация о пользователе.
    /// </summary>
    public virtual User? User { get; set; }
}
