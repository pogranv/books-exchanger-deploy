using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.User;
using BooksExchanger.Entities;
using BooksExchanger.Models;
using BooksExchanger.Models.Requests;
using BooksExchanger.Services.Implementations;
using BooksExchanger.Services.Implementations.UserService.Exceptions;
using BooksExchanger.Services.Interfaces;
using BooksExchanger.Settings;
using BooksExchanger.VerificationCodesManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Obshajka.VerificationCodesManager;
using Obshajka.VerificationCodesManager.Exceptions;

namespace BooksExchanger.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Контроллер пользователей.
/// </summary>
[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private static readonly IVerificationCodeService s_verificationCodeService;
    private IUserService _userService;

    static UserController()
    {
        var emailParams = new EmailParams("Books Exhcanger", "Books Exhcanger", "Здравствуйте! Ваш код подтверждения для приложения Books Exhcanger: ");
        s_verificationCodeService = new VerificationCodeService(5, emailParams);
    }

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Выдает токен авторизации.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="404">Неправильная почта или пароль.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [AllowAnonymous]
    [HttpPost("auth")]
    public IActionResult AuthorizeUser(AuthRequest request)
    {
        try
        {
            var token = _userService.GetAuthToken(request.Email, request.Password);
            return Ok(new AuthResponse { Token = token });
        }
        catch (Exception ex) when (ex is IncorrectPasswordException || ex is UserNotFoundException)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
    }
    
    /// <summary>
    /// Осуществляет отправку кода подтверждения.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="409">Пользователь с такой почтой уже зарегистрирован.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [AllowAnonymous]
    [HttpPut("register")]
    public IActionResult RegisterUser(RegisterUserRequest request)
    {
        if (_userService.IsUserRegistered(request.Email))
        {
            return Conflict(new { message = "Пользователь с такой почтой уже существует!" });
        }
        var user = new ShortUserInfo() { Email = request.Email, Name = request.Name, Password = request.Password};

        try
        {
            s_verificationCodeService.SendCodeAndRememberUser(user);
        }
        catch (UserAlreadyWaitConfirmationException ex)
        {
            return Conflict(new ErrorResponse() { Message = ex.Message });
        } 
        
        
        return Ok();
    }
    
    /// <summary>
    /// Осуществляет подтверждение почты пользователя и регистрирует его.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="403">Неверный код подтверждения.</response>
    /// <response code="404">Пользователь не запрашивал код на почту или прошло силком много времени.</response>
    /// <response code="409">Пользователь с такой почтой уже зарегистрирован.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [AllowAnonymous]
    [HttpPost("verify")]
    public IActionResult VerifyrUser(VerifyUserRequest request)
    {
        try
        {
            var user = s_verificationCodeService.VerifyUser(request.Email, request.Code);
            var token = _userService.RegisterUser(user.Name, user.Email, user.Password);
            return Ok(new AuthResponse { Token = token });
        }
        catch (InvalidVerificationCodeException ex)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new ErrorResponse{ Message = ex.Message }); 
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
        catch (UserAlreadyExistExeption ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message});
        }
    }
    
    /// <summary>
    /// Позволяет назначить пользователя админом.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Пользователь с такой почтой не существует.</response>
    /// <response code="409">Пользователь уже является админом.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [Attributes.Auth.AdminAuthorize]
    [HttpPost("set-admin")]
    public IActionResult SetAdmin(SetNewAdminRequest request)
    {
        using (DbCtx db = new DbCtx())
        {
            try
            {
                _userService.SetAdmin(request.Email);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            
            return Ok();
        }
    }
    
    [HttpGet("check")]
    [Attributes.Auth.CheckAuthorize]
    public IActionResult CheckToken()
    {
        return Ok();
    }
}