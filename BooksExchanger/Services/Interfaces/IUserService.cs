namespace BooksExchanger.Services.Interfaces;

public interface IUserService
{
    public string GetAuthToken(string email, string password);

    public string RegisterUser(string name, string email, string password);

    public bool IsUserRegistered(string email);
    
    public bool IsUserExist(long userId);

    public void SetAdmin(string email);
}