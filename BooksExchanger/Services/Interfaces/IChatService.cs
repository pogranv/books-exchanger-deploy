using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Определяет интерфейс для службы чата, позволяющей получать доступ к чатам и сообщениям.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Получает все чаты, ассоциированные с пользователем.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, чьи чаты необходимо получить.</param>
    /// <returns>Коллекция чатов указанного пользователя.</returns>
    public IEnumerable<Chat> GetChats(long userId);

    /// <summary>
    /// Получает чат между двумя пользователями вместе со всеми сообщениями.
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя.</param>
    /// <param name="secondUserId">Идентификатор второго пользователя, с которым ведется диалог.</param>
    /// <returns>Объект чата, содержащий информацию о чате и ассоциированные с ним сообщения.</returns>
    public ChatWithMessages GetChatWithMessages(long currentUserId, long secondUserId);
}