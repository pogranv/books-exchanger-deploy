namespace BooksExchanger.Models;

/// <summary>
/// Модель книжной подборки.
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
