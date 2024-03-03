using System.Net;

using Microsoft.AspNetCore.Mvc;

using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Books;
using BooksExchanger.Models;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;


namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер книг.
/// </summary>
[ApiController]
[Route("admin/v1/books")]
public class BooksController : ControllerBase
{
    private IBookService _bookService;
    private ResponseMapper _responseMapper;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
        _responseMapper = new();
    }
    
    /// <summary>
    /// Создает новую книгу.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="409">Не существует указанных авторов или жанра.</response>
    /// <response code="500">Неизвестная ошибка.</response>
    [ProducesResponseType(typeof(CreateBookResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpPost("create")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult Create(CreateBookRequest request)
    {
        try
        {
            var bookId = _bookService.AddBook(request.Title, request.GenreId.Value, request.AuthorIds);
            return Ok(new CreateBookResponse{ Id = bookId});
        }
        catch (AuthorNotFoundException ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message });
        }
        catch (GenreNotFoundException ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message });
        }
    }
    
    
    /// <summary>
    /// Редактирует существующую книгу.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Не найдено книги.</response>
    /// <response code="409">Не существует указанных авторов или жанра.</response>
    [ProducesResponseType(typeof(UpdateBookResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpPost("update")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult Update(UpdateBookRequest request)
    {
        if (request.AuthorIds != null && request.AuthorIds.Count == 0)
        {
            return BadRequest(new ErrorResponse{ Message = "Авторы не могут быть пустыми" });
        }

        try
        {
            var bookId = _bookService.UpdateBook(request.BookId.Value, request.Title, request.GenreId.Value,
                request.AuthorIds);
            return Ok(new UpdateBookResponse { Id = bookId });
        }
        catch (BookNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
        catch (AuthorNotFoundException ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message });
        }
        catch (GenreNotFoundException ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message });
        }
    }

    /// <summary>
    /// Получение списка существующих книг.
    /// </summary>
    /// <param name="bookId">id книги. Можно использовать для получения конкретной книги.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetBooksResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult GetBooks([FromQuery] long? bookId)
    {
        var books = _bookService.GetBooks(bookId);
        return Ok(new GetBooksResponse { Books = books.ConvertAll(_responseMapper.MapBook) });
    }
    
    /// <summary>
    /// Удаляет книгу.
    /// </summary>
    /// <param name="bookId">id книги</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Книга не найдена.</response>
    /// <response code="409">Нельзя удалить книгу, потому что к ней привязаны офферы или отзывы.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpDelete("{bookId:long:min(1)}")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult RemoveBook(long bookId)
    {
        try
        {
            _bookService.RemoveBook(bookId);
            return Ok();
        }
        catch (RemoveBookNotAllowedException ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message });
        }
        catch (BookNotFoundException ex)
        {
            return NotFound(new ErrorResponse {Message = ex.Message}); 
        }
    }

    /// <summary>
    /// Осущствляет поиск книг по названию.
    /// </summary>
    /// <param name="bookTitle">Начало названия книги</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetBooksResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet("search")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult SearchBooksByName([FromQuery] string bookTitle)
    {
        var books = _bookService.SearchBooks(bookTitle);
        return Ok(new GetBooksResponse {Books = books.ToList().ConvertAll(_responseMapper.MapBook)});
    }
}