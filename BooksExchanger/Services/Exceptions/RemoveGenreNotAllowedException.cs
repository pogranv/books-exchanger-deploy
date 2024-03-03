namespace BooksExchanger.Services.Exceptions;

public class RemoveGenreNotAllowedException : Exception
{
    public RemoveGenreNotAllowedException()
    {
    }

    public RemoveGenreNotAllowedException(string message) : base(message)
    {
    }

    public RemoveGenreNotAllowedException(string message, Exception inner) : base(message, inner)
    {
    }
}