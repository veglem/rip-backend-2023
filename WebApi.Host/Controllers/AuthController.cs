using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Exceptions;

namespace WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/auth")]
public class AuthController : Controller
{
    private IUserHandler _userHandler;

    private readonly IDistributedCache _cache;

    public AuthController(IUserHandler userHandler, IDistributedCache cache)
    {
        _userHandler = userHandler;
        _cache = cache;
    }
    
    /// <summary>
    /// Выполняет вход в аккаунт
    /// </summary>
    /// <param name="userCredentials"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Успешный вход</response>
    /// <response code="401">Не верные логин или пароль</response>
    [HttpPost("login")]
    public async Task Login(
        [FromBody]UserCredentials userCredentials,
        CancellationToken cancellationToken)
    {
        User? user =
            await _userHandler.GetUserByLoginAndPass(userCredentials.Username, userCredentials.Password,
                cancellationToken);

        if (user is null)
        {
            await Results.Unauthorized().ExecuteAsync(HttpContext);
            return;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.IsModerator ? "moderator" : "user")
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
    }

    /// <summary>
    /// Выполняет выход из аккаунта
    /// </summary>
    /// <param name="cancellationToken"></param>
    [HttpPost("logout")]
    public async Task Logout(CancellationToken cancellationToken)
    {
        _cache.SetStringAsync(HttpContext.Request.Cookies[".AspNetCore.Cookies"],
            "true", cancellationToken);
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults
            .AuthenticationScheme);
        
        return;
    }
    
    /// <summary>
    /// Регистрирует новый аккаунт и выполняет вход в аккаунт
    /// </summary>
    /// <param name="userCredentials"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Успешное создание </response>
    /// <response code="401">Не верные логин или пароль</response>
    [HttpPost("signin")]
    public async Task Signin(
        [FromBody]UserCredentials userCredentials,
        CancellationToken cancellationToken)
    {
        try
        {
            User user =
                await _userHandler.AddUser(userCredentials.Username, userCredentials.Password,
                    cancellationToken);

            await Login(userCredentials, cancellationToken);
            return;
            
        }
        catch (ArgumentException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
            return;
        }
    }

    [HttpPut("update")]
    [Authorize]
    public async Task UpdateUserInfo(
        [FromBody] UpdateUserInfo info,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return;
        }

        try
        {
            await _userHandler.UpdateUserInfo(User.Identity.Name, info,
                cancellationToken);
        }
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
            return;
        }
        
    }
    
    [HttpGet]
    [Authorize]
    public async Task<GetUserInfo> GetUserInfo(
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        try
        {
            return await _userHandler.GetUserInfo(
                User.Identity.Name, 
                cancellationToken);
        }
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
            return null;
        }
    }

    [HttpPut("image")]
    [Authorize]
    public async Task UpdateImage(
        CancellationToken cancellationToken,
        IFormFile image)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return;
        }

        try
        {
            await _userHandler.AddImageProfile(image.OpenReadStream(),
                User.Identity.Name, cancellationToken);
        }
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message});
        }
    }

    [HttpGet("image")]
    [Authorize]
    public async Task GetImage(CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return;
        }

        GetUserInfo user = await
            _userHandler.GetUserInfo(User.Identity.Name, cancellationToken);
        
        await Results.Redirect("http://localhost:9000/" + HttpUtility.UrlPathEncode(user.ImageUrl)).ExecuteAsync(HttpContext);
    }
}
