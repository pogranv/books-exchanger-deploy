namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Подель книжной подборки.
/// </summary>
public class Selection
{
    /// <summary>
    /// Название подборки.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Офферы.
    /// </summary>
    public List<Offer> Offers { get; set; }
}