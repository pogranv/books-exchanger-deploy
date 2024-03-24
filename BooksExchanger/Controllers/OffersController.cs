using System.Net;

using Microsoft.AspNetCore.Mvc;

using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Offers;
using BooksExchanger.Models;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;


namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер офферов.
/// </summary>
[ApiController]
[Route("api/v1/offers")]
public class OffersController : ControllerBase
{
    private IOfferService _offerService;
    private ResponseMapper _responseMapper;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="offerService">Сервис офферов.</param>
    public OffersController(IOfferService offerService)
    {
        _offerService = offerService;
        _responseMapper = new();
    }
    
    /// <summary>
    /// Возвращает оффер по id.
    /// </summary>
    /// <param name="offerId">id оффера.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="404">Оффер не найден.</response>
    [ProducesResponseType(typeof(GetOfferResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [HttpGet("{offerId:Guid}")]
    public IActionResult GetOffer(Guid offerId)
    {
        try
        {
            var offer = _offerService.GetOffer(offerId);
            var isFavoriteForUser = false;
            var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
            if (authUser != null)
            {
                isFavoriteForUser = _offerService.IsOfferFavoriteForUser(authUser.Id, offerId);
            }
            return Ok(new GetOfferResponse {Offer = _responseMapper.MapOffer(offer, isFavoriteForUser)});
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
    }
    
    /// <summary>
    /// Возвращает офферы, подходящие под заданные фильтры.
    /// </summary>
    /// <param name="genreId">id жанра.</param>
    /// <param name="city">Название города.</param>
    /// <param name="userId">id владельца..</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    [ProducesResponseType(typeof(GetOffersResponse), (int)HttpStatusCode.OK)]
    [HttpGet]
    public IActionResult GetOffers([FromQuery] int? genreId = null, [FromQuery] string? city = null, [FromQuery] long? userId = null)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        long? notUserId = null;
        if (authUser != null)
        {
            notUserId = authUser.Id;
        }

        var offers = _offerService.GetOffers(genreId, city, userId, notUserId);
        return Ok(new GetOffersResponse {Offers = offers.ToList().ConvertAll(_responseMapper.MapOffer)});
    }

    /// <summary>
    /// Добавляет оффер в избранное пользователя.
    /// </summary>
    /// <param name="offerId">id оффера.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="404">Не найдено оффера.</response>
    /// <response code="409">Отзыв уже добавлен в избранное.</response>
    /// <response code="500">Неизвестная ошибка.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [Attributes.Auth.UserAuthorize]
    [HttpPatch("favorites/{offerId:Guid}")]
    public IActionResult MarkOfferFavorite(Guid offerId)
    {
        
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        try
        {
            _offerService.AddOfferToFavorite(offerId, authUser.Id);
            return Ok();
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
        catch (OfferAlreadyFavoriteException ex)
        {
            return Conflict(new ErrorResponse{ Message = ex.Message });
        }
    }
    
    /// <summary>
    /// Удаляет оффер из избранного пользователя.
    /// </summary>
    /// <param name="offerId">id оффера.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="404">Оффер не находится в избранном пользователя.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Attributes.Auth.UserAuthorize]
    [HttpDelete("favorites/{offerId:Guid}")]
    public IActionResult RemoveOfferFromFavorite(Guid offerId)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        try
        {
            _offerService.RemoveOfferFromFavorite(offerId, authUser.Id);
            return Ok();
        }
        catch (OfferAlreadyNotInFavoritesException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет оффер.
    /// </summary>
    /// <param name="offerId">id оффера</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является владельцем объявления.</response>
    /// <response code="404">Оффер не найден.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Attributes.Auth.UserAuthorize]
    [HttpDelete("{offerId:Guid}")]
    public IActionResult RemoveOffer(Guid offerId)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        try
        {
            _offerService.RemoveOffer(offerId, authUser.Id);
            return Ok();
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (UserNotOfferOwnerException ex)
        {
            return StatusCode((int)HttpStatusCode.Forbidden,
            new ErrorResponse { Message = ex.Message });
        }
    }
    
    /// <summary>
    /// Получение избранных офферов пользователя.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    [ProducesResponseType(typeof(GetOffersResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Attributes.Auth.UserAuthorize]
    [HttpGet("favorites")]
    public IActionResult GetFavoriteOffers()
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        var offers = _offerService.GetFavoriteOffers(authUser.Id).ToList().ConvertAll(_responseMapper.MapOffer);
        return Ok(new GetOffersResponse{ Offers = offers });
    }
    
    /// <summary>
    /// Возвращает подборки книг для главной приложения.
    /// Может фильтровать по городу.
    /// </summary>
    /// <param name="city">Название города.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    [ProducesResponseType(typeof(GetOffersSelectionsResponse), (int)HttpStatusCode.OK)]
    [HttpGet("selections")]
    public IActionResult GetOffersSelections([FromQuery] string? city = null)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        long? userId = authUser?.Id;

        var selections = _offerService.GetOfferSelections(userId, city);
        var response = new GetOffersSelectionsResponse
        {
            Selections = selections.ConvertAll(_responseMapper.MapSelection)
        };

        return Ok(response);
        
    }
}