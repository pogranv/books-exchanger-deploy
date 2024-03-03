using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

public interface IFeedbackRepository
{
    long UpdateFeedback(long authorId, Guid offerId, string text, int? estimation, BookEstimation bookEstimation);

    public long InsertFeedback(long authorId, Guid offerId, string text, int? estimation);

    Models.Feedback? GetFeedback(Guid offerId, long authorId);
    
    Models.Feedback? GetFeedback(long feedbackId);

    bool RemoveFeedback(long feedbackId);

    IEnumerable<Models.Feedback> GetFeedbacks(Guid offerId);
}