namespace BooksExchanger.Models;

/// <summary>
/// Модель чата с сообщениями.
/// </summary>
public class ChatWithMessages
{
    /// <summary>
    /// Id чата.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Имя собеседника.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// id собеседника.
    /// </summary>
    
    public long UserId { get; set; }
    
    /// <summary>
    /// Сообщения.
    /// </summary>
    public List<Message> Messages { get; set; }
}