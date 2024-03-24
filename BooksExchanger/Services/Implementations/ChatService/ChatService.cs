using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.ChatService;

/// <summary>
/// Сервис для работы с чатами.
/// </summary>
public class ChatService : IChatService
{
    private IChatRepository _chatRepository;

    /// <summary>
    /// Конструктор сервиса чатов.
    /// </summary>
    /// <param name="chatRepository">Репозиторий чатов.</param>
    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    /// <summary>
    /// Получает список всех чатов для указанного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, чьи чаты необходимо получить.</param>
    /// <returns>Коллекция чатов пользователя.</returns>
    public IEnumerable<Chat> GetChats(long userId)
    {
        return _chatRepository.GetChats(userId);
    }

    /// <summary>
    /// Получает детали чата вместе с сообщениями между текущим пользователем и другим указанным пользователем.
    /// Создает новый чат, если таковой не существует.
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя.</param>
    /// <param name="secondUserId">Идентификатор другого пользователя, с которым идет обмен сообщениями.</param>
    /// <returns>Детали чата с сообщениями.</returns>
    public ChatWithMessages GetChatWithMessages(long currentUserId, long secondUserId)
    {
        var chat = _chatRepository.GetChatWithMessages(currentUserId, secondUserId);
        if (chat == null)
        {
            chat = _chatRepository.CreateChatWithMessages(currentUserId, secondUserId);
        }

        return chat;
    }
}