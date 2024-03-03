namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель оффера на модерации.
/// </summary>
public class OfferCollector
{
    /// <summary>
    /// Id оффера.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание оффера.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Информация о владельце.
    /// </summary>
    public Owner Owner { get; set; }
    
    /// <summary>
    /// Информация об авторах.
    /// </summary>
    public string Authors { get; set; }
    
    /// <summary>
    /// Цена.
    /// </summary>
    public decimal? Price { get; set; }
    
    /// <summary>
    /// Статус модерации
    /// </summary>
    public ModerationStatus ModerationStatus { get; set; }
    
    /// <summary>
    /// Город публикации.
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// Ссылка на картинку.
    /// </summary>
    public string? Picture { get; set; }
    
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public string CreatedAt { get; set; }
    
    /// <summary>
    /// Причина отказа (при наличии).
    /// </summary>
    public string? RegectReason { get; set; }
}