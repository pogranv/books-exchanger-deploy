using BooksExchanger.Context;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksExchanger.Repositories.Implementations;

/// <summary>
/// Хранилище чатов.
/// </summary>
public class ChatRepository : IChatRepository
{
    private ResponseMapper _responseMapper;

    public ChatRepository()
    {
        _responseMapper = new();
    }
    
    /// <summary>
    /// Получение чатов.
    /// </summary>
    /// <param name="userId">Юзер.</param>
    /// <param name="needOrderMessagesByDescending">Нужно ли сортировать сообщения.</param>
    /// <returns>Чаты.</returns>
    public IEnumerable<Chat> GetChats(long userId, bool needOrderMessagesByDescending = true)
    {
        using (DbCtx db = new DbCtx())
        {
            var chats = db.Chats
                .Include(chat => chat.User1)
                .Include(chat => chat.User2)
                .Include(chat => chat.Messages)
                .Where(chat => chat.User1.Id == userId || chat.User2.Id == userId)
                .ToList();
            if (needOrderMessagesByDescending)
            {
                foreach (var chat in chats)
                {
                    chat.Messages.OrderByDescending(message => message.SentAt);
                }
            }
            return chats.ConvertAll(chat => _responseMapper.MapChat(chat, userId));
        }
    }

    /// <summary>
    /// Добавляет сообщение.
    /// </summary>
    /// <param name="senderId">Отправитель.</param>
    /// <param name="chatId">Чат.</param>
    /// <param name="text">Сообщение.</param>
    /// <returns>id сообщения.</returns>
    public long AddMessage(long senderId, long chatId, string text)
    {
        using (DbCtx db = new DbCtx())
        {
            var chat = db.Chats
                .Include(chat => chat.User1)
                .Include(chat => chat.User2)
                .FirstOrDefault(chat => chat.ChatId == chatId);

            var message = new Entities.Message
            {
                UserId = senderId,
                Text = text
            };
            chat.Messages.Add(message);
            db.SaveChanges();
            return message.MessageId;
        }
    }

    /// <summary>
    /// Получение чата с сообщениями.
    /// </summary>
    /// <param name="currentUserId">Текущий пользователь.</param>
    /// <param name="secondUserId">Собеседник.</param>
    /// <param name="needOrderMessagesByDescending">Нужно ли сортировать сообщения.</param>
    /// <returns>Чат с ссобщениями.</returns>
    public ChatWithMessages? GetChatWithMessages(long currentUserId, long secondUserId, bool needOrderMessagesByDescending = true)
    {
        long firstUserId = currentUserId;
        (firstUserId, secondUserId) = (Int64.Min(firstUserId, secondUserId), Int64.Max(firstUserId, secondUserId));
        using (DbCtx db = new DbCtx())
        {
            var chat = db.Chats
                .Include(chat => chat.User1)
                .Include(chat => chat.User2)
                .Include(chat => chat.Messages)
                .FirstOrDefault(chat => (chat.User1.Id == firstUserId && chat.User2.Id == secondUserId));
            if (chat == null)
            {
                return null;
            }
            if (needOrderMessagesByDescending)
            {
                chat.Messages.OrderByDescending(message => message.SentAt);
            }

            return _responseMapper.MapChatWithMessages(chat, currentUserId);
        }
    }

    /// <summary>
    /// Создать чат с сообщениями.
    /// </summary>
    /// <param name="currentUserId">Текущий пользователь.</param>
    /// <param name="secondUserId">Собеседник.</param>
    /// <returns>Чат с сообщениями.</returns>
    public ChatWithMessages CreateChatWithMessages(long currentUserId, long secondUserId)
    {
        long firstUserId = currentUserId;
        (firstUserId, secondUserId) = (Int64.Min(firstUserId, secondUserId), Int64.Max(firstUserId, secondUserId));
        using (DbCtx db = new DbCtx())
        {
            var chat = new Entities.Chat
            {
                User1Id = firstUserId,
                User2Id = secondUserId,
            };
            db.Chats.Add(chat);
            db.SaveChanges();
            var createdChat = db.Chats
                .Include(chat => chat.User1)
                .Include(chat => chat.User2)
                .FirstOrDefault(chat => chat.User1Id == firstUserId && chat.User2Id == secondUserId);
            return _responseMapper.MapChatWithMessages(createdChat, currentUserId);
        }
    }
}