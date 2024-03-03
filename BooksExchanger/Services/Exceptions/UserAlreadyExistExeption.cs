namespace BooksExchanger.Services.Implementations.UserService.Exceptions;

public class UserAlreadyExistExeption : Exception
{
    public UserAlreadyExistExeption()
    {
    }

    public UserAlreadyExistExeption(string message) : base(message)
    {
    }

    public UserAlreadyExistExeption(string message, Exception inner) : base(message, inner)
    {
    }
}