using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.Genres;

/// <summary>
/// Модель запроса на редактирование жанра.
/// </summary>
public class UpdateGenresRequest
{
    /// <summary>
    /// id жанра
    /// </summary>
    [Required(ErrorMessage = "Не указан id жанра")]
    public int? Id { get; set; }
    
    /// <summary>
    /// Новое название жанра.
    /// </summary>
    [Required(ErrorMessage = "Не указано новое название жанра")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Максимальная длина 50 символов, минимальная - 3")]
    public string Name { get; set; }

}