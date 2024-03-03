using System.Net;
using BooksExchanger.Context;
using BooksExchanger.Controllers.Specs;
using BooksExchanger.Controllers.Specs.Offers;
using BooksExchanger.Controllers.Specs.OffersCollector;
using BooksExchanger.Entities;
using BooksExchanger.Models;
using BooksExchanger.Services.Exceptions;
using BooksExchanger.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Feedback = BooksExchanger.Models.Feedback;

namespace BooksExchanger.Controllers;

/// <summary>
/// Контроллер отзывов.
/// </summary>
[ApiController]
[Route("api/v1/feedbacks")]
public class FeedbacksConstroller : ControllerBase
{
    private IFeedbackService _feedbackService;
    private ResponseMapper _responseMapper;

    public FeedbacksConstroller(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
        _responseMapper = new();
    }
    
    /// <summary>
    /// Добавляет отзыв к офферу или редактирует старый (если отзыв уже есть).
    /// </summary>
    /// <param name="offerId">id оффера.</param>
    /// <param name="request">запрос.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Не указан ни отзыв, ни оценка.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="404">Не найдено отзыва.</response>
    [ProducesResponseType(typeof(AddFeedbackReponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Attributes.Auth.UserAuthorize]
    [HttpPatch("{offerId:Guid}/feedbacks")]
    public IActionResult AddFeedback(Guid offerId, AddFeedbackRequest request)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        if (string.IsNullOrEmpty(request.Feedback) && request.Estimation == null)
        {
            return BadRequest( new ErrorResponse {Message = "Не указано ни отзыва, ни оценкки, укажите хотя бы одно"});
        }

        try
        {
            var text = request.Feedback ?? "";
            var feedbackId = _feedbackService.AddOrEditFeedback(offerId, authUser.Id, text, request.Estimation);
            return Ok(new AddFeedbackReponse { Id = feedbackId });
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse{ Message = ex.Message });
        }
    }

    // фронту отправлять null в userFeedback
    /// <summary>
    /// Возвращает все отзывы для заданного оффера.
    /// Если пользователь авторизирован, то его отзыв возвращается отдельно от остальных.
    /// </summary>
    /// <param name="offerId">id оффера.</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    [ProducesResponseType(typeof(GetFeedbacksResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [HttpGet("feedbacks/{offerId:Guid}")]
    public IActionResult GetFeedbacks(Guid offerId)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        try
        {
            var feedbacks = _feedbackService.GetFeedbacks(offerId).ToList();
            Feedback? userFeedback = null;
            if (authUser != null)
            {
                userFeedback = _feedbackService.FilterOutUserFeedback(feedbacks, authUser.Id);
            }

            var response = new GetFeedbacksResponse
            {
                Feedbacks = feedbacks.ConvertAll(_responseMapper.MapFeedback)
            };
            if (userFeedback != null)
            {
                response.UserFeedback = _responseMapper.MapFeedback(userFeedback);
            }

            return Ok(response);
        }
        catch (OfferNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет отзыв.
    /// </summary>
    /// <param name="offerId"></param>
    /// <param name="feedbackId"></param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение.</response>
    /// <response code="400">Неверный формат запроса.</response>
    /// <response code="401">Пользователь не авторизован.</response>
    /// <response code="404">Не найдено оффера или отзыва.</response>
    /// <response code="403">Пользователь не является автором отзыва.</response>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
    [Attributes.Auth.UserAuthorize]
    [HttpDelete("feedbacks/{feedbackId:long}")]
    public IActionResult RemoveFeedback(long feedbackId)
    {
        var authUser = (AuthUser)ControllerContext.HttpContext.Items["User"];
        var permissionsCheker = (Models.Feedback feedback) => { return feedback.GivenByUserId == authUser.Id; };
        try
        {
            _feedbackService.RemoveFeedback(feedbackId, permissionsCheker);
            return Ok();
        }
        catch (FeedbackNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Message = ex.Message });
        }
        catch (PermissionDenidedException ex)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new ErrorResponse{ Message = "Пользователь не является автором отзыва." }); 
        }
    }
}