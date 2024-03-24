using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с чатами.
/// </summary>
public interface IChatRepository
{
    /// <summary>
    /// Получает список чатов для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, для которого необходимо получить списки чатов.</param>
    /// <param name="needOrderMessagesByDescending">Если true, сообщения в чатах упорядочиваются по убыванию; в противном случае, по возрастанию.</param>
    /// <returns>Коллекция чатов пользователя.</returns>
    public IEnumerable<Chat> GetChats(long userId, bool needOrderMessagesByDescending = true);

    /// <summary>
    /// Получает чат между двумя пользователями с историей сообщений.
    /// </summary>
    /// <param name="firstUserId">Идентификатор первого пользователя.</param>
    /// <param name="secondUserId">Идентификатор второго пользователя.</param>
    /// <param name="needOrderMessagesByDescending">Если true, сообщения упорядочиваются по убыванию; в противном случае, по возрастанию.</param>
    /// <returns>Чат между пользователями с историей сообщений или null, если чат не найден.</returns>
    public ChatWithMessages? GetChatWithMessages(long firstUserId, long secondUserId, bool needOrderMessagesByDescending = true);
    
    /// <summary>
    /// Создает чат между двумя пользователями.
    /// </summary>
    /// <param name="firstUserId">Идентификатор первого пользователя.</param>
    /// <param name="secondUserId">Идентификатор второго пользователя.</param>
    /// <returns>Созданный чат с сообщениями.</returns>
    public ChatWithMessages CreateChatWithMessages(long firstUserId, long secondUserId);

    /// <summary>
    /// Добавляет сообщение в чат.
    /// </summary>
    /// <param name="senderId">Идентификатор отправителя сообщения.</param>
    /// <param name="chatId">Идентификатор чата, в который добавляется сообщение.</param>
    /// <param name="text">Текст сообщения.</param>
    /// <returns>Идентификатор созданного сообщения.</returns>
    public long AddMessage(long senderId, long chatId, string text);
}