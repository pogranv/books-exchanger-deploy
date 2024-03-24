using System.Net;

using Microsoft.AspNetCore.Mvc;

using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Authors;
using BooksExchanger.Models;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;

namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер авторов.
/// </summary>
[ApiController]
[Route("admin/v1/authors")]
public class AuthorsController : ControllerBase
{
    private IAuthorService _authorService;
    private ResponseMapper _responseMapper;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="authorService">Сервис авторов.</param>
    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
        _responseMapper = new();
    }
    
    /// <summary>
    /// Создает нового автора.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(CreateAuthorResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpPost("create")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult Create(CreateAuthorRequest request)
    {
        var authorId = _authorService.AddAuthor(request.Name);
        return Ok(new CreateAuthorResponse{Id = authorId});
    }
    
    /// <summary>
    /// Редактирует автора по id.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Автора с таким id не существует.</response>
    [ProducesResponseType(typeof(UpdateAuthorRequest), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpPost("update")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult UpdateAuthor(UpdateAuthorRequest request)
    {
        try
        {
            var authorId = _authorService.UpdateAuthor(request.Id, request.NewName);
            return Ok(new UpdateAuthorResponse { Id = authorId });
        }
        catch (AuthorNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
    }

    /// <summary>
    /// Отдает всех существующих авторов.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetAuthorsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult GetAuthors()
    {
        var authors = _authorService.GetAuthors().ToList();
        var responseAuthors = authors.ConvertAll(_responseMapper.MapAuthor);
        return Ok(new GetAuthorsResponse { Authors = responseAuthors });
    }


    /// <summary>
    /// Удаляет автора по id.
    /// </summary>
    /// <param name="authorId">id автора.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Автора не существует.</response>
    /// <response code="409">К данному автору привязаны книги.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpDelete("{authorId:long:min(1)}")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult RemoveAuthor(long authorId)
    {
        try
        {
            _authorService.RemoveAuthor(authorId);
            return Ok();
        }
        catch (AuthorNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (RemoveAuthorNotAllowedException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Осуществляет поиск авторов.
    /// </summary>
    /// <param name="authorName">Начало ФИО автора.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetAuthorsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet("search")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult SearchAuthorsByName([FromQuery] string authorName)
    {
        var authors = _authorService.SearchAuthors(authorName).ToList();
        var responseAuthors = authors.ConvertAll(_responseMapper.MapAuthor);
        return Ok(new GetAuthorsResponse { Authors = responseAuthors });
    }
}