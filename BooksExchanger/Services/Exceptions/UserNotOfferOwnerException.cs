namespace BooksExchanger.Services.Exceptions;

public class UserNotOfferOwnerException : Exception
{
    public UserNotOfferOwnerException()
    {
    }

    public UserNotOfferOwnerException(string message) : base(message)
    {
    }

    public UserNotOfferOwnerException(string message, Exception inner) : base(message, inner)
    {
    }
}
