using BattleshipBackend.Interfaces;
using BattleshipBackend.Middlewares;
using BattleshipBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipBackend.Controllers;

[Route("api/battleship")]
[ApiController]
public class BattleshipController(IBattleshipService battleshipService) : Controller
{
    [HttpGet]
    [AuthMiddleware]
    public async Task<IActionResult> GetGames()
    {
        var user = HttpContext.Items["@me"] as User
                   ?? throw new Exception("Error user not found in context");

        return Ok(new { user });
    }
}