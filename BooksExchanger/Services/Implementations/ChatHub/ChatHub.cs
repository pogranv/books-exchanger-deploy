using Microsoft.AspNetCore.SignalR;

using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;

namespace BooksExchanger.MetanitHub;

/// <summary>
/// Класс пользователя.
/// </summary>
public class CustomUserIdProvider : IUserIdProvider
{
    /// <summary>
    /// Получает id пользователя.
    /// </summary>
    /// <param name="connection">Подкючение.</param>
    /// <returns>Id пользователя.</returns>
    public virtual string GetUserId(HubConnectionContext connection)
    {
        var user = (AuthUser)connection.GetHttpContext().Items["User"];
        return user.Id.ToString();
        // или так
        //return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}

/// <summary>
/// Хаб чата.
/// </summary>
public class ChatHub : Hub
{
    private IUserRepository _userRepository;
    private IChatRepository _chatRepository;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="userRepository">Хранилище пользователей.</param>
    /// <param name="chatRepository">Хранилище чатов.</param>
    public ChatHub(IUserRepository userRepository, IChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }
    
    /// <summary>
    /// Обработчик подключения нового клиента.
    /// </summary>
    public override Task OnConnectedAsync()
    {
        var user = (AuthUser)Context.GetHttpContext().Items["User"];
        if (user == null)
        {
            // non auth user
            Context.Abort();
        }
        return base.OnConnectedAsync();
    }
    
    /// <summary>
    /// Отправляет сообщение.
    /// </summary>
    public async Task Send(SendMessage message)
    {
        var authUser = (AuthUser)Context.GetHttpContext().Items["User"];
        
        if (!_userRepository.IsUserExist(message.ToUserId) || string.IsNullOrEmpty(message.Text))
        {
            return;
        }
        
        var chat = _chatRepository.GetChatWithMessages(authUser.Id, message.ToUserId);
        if (chat == null)
        {
            chat = _chatRepository.CreateChatWithMessages(authUser.Id, message.ToUserId);
        }
        var messageId = _chatRepository.AddMessage(authUser.Id, chat.Id, message.Text);
        var recieveMessage = new RecieveMessage
        {
            ChatId = chat.Id,
            MessageId = messageId,
            Text = message.Text,
            Time = DateTime.Now.ToString()
        };

       await Clients.User(message.ToUserId.ToString()).SendAsync("Recieve", recieveMessage);
    }
}