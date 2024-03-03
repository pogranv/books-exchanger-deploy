namespace BooksExchanger.Models;

/// <summary>
/// Модель сообщения для отправки.
/// </summary>
public class SendMessage
{
    /// <summary>
    /// Id получателя.
    /// </summary>
    public long ToUserId { get; set; }
    
    /// <summary>
    /// Текс сообщения.
    /// </summary>
    public string Text { get; set; }
}