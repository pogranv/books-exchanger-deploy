using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.Authors;

/// <summary>
/// Запрос на создание автора.
/// </summary>
public class CreateAuthorRequest
{
    /// <summary>
    /// ФИО автора
    /// </summary>
    [Required(ErrorMessage = "Не указано имя автора")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Максимальная длина 50 символов")]
    public string Name { get; set; }
}