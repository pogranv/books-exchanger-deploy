using BooksExchanger.Context;
using BooksExchanger.Entities;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Implementations.UserService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BooksExchanger.MetanitHub;

public class CustomUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        var user = (AuthUser)connection.GetHttpContext().Items["User"];
        return user.Id.ToString();
        // или так
        //return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}

public class ChatHub : Hub
{
    private IUserRepository _userRepository;
    private IChatRepository _chatRepository;

    public ChatHub(IUserRepository userRepository, IChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }
    
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

// using BooksExchanger.Context;
// using BooksExchanger.Entities;
// using BooksExchanger.Models;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.MetanitHub;
//
// public class CustomUserIdProvider : IUserIdProvider
// {
//     public virtual string GetUserId(HubConnectionContext connection)
//     {
//         var user = (AuthUser)connection.GetHttpContext().Items["User"];
//         return user.Id.ToString();
//         // или так
//         //return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
//     }
// }
//
// public class ChatHub : Hub
// {
//     public override Task OnConnectedAsync()
//     {
//         var lol = (AuthUser)Context.GetHttpContext().Items["User"];
//         if (lol == null)
//         {
//             // non auth user
//             Context.Abort();
//         }
//         // попробовать поменять название метода
//         // Clients.All.SendAsync("Send", $"Приветствуем {Context.UserIdentifier}");
//         return base.OnConnectedAsync();
//     }
//
//     public class SendMessage
//     {
//         public long ToUserId { get; set; }
//         public string Text { get; set; }
//     }
//
//     public class RecieveMessage
//     {
//         public long ChatId { get; set; }
//         
//         public long MessageId { get; set; }
//         public string Text { get; set; }
//         public string Time { get; set; }
//     }
//
//     
//     private Tuple<long, long> AddMessageToDbAndGetId(long user1, long user2, string text) // (chatId, messageId)
//     {
//         var messageSenderId = user1;
//         if (user2 < user1)
//         {
//             (user1, user2) = (user2, user1);
//         }
//         using (DbCtx db = new DbCtx())
//         {
//             var chat = db.Chats
//                 .Include(chat => chat.User1)
//                 .Include(chat => chat.User2)
//                 .FirstOrDefault(
//                 chat => chat.User1.Id == user1 && chat.User2.Id == user2
//             );
//             if (chat == null)
//             {
//                 chat = new Chat
//                 {
//                     User1Id = user1,
//                     User2Id = user2,
//                 };
//                 db.Chats.Add(chat);
//             }
//
//             var message = new Message
//             {
//                 UserId = messageSenderId,
//                 Text = text
//             };
//             chat.Messages.Add(message);
//             // try
//             // {
//                 db.SaveChanges();
//             // }
//             // catch (Exception ex)
//             // {
//             //     string kek = ex.ToString();
//             //     Console.WriteLine(ex);
//             // }
//             return new (chat.ChatId, message.MessageId);
//         }
//     }
//
//     private bool IsUserExist(long userId)
//     {
//         using (DbCtx db = new DbCtx())
//         {
//             var user = db.Users.FirstOrDefault(user => user.Id == userId);
//             return user != null;
//         }
//     }
//     
//     public async Task Send(SendMessage message)
//     {
//         var authUser = (AuthUser)Context.GetHttpContext().Items["User"];
//         if (!IsUserExist(message.ToUserId) || string.IsNullOrEmpty(message.Text))
//         {
//             return;
//         }
//
//         var chatIdMessageId = AddMessageToDbAndGetId(authUser.Id, message.ToUserId, message.Text);
//         var newMessage = new RecieveMessage
//         {
//             ChatId = chatIdMessageId.Item1,
//             MessageId = chatIdMessageId.Item2,
//             Text = message.Text,
//             Time = DateTime.Now.ToString()
//         };
//
//        await Clients.User(message.ToUserId.ToString()).SendAsync("Recieve", newMessage);
//     }
// }