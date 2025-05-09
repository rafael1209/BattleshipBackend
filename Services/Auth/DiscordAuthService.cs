using BattleshipBackend.Interfaces;
using BattleshipBackend.Models;

namespace BattleshipBackend.Services.Auth;

public class DiscordAuthService : IOAuthProvider
{
    public async Task<Uri> GetAuthUrl()
    {
        return new Uri("https://discord.com");
    }

    public Task<AuthenticatedUser> GetEmailAddress(string code)
    {
        throw new NotImplementedException();
    }
}