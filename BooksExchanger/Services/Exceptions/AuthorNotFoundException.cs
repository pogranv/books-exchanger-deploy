namespace BooksExchanger.Services.Exceptions;

public class AuthorNotFoundException : Exception
{
    public AuthorNotFoundException()
    {
    }

    public AuthorNotFoundException(string message) : base(message)
    {
    }

    public AuthorNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}