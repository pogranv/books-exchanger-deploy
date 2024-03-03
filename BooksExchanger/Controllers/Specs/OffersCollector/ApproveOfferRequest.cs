using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.OffersCollector;

/// <summary>
/// Модель запроса подтверждения оффера.
/// </summary>
public class ApproveOfferRequest
{
    /// <summary>
    /// Id оффера.
    /// </summary>
    [Required(ErrorMessage = "Не указан id оффера")]
    public Guid OfferId { get; set; }
    
    
    /// <summary>
    /// Привязанная к офферу книга.
    /// </summary>
    [Required(ErrorMessage = "Не указан id книги")]
    public long LinkedBookId { get; set; }
}