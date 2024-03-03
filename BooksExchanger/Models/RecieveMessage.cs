namespace BooksExchanger.Models;

/// <summary>
/// Модель сообщения для получения.
/// </summary>
public class RecieveMessage
{
    /// <summary>
    /// Id чата.
    /// </summary>
    public long ChatId { get; set; }
        
    /// <summary>
    /// Id сообщения.
    /// </summary>
    public long MessageId { get; set; }
    
    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Дата и время сообщения.
    /// </summary>
    public string Time { get; set; }
}