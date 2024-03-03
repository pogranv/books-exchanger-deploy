using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.OffersCollector;

/// <summary>
/// Модель запроса на отклонение оффера.
/// </summary>
public class RejectOfferRequest
{
    /// <summary>
    /// Id оффера.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Причина отказа.
    /// </summary>
    [Required(ErrorMessage = "Не указана причина отказа")]
    public string RejectReason { get; set; }
}