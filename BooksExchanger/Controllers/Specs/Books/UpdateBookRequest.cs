using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.Books;

/// <summary>
/// Модель запроса для редактирования книги.
/// </summary>
public class UpdateBookRequest
{
    /// <summary>
    /// Id книги.
    /// </summary>
    [Required(ErrorMessage = "Не указан id книги")]
    public long? BookId { get; set; }
    
    /// <summary>
    /// Название книги.
    /// </summary>
    [Required(ErrorMessage = "Не указано название книги")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Максимальная длина 50 символов, минимальная - 3")]
    public string? Title { get; set; }
    
    /// <summary>
    /// Id жанра.
    /// </summary>
    [Required(ErrorMessage = "Не указан id жанра")]
    public int? GenreId { get; set; }
    
    /// <summary>
    /// id авторов.
    /// </summary>
    [Required(ErrorMessage = "Не указаны авторы")]
    [MinLength(1, ErrorMessage = "Укажите хотя бы одного автора")]
    public List<long>? AuthorIds { get; set; }
}