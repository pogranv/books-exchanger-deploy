using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.OffersCollector;

/// <summary>
/// Модель запроса на создание оффера.
/// </summary>
public class CreateOfferRequest
{
    /// <summary>
    /// Название книги.
    /// </summary>
    [Required(ErrorMessage = "Не указано название книги")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Максимальная длина 100 символов, минимальная - 3")]
    public string Title { get; set; }
    
    /// <summary>
    /// Описание оффера.
    /// </summary>
    [StringLength(500, MinimumLength = 3, ErrorMessage = "Максимальная длина 500 символов, минимальная - 3")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Авторы книги.
    /// </summary>
    [Required(ErrorMessage = "Не указаны авторы книги")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Максимальная длина 200 символов, минимальная - 3")]
    public string Authors { get; set; }
    
    
    /// <summary>
    /// Цена.
    /// </summary>
    [Range(1, 1000000, ErrorMessage = "Цена может быть от 1 до 1000000 рублей")]
    public int? Price { get; set; }
    
    
    /// <summary>
    /// Город, в котором выкладывается объявление.
    /// </summary>
    [Required(ErrorMessage = "Не указан город")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Максимальная длина 200 символов, минимальная - 2")]
    public string City { get; set; }
    
    /// <summary>
    /// Фотография книги.
    /// </summary>
    public IFormFile? Image { get; set; }
}