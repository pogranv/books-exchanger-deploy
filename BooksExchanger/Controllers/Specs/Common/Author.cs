namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель автора.
/// </summary>
public class Author
{
    /// <summary>
    /// id автора.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// ФИО автора.
    /// </summary>
    public string Name { get; set; }
}