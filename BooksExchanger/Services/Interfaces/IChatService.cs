using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface IChatService
{
    public IEnumerable<Chat> GetChats(long userId);

    public ChatWithMessages GetChatWithMessages(long currentUserId, long secondUserId);
}