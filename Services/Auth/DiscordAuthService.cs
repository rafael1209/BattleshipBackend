using BattleshipBackend.Interfaces;

namespace BattleshipBackend.Services.Auth;

public class DiscordAuthService : IOAuthProvider
{
    public async Task<Uri> GetAuthUrl()
    {
        return new Uri("https://discord.com");
    }

    public Task<string> GetEmailAddress(string code)
    {
        throw new NotImplementedException();
    }
}