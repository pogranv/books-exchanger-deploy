using System.Collections.Immutable;
using System.Security.Principal;
using BooksExchanger.Controllers.Specs.Common;
using BooksExchanger.Entities;

namespace BooksExchanger.Controllers.Specs.OffersCollector;

/// <summary>
/// Модель ответа получения офферов, находящихся на модерации.
/// </summary>
public class GetOffersCollectorResponse
{
    /// <summary>
    /// Информация об офферах.
    /// </summary>
    public List<OfferCollector> Offers { get; set; }
}