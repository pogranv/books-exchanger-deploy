namespace BooksExchanger.Services.Exceptions;

public class OfferAlreadyApprovedException : Exception
{
    public OfferAlreadyApprovedException()
    {
    }

    public OfferAlreadyApprovedException(string message) : base(message)
    {
    }

    public OfferAlreadyApprovedException(string message, Exception inner) : base(message, inner)
    {
    }
}