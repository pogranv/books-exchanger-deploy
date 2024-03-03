namespace BooksExchanger.Services.Exceptions;

public class OfferAlreadyFavoriteException : Exception
{
    public OfferAlreadyFavoriteException()
    {
    }

    public OfferAlreadyFavoriteException(string message) : base(message)
    {
    }

    public OfferAlreadyFavoriteException(string message, Exception inner) : base(message, inner)
    {
    }
}