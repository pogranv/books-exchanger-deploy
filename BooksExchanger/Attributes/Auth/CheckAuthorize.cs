using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using BooksExchanger.Controllers.Specs;
using BooksExchanger.Models;

namespace BooksExchanger.Attributes.Auth;

/// <summary>
/// Атрибут проверки авторизации.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CheckAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Проверка авторизации.
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (AuthUser)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new ErrorResponse{ Message = "Пользователь не авторизирован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        if (user.Role != UserRole.Admin.ToString())
        {
            context.Result = new JsonResult(new ErrorResponse { Message = "Пользователь не является админом!" }) { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }
    }
}