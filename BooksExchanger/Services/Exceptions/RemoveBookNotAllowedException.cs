namespace BooksExchanger.Services.Exceptions;

public class RemoveBookNotAllowedException : Exception
{
    public RemoveBookNotAllowedException()
    {
    }

    public RemoveBookNotAllowedException(string message) : base(message)
    {
    }

    public RemoveBookNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}