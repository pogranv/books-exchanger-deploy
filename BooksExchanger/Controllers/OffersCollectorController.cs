using System.Net;

using Microsoft.AspNetCore.Mvc;

using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.OffersCollector;
using BooksExchanger.Models;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;

using ModerationStatus = BooksExchanger.Models.ModerationStatus;

namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер офферов на модрации.
/// </summary>
[ApiController]
public class OffersCollectorController : ControllerBase
{
    private IOffersCollectorService _offersCollectorService;
    private ResponseMapper _responseMapper;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="offersCollectorService">Сервис офферов на модерации.</param>
    public OffersCollectorController(IOffersCollectorService offersCollectorService)
    {
        _offersCollectorService = offersCollectorService;
        _responseMapper = new();
    }

    /// <summary>
    /// Отправляет оффер на модерацию.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    [ProducesResponseType(typeof(CreateOfferResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Attributes.Auth.UserAuthorize]
    [HttpPost("api/v1/offers/collector/create")]
    public async Task<IActionResult> CreateOffer(CreateOfferRequest request)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        var offerId = await _offersCollectorService.CreateOffer(authUser.Id, request.Title, request.Authors, request.City,
            request.Description, request.Image, request.Price);
        return Ok(new CreateOfferResponse{ Id = offerId});
    }

    /// <summary>
    /// Отклоняет оффер при модерации.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Оффер не найден.</response>
    [ProducesResponseType(typeof(RejectOfferResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [HttpPatch("admin/v1/offers/collector/reject")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult RejectOffer(RejectOfferRequest request)
    {
        try
        {
            var offerId = _offersCollectorService.RejectOffer(request.Id, request.RejectReason);
            return Ok(new RejectOfferResponse { Id = offerId });
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
    }

    /// <summary>
    /// Подтверждает оффер при модерации.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    /// <response code="404">Оффер не найден или книга не найдена.</response>
    /// <response code="409">Оффер уже подтвержден.</response>
    [ProducesResponseType(typeof(ApproveOfferResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
    [HttpPost("admin/v1/offers/collector/approve")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult ApproveOffer(ApproveOfferRequest request)
    {
        try
        {
            var offerId = _offersCollectorService.ApproveOfferAndGetId(request.OfferId, request.LinkedBookId);
            return Ok(new ApproveOfferResponse { Id = offerId });
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (OfferAlreadyApprovedException ex)
        {
            return Conflict(new ErrorResponse { Message = ex.Message });
        }
        catch (BookNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
    }
    
    /// <summary>
    /// Возвращает офферы для модерации.
    /// </summary>
    /// <param name="offerId">id оффера, если нужно вернуть конкретный оффер.</param>
    /// <param name="considerationOnly">Вернуть только офферы, которые ждут модерации.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="403">Пользователь не является админом.</response>
    [ProducesResponseType(typeof(GetOffersCollectorResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [HttpGet("admin/v1/offers/collector")]
    [Attributes.Auth.AdminAuthorize]
    public IActionResult GetOffers([FromQuery] Guid? offerId = null, [FromQuery] bool? considerationOnly = null)
    {
        HashSet<ModerationStatus> needStatuses;
        if (considerationOnly.HasValue && considerationOnly.Value)
        {
            needStatuses = new HashSet<ModerationStatus> {ModerationStatus.Consideration};
        }
        else
        {
            needStatuses = new HashSet<ModerationStatus> {ModerationStatus.Rejected, ModerationStatus.Submitted, ModerationStatus.Approved, ModerationStatus.Consideration};
        }

        try
        {
            var offers = _offersCollectorService.GetOffers(needStatuses, offerId).ToList()
                .ConvertAll(_responseMapper.MapOfferCollector);
            return Ok(new GetOffersCollectorResponse { Offers = offers });
        }
        catch (OfferNotFoundException ex)
        {
            return Ok(new GetOffersCollectorResponse { Offers = {} }); 
        }
    }
    
    /// <summary>
    /// Возвращает отклоненные офферы пользователя.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    [ProducesResponseType(typeof(GetOffersCollectorResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Attributes.Auth.UserAuthorize]
    [HttpGet("api/v1/offers/collector/rejected")]
    public IActionResult GetRejectedOffers()
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        var offers = _offersCollectorService.GetOffers(new HashSet<ModerationStatus> { ModerationStatus.Rejected },
            userId: authUser.Id).ToList().ConvertAll(_responseMapper.MapOfferCollector);
        return Ok(new GetOffersCollectorResponse{ Offers = offers });
    }
    
    /// <summary>
    /// Возвращает офферы пользователя, находящиеся в статусе модерации.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    [ProducesResponseType(typeof(GetOffersCollectorResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Attributes.Auth.UserAuthorize]
    [HttpGet("api/v1/offers/collector/considerations")]
    public IActionResult GetConsiderationOffers()
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        var offers = _offersCollectorService.GetOffers(new HashSet<ModerationStatus> { ModerationStatus.Submitted },
            userId: authUser.Id).ToList().ConvertAll(_responseMapper.MapOfferCollector);
        return Ok(new GetOffersCollectorResponse{ Offers = offers });
    }
    
    
    /// <summary>
    /// Удаляет оффер, находящийся на модерации.
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
    [HttpDelete("api/v1/offers/collector/{offerId:Guid}")]
    public IActionResult RemoveOffer(Guid offerId)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        try
        {
            _offersCollectorService.RemoveOffer(offerId, authUser.Id);
            return Ok();
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message =ex.Message });
        }
        catch (UserNotOfferOwnerException ex)
        {
            return StatusCode((int)HttpStatusCode.Forbidden,
                new ErrorResponse { Message = ex.Message });
        }
    }
}