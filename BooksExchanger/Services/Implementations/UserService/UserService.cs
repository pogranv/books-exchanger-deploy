using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Models;
using BooksExchanger.Models.Requests;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Implementations.UserService.Exceptions;
using BooksExchanger.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Settings;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Options;

namespace BooksExchanger.Services.Implementations.UserService;


public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private string _secretKeyForToken;

    public UserService(IOptions<AppSettings> settings, IUserRepository repository)
    {
        _secretKeyForToken = settings.Value.Secret;
        _userRepository = repository;
    }

    public string GetAuthToken(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new UserNotFoundException("Пользователь с такой почтой не найден.");
        }
        
        string hashedRequestPassword = PasswordHasher.HashPassword(password);
        if (hashedRequestPassword == user.Password)
        {
            return GenerateJwtToken(user);
        }

        throw new IncorrectPasswordException("Неверный пароль.");
    }

    public string RegisterUser(string name, string email, string password)
    {
        string hashedRequestPassword = PasswordHasher.HashPassword(password);
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

    public bool IsUserRegistered(string email)
    {
        var user = _userRepository.GetUserByEmail(email);
        return user != null;
    }

    public bool IsUserExist(long userId)
    {
        return _userRepository.IsUserExist(userId);
    }

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