using BattleshipBackend.Enums;
using BattleshipBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipBackend.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController(IAuthService authService) : Controller
{
    [HttpGet("google/url")]
    public async Task<IActionResult> GetGoogleAuthUrl()
    {
        try
        {
            var authUrl = await authService.GetAuthUrl(AuthStrategies.Google);

            return Ok(new { url = authUrl });
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                error = "An error occurred while generating the Google authentication URL. Please try again later."
            });
        }
    }

    [HttpGet("discord/url")]
    public async Task<IActionResult> GetDiscordAuthUrl()
    {
        try
        {
            var authUrl = await authService.GetAuthUrl(AuthStrategies.Discord);

            return Ok(new { url = authUrl });
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                error = "An error occurred while generating the Discord authentication URL. Please try again later."
            });
        }
    }
}