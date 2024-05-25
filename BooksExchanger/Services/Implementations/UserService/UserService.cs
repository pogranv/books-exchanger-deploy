using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using BooksExchanger.Models;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Implementations.UserService.Exceptions;
using BooksExchanger.Services.Interfaces;
using BooksExchanger.Settings;

namespace BooksExchanger.Services.Implementations.UserService;


/// <summary>
/// Сервис пользователей.
/// </summary>
public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private string _secretKeyForToken;

    /// <summary>
    /// Инициализирует новый экземпляр службы пользователей.
    /// </summary>
    /// <param name="settings">Настройки приложения.</param>
    /// <param name="repository">Репозиторий пользователей.</param>
    public UserService(IOptions<AppSettings> settings, IUserRepository repository)
    {
        _secretKeyForToken = settings.Value.Secret;
        _userRepository = repository;
    }

    /// <summary>
    /// Генерирует токен аутентификации для пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Строка, содержащая JWT-токен.</returns>
    public string GetAuthToken(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new UserNotFoundException("Пользователь с такой почтой не найден.");
        }
        
        string hashedRequestPassword = PasswordHasher.HashPassword(password, user.Salt);
        if (hashedRequestPassword == user.Password)
        {
            return GenerateJwtToken(user);
        }

        throw new IncorrectPasswordException("Неверный пароль.");
    }

    /// <summary>
    /// Регистрирует пользователя и генерирует токен аутентификации.
    /// </summary>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Строка, содержащая JWT-токен.</returns>
    public string RegisterUser(string name, string email, string password)
    {
        var salt = PasswordHasher.BuildSalt();
        string hashedRequestPassword = PasswordHasher.HashPassword(password, salt);
        try
        {
            var user = _userRepository.TryAddNewUser(name, email, hashedRequestPassword);
            return GenerateJwtToken(user);
        }
        catch (Repositories.Exeptions.UserAlreadyExistException ex)
        {
            throw new UserAlreadyExistExeption(ex.Message);
        }
    }

    /// <summary>
    /// Проверяет, зарегистрирован ли пользователь.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <returns>true, если пользователь зарегистрирован; иначе false.</returns>
    public bool IsUserRegistered(string email)
    {
        var user = _userRepository.GetUserByEmail(email);
        return user != null;
    }

    /// <summary>
    /// Проверяет, существует ли пользователь.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>true, если пользователь существует; иначе false.</returns>
    public bool IsUserExist(long userId)
    {
        return _userRepository.IsUserExist(userId);
    }

    /// <summary>
    /// Назначает пользователю роль администратора.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    public void SetAdmin(string email)
    {
        try
        {
            _userRepository.SetAdminByEmail(email);
        }
        catch (Repositories.Exeptions.UserNotFoundException ex)
        {
            throw new UserNotFoundException(ex.Message);
        }
    }

    /// <summary>
    /// Генерирует JWT-токен для пользователя.
    /// </summary>
    /// <param name="user">Данные пользователя, для которого генерируется токен.</param>
    /// <returns>Строка, содержащая JWT-токен.</returns>
    private string GenerateJwtToken(Models.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKeyForToken);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role.ToString())
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}