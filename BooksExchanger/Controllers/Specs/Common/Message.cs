namespace BooksExchanger.Controllers.Specs.Common;


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
    /// Флаг, что это сообщение текущего пользователя (для окрашивания сообщений разными цветами)
    /// </summary>
    public bool IsUserSender { get; set; }
    
    /// <summary>
    /// Id автора сообщения
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// Дата и время отправки сообщения
    /// </summary>
    public string SentAt { get; set; }
}