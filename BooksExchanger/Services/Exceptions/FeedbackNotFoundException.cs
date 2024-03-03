namespace BooksExchanger.Services.Exceptions;

public class FeedbackNotFoundException : Exception
{
    public FeedbackNotFoundException()
    {
    }

    public FeedbackNotFoundException(string message) : base(message)
    {
    }

    public FeedbackNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}