using BooksExchanger.Controllers.Specs.Common;

namespace BooksExchanger.Controllers.Specs.Chats;

/// <summary>
/// Модель ответа получения чатов.
/// </summary>
public class GetChatsResponse
{
    /// <summary>
    /// Информация о чатах.
    /// </summary>
    public List<Chat> Chats { get; set; }
}