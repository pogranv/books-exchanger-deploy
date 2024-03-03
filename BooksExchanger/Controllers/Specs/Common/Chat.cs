namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель чата.
/// </summary>
public class Chat
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
    /// Последнее отправленное сообщение.
    /// </summary>
    public string LastMessage { get; set; }
    
    /// <summary>
    /// id собеседника.
    /// </summary>
    
    public long UserId { get; set; }
}