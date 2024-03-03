using System.Net;
using System.Security.AccessControl;
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Authors;
using BooksExchanger.Controllers.Specs.Genres;
using BooksExchanger.Models;
using BooksExchanger.Models.Requests;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Implementations.GenreService;
using BooksExchanger.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер жанров.
/// </summary>
[ApiController]
[Route("admin/v1/genres")]
public class GenresController : ControllerBase
{
    private IGenreService _genreService;
    private ResponseMapper _responseMapper;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
        _responseMapper = new();
    }
    
    /// <summary>
    /// Создает новый жанр.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="409">Жанр с таким именем уже существует.</response>
    [ProducesResponseType(typeof(CreateUpdateGenreResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpPost("create")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult CreateGenre(AddGenreRequest request)
    {
        try
        {
            var genreId = _genreService.AddGenre(request.Name);
            return Ok(new CreateUpdateGenreResponse { Id = genreId });
        }
        catch (GenreAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Редактирует существующий жанр.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Жанр с таким id не существует.</response>
    /// <response code="409">Жанр с таким именем уже существует.</response>
    [ProducesResponseType(typeof(CreateUpdateGenreResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpPost("update")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult UpdateGenre(UpdateGenresRequest request)
    {
        try
        {
            var genreId = _genreService.UpdateGenre(request.Id.Value, request.Name);
            return Ok(new CreateUpdateGenreResponse { Id = genreId });
        }
        catch (GenreNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (GenreAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Возвращает все существующие жанры.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetGenresResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet]
    public IActionResult GetGenres()
    {
        var authors = _genreService.GetGenres().ToList();
        var responseGenres = authors.ConvertAll(_responseMapper.MapGenre);
        return Ok(new GetGenresResponse { Genres = responseGenres });
    }
    
    /// <summary>
    /// Удаляет жанр.
    /// </summary>
    /// <param name="genreId">id жанра</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Жанр не найден.</response>
    /// <response code="409">К жанру привязаны книги.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpDelete("{genreId:int:min(1)}")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult RemoveGenres(int genreId)
    {
        try
        {
            _genreService.RemoveGenre(genreId);
            return Ok();
        }
        catch (GenreNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (RemoveGenreNotAllowedException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Осуществляет поиск жанров.
    /// </summary>
    /// <param name="genreName">Начало названия жанра.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetGenresResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet("search")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult SearchGenres([FromQuery] string genreName)
    {
        var genres = _genreService.SearchGenres(genreName).ToList();
        var responseGenres = genres.ConvertAll(_responseMapper.MapGenre);
        return Ok(new GetGenresResponse { Genres = responseGenres });
    }
}