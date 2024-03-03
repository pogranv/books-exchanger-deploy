using BooksExchanger.Models;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Services.Implementations.ChatService;

public class ChatService : IChatService
{
    private IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }
    
    public IEnumerable<Chat> GetChats(long userId)
    {
        return _chatRepository.GetChats(userId);
    }

    public ChatWithMessages GetChatWithMessages(long currentUserId, long secondUserId)
    {
        var chat = _chatRepository.GetChatWithMessages(currentUserId, secondUserId);
        if (chat == null)
        {
            chat = _chatRepository.CreateChatWithMessages(currentUserId, secondUserId);
        }

        return chat;
        // public void CreateChatWithMessages(long firstUserId, long secondUserId);
    }
}