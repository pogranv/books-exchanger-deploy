using BooksExchanger.Models;

namespace BooksExchanger.Services.Interfaces;

public interface ISearchService
{
    IEnumerable<Offer> SearchOffersByTitle(string title);
    
    IEnumerable<Offer> SearchOffersByAuthor(string author);
}