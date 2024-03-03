using System.Net;
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Chats;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Implementations.ChatService;
using BooksExchanger.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер чатов.
/// </summary>
[ApiController]
[Route("api/v1/chats")]
public class ChatController : ControllerBase
{
    private IChatService _chatService;
    private IUserService _userService;
    private ResponseMapper _responseMapper;

    public ChatController(IChatService chatService, IUserService userService)
    {
        _chatService = chatService;
        _userService = userService;
        _responseMapper = new();
    }

    /// <summary>
    /// Отдает все чаты пользователя.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    [ProducesResponseType(typeof(GetChatsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [HttpGet]
    [Attributes.Auth.UserAuthorize]
    public IActionResult GetChats()
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        var chats = _chatService.GetChats(authUser.Id).ToList().ConvertAll(_responseMapper.MapChat);
        return Ok(new GetChatsResponse { Chats = chats });
    }

    /// <summary>
    /// Получения конкретного чата вместе с сообщениями.
    /// Если чата не существует, создается новый.
    /// </summary>
    /// <param name="userId">id собеседника</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="404">Получатель не найден.</response>
    [ProducesResponseType(typeof(GetChatResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [HttpGet("{userId:long:min(1)}")]
    [Attributes.Auth.UserAuthorize]
    public IActionResult GetChatWithMessages(long userId)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        if (authUser.Id == userId)
        {
            return BadRequest(new ErrorResponse { Message = "Невозможно создать чат" });
        }

        if (!_userService.IsUserExist(userId))
        {
            return NotFound(new ErrorResponse { Message = $"Пользователя с id={userId} не существует" });
        }
        var chat = _chatService.GetChatWithMessages(authUser.Id, userId);

        var response = new GetChatResponse
        {
            ChatId = chat.Id,
            UserId = userId,
            UserName = chat.UserName,
            Messages = chat.Messages.ConvertAll(message => _responseMapper.MapMessage(message, authUser.Id))
        };
        return Ok(response);
    }
}
