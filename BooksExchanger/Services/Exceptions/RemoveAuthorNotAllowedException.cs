namespace BooksExchanger.Services.Exceptions;

public class RemoveAuthorNotAllowedException : Exception
{
    public RemoveAuthorNotAllowedException()
    {
    }

    public RemoveAuthorNotAllowedException(string message) : base(message)
    {
    }

    public RemoveAuthorNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}