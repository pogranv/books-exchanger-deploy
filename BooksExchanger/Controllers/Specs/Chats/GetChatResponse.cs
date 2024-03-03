using BooksExchanger.Controllers.Specs.Common;

namespace BooksExchanger.Controllers.Specs.Chats;

/// <summary>
/// Модель ответа получения чата.
/// </summary>
public class GetChatResponse
{
    /// <summary>
    /// id чата.
    /// </summary>
    public long ChatId { get; set; }
    
    /// <summary>
    /// Имя собеседника
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Id собеседника
    /// </summary>
    
    public long UserId { get; set; }

    /// <summary>
    /// Сообщения.
    /// </summary>
    public List<Message> Messages { get; set; }
    
}