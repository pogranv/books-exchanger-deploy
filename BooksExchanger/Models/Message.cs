namespace BooksExchanger.Models;

/// <summary>
/// Модель сообщения.
/// </summary>
public class Message
{
    /// <summary>
    /// Id 
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Текс сообщения.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Id автора сообщения
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// Дата и время отправки сообщения
    /// </summary>
    public DateTime SentAt { get; set; }
}