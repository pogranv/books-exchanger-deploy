using BooksExchanger.Context;
using BooksExchanger.Models;
using BooksExchanger.Repositories.Exeptions;
using BooksExchanger.Repositories.Interfaces;
using User = BooksExchanger.Entities.User;
using UserRole = BooksExchanger.Entities.UserRole;

namespace BooksExchanger.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private ResponseMapper _responseMapper;

    public UserRepository()
    {
        _responseMapper = new ResponseMapper();
    }
    public Models.User? GetUserByEmail(string email)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.FirstOrDefault(user => user.Email == email);
            return _responseMapper.MapUser(user);
        }
    }

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

    public bool IsUserExist(long userId)
    {
        using (DbCtx db = new DbCtx())
        {
            var user = db.Users.FirstOrDefault(user => user.Id == userId);
            return user != null;
        }
    }
}