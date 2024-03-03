namespace BooksExchanger.Services.Exceptions;

public class OfferNotFoundException : Exception
{
    public OfferNotFoundException()
    {
    }

    public OfferNotFoundException(string message) : base(message)
    {
    }

    public OfferNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}