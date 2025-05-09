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

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    {
        try
        {
            var authToken = await authService.AuthorizeUser(AuthStrategies.Google, code);

            return Ok(new { authToken });
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                error = "An error occurred while processing the Google authentication callback. Please try again later."
            });
        }
    }

    [HttpGet("discord/callback")]
    public async Task<IActionResult> DiscordCallback([FromQuery] string code)
    {
        try
        {
            var authToken = await authService.AuthorizeUser(AuthStrategies.Discord, code);

            return Ok(new { authToken });
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                error = "An error occurred while processing the Discord authentication callback. Please try again later."
            });
        }
    }
}