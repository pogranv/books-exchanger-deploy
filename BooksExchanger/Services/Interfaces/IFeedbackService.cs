namespace BooksExchanger.Services.Interfaces;

public interface IFeedbackService
{
    public long AddOrEditFeedback(Guid offerId, long authorId, string text = "", int? estimation = null);

    public IEnumerable<Models.Feedback> GetFeedbacks(Guid offerId);

    public void RemoveFeedback(long feedbackId, Func<Models.Feedback, bool> permissionsCheker);

    public Models.Feedback FilterOutUserFeedback(List<Models.Feedback> feedbacks, long userId);
}