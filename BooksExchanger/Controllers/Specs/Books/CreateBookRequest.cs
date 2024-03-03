using System.ComponentModel.DataAnnotations;

namespace BooksExchanger.Controllers.Specs.Books;

/// <summary>
/// Модель запроса для создания книги.
/// </summary>
public class CreateBookRequest
{
    /// <summary>
    /// Название книги.
    /// </summary>
    [Required(ErrorMessage = "Не указано название книги")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Максимальная длина 50 символов, минимальная - 3")]
    public string Title { get; set; }
    
    /// <summary>
    /// id жанра.
    /// </summary>
    [Required(ErrorMessage = "Не указан id жанра")]
    public int? GenreId { get; set; }
    
    /// <summary>
    /// id авторов книг.
    /// </summary>
    [Required(ErrorMessage = "Не указаны авторы")]
    [MinLength(1, ErrorMessage = "Укажите хотя бы одного автора")]
    public List<long> AuthorIds { get; set; }
}