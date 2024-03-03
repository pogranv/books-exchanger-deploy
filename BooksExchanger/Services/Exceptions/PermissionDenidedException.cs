namespace BooksExchanger.Services.Exceptions;

public class PermissionDenidedException : Exception
{
    public PermissionDenidedException()
    {
    }

    public PermissionDenidedException(string message) : base(message)
    {
    }

    public PermissionDenidedException(string message, Exception inner) : base(message, inner)
    {
    }
}