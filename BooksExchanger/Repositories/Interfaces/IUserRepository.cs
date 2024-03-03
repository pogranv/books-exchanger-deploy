using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

public interface IUserRepository
{
    public User? GetUserByEmail(string email);

    public User TryAddNewUser(string name, string email, string password);

    public void SetAdminByEmail(string email);

    public bool IsUserExist(long userId);
}