namespace BooksExchanger.Models;

/// <summary>
/// Модель рейтинга книги.
/// </summary>
public class BookEstimation
{
    /// <summary>
    /// Количество оценок.
    /// </summary>
    public long CountRating { get; set; }
    
    /// <summary>
    /// Сумма оценок.
    /// </summary>
    public long SumRating { get; set; }
}