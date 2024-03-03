using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.Authors;

/// <summary>
/// Модель запроса для редактирования автора.
/// </summary>
public class UpdateAuthorRequest
{
    /// <summary>
    /// Id автора.
    /// </summary>
    [Required(ErrorMessage = "Не указан id автора")]
    public long Id { get; set; }
    
    /// <summary>
    /// Новое ФИО для автора.
    /// </summary>
    [Required(ErrorMessage = "Не указано имя автора")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Максимальная длина 50 символов")]
    public string NewName { get; set; }
}