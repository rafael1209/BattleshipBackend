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

    public async Task<string> AuthorizeUser(AuthStrategies strategy, string code)
    {
        var authenticatedUser = strategy switch
        {
            AuthStrategies.Google => await googleAuthProvider.GetEmailAddress(code),
            AuthStrategies.Discord => await discordAuthProvider.GetEmailAddress(code),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
        };

        return $"{authenticatedUser.Name} {authenticatedUser.Email} {authenticatedUser.AvatarUrl}"; //TODO: Generate JWT token
    }
}