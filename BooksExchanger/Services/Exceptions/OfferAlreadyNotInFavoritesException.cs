namespace BooksExchanger.Services.Exceptions;

public class OfferAlreadyNotInFavoritesException : Exception
{
    public OfferAlreadyNotInFavoritesException()
    {
    }

    public OfferAlreadyNotInFavoritesException(string message) : base(message)
    {
    }

    public OfferAlreadyNotInFavoritesException(string message, Exception inner) : base(message, inner)
    {
    }
}