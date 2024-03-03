using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Models.Requests;

/// <summary>
/// Модель запроса создания жанра.
/// </summary>
public class AddGenreRequest
{
    /// <summary>
    /// Название жанра.
    /// </summary>
    [Required(ErrorMessage = "Не указано название жанра")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Максимальная длина 50 символов")]
    public string Name { get; set; }
}