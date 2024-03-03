using BooksExchanger.Controllers.Specs.Common;

namespace BooksExchanger.Controllers.Specs.Offers;

/// <summary>
/// Модель ответа получения офферов.
/// </summary>
public class GetOffersResponse
{
    /// <summary>
    /// Информация об офферах.
    /// </summary>
    public List<Offer> Offers { get; set; }
}