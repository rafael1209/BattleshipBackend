using BattleshipBackend.Enums;
using BattleshipBackend.Interfaces;

namespace BattleshipBackend.Services.Auth;

public class AuthService(
    [FromKeyedServices(AuthStrategies.Google)] IOAuthProvider googleAuthProvider,
    [FromKeyedServices(AuthStrategies.Discord)] IOAuthProvider discordAuthProvider) : IAuthService
{
    public async Task<Uri> GetAuthUrl(AuthStrategies strategy)
    {
        return strategy switch
        {
            AuthStrategies.Google => await googleAuthProvider.GetAuthUrl(),
            AuthStrategies.Discord => await discordAuthProvider.GetAuthUrl(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
        };
    }
}