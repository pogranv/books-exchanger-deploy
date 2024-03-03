namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель оффера.
/// </summary>
public class Offer
{
    /// <summary>
    /// id оффера.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание оффера.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Цена.
    /// </summary>
    public decimal? Price { get; set; }
    
    /// <summary>
    /// Город.
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// Загрузочная ссылка на картинку.
    /// </summary>
    public string? Picture { get; set; }
    
    /// <summary>
    /// Информация о жанре.
    /// </summary>
    public Genre Genre { get; set; }
    
    /// <summary>
    /// Список авторов.
    /// </summary>
    public List<Author> Authors { get; set; }

    /// <summary>
    /// Информация о владельце.
    /// </summary>
    public Owner Owner { get; set; }
    
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public string CreatedAt { get; set; }
    
    /// <summary>
    /// Рейтинг книги.
    /// </summary>
    
    public decimal? BookRating { get; set; }
    
    /// <summary>
    /// Находится ли оффер в избранном пользователя (чтобы отобрадажать это).
    /// </summary>
    public bool IsFavoriteForUser { get; set; }
}