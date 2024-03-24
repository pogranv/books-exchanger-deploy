using BooksExchanger.Context;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using User = BooksExchanger.Entities.User;
using UserRole = BooksExchanger.Entities.UserRole;

namespace BooksExchanger.Repositories.Implementations;

/// <summary>
/// Реализация репозитория пользователей.
/// </summary>
public class UserRepository : IUserRepository
{
    private ResponseMapper _responseMapper;

    /// <summary>
    /// Создает экземпляр UserRepository.
    /// </summary>
    public UserRepository()
    {
        _responseMapper = new ResponseMapper();
    }
    
    /// <summary>
    /// Получает пользователя по его электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <returns>Модель <see cref="Models.User"/>, если пользователь с такой почтой существует, иначе null.</returns>
    public Models.User? GetUserByEmail(string email)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.FirstOrDefault(user => user.Email == email);
            return _responseMapper.MapUser(user);
        }
    }

    /// <summary>
    /// Пытается добавить нового пользователя в систему.
    /// </summary>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Модель <see cref="Models.User"/> добавленного пользователя.</returns>
    /// <exception cref="UserAlreadyExistException">Бросается, если пользователь с такой почтой уже существует.</exception>
    public Models.User TryAddNewUser(string name, string email, string password)
    {
        using (DbCtx db = new DbCtx())
        {
            // TODO: не проверять, а ловить исключения от базы
            if (db.Users.FirstOrDefault(user => user.Email == email) != null)
            {
                throw new UserAlreadyExistException("Пользователь с такой почтой уже существует");
            }
            var user = new User { Name = name, Email = email, Password = password };
            db.Users.Add(user);
            db.SaveChanges();
            return _responseMapper.MapUser(user);
        }
    }

    /// <summary>
    /// Назначает роль администратора пользователю по его электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <exception cref="UserNotFoundException">Бросается, если пользователь с такой почтой не найден.</exception>
    public void SetAdminByEmail(string email)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.FirstOrDefault(user => user.Email == email);
            if (user == null)
            {
                throw new UserNotFoundException($"Пользователя с почтой {email} не найдено");
            }
            user.Role = UserRole.Admin;
            db.SaveChanges();
        }
    }

    /// <summary>
    /// Проверяет, существует ли пользователь по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>true, если пользователь существует; иначе false.</returns>
    public bool IsUserExist(long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.FirstOrDefault(user => user.Id == userId);
            return user != null;
        }
    }
}