using BooksExchanger.Controllers.Specs.Common;
using BooksExchanger.Entities;
using Author = BooksExchanger.Controllers.Specs.Common.Author;
using Feedback = BooksExchanger.Controllers.Specs.Common.Feedback;
using Genre = BooksExchanger.Controllers.Specs.Common.Genre;
using Offer = BooksExchanger.Controllers.Specs.Common.Offer;

namespace BooksExchanger.Controllers.Specs.Offers;

/// <summary>
/// Модель ответа получения оффера.
/// </summary>
public class GetOfferResponse
{
    /// <summary>
    /// Информация об оффере.
    /// </summary>
    public Offer Offer { get; set; }
}