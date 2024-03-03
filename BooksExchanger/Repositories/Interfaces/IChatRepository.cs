using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

public interface IChatRepository
{
    public IEnumerable<Chat> GetChats(long userId, bool needOrderMessagesByDescending = true);

    public ChatWithMessages? GetChatWithMessages(long firstUserId, long secondUserId, bool needOrderMessagesByDescending = true);
    
    public ChatWithMessages CreateChatWithMessages(long firstUserId, long secondUserId);

    public long AddMessage(long senderId, long chatId, string text);
}