using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.DataAccess;

namespace WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/auth")]
public class AuthController : Controller
{
    private IUserHandler _userHandler;

    public AuthController(IUserHandler userHandler)
    {
        _userHandler = userHandler;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userCredentials"></param>
    /// <param name="cancellationToken"></param>
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
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    [HttpPost("logout")]
    public async Task Logout(CancellationToken cancellationToken)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults
            .AuthenticationScheme);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userCredentials"></param>
    /// <param name="cancellationToken"></param>
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
            Results.BadRequest(ex.Message);
        }
    }
}
