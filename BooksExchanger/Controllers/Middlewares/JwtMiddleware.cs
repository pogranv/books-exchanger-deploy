using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BooksExchanger.Models;
using BooksExchanger.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BooksExchanger.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _applicationSettings;

    /// <summary>
    /// Конструктор мидлвари авторизации.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="appSettings"></param>
    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _applicationSettings = appSettings.Value;
    }

    /// <summary>
    /// Добавляет информацию о пользователе в контекст запроса.
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            AttachUserToContext(context, token);
        }
        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_applicationSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = long.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            var userRole = jwtToken.Claims.First(x => x.Type == "role").Value;

            AuthUser user = new AuthUser();
            user.Id = userId;
            user.Role = userRole;

            context.Items["User"] = user;
        }
        catch
        {
            // ignored
        }
    }
}