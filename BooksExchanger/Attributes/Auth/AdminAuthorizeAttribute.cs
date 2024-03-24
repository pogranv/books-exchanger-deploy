using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using BooksExchanger.Controllers.Specs;
using BooksExchanger.Models;

namespace BooksExchanger.Attributes.Auth;

/// <summary>
/// Атрибут авторизации с правами админа.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Проверка авторизации админа.
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
            context.Result = new JsonResult(new ErrorResponse { Message = "Пользователь не вяляется админом!" }) { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }
    }
}