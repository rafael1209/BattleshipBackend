using BattleshipBackend.Enums;
using BattleshipBackend.Interfaces;
using BattleshipBackend.Models;

namespace BattleshipBackend.Services.Auth;

public class AuthService(
    [FromKeyedServices(AuthStrategies.Google)] IOAuthProvider googleAuthProvider,
    [FromKeyedServices(AuthStrategies.Discord)] IOAuthProvider discordAuthProvider,
    IUserService userService,
    ITokenService tokenService) : IAuthService
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
            AuthStrategies.Google => await googleAuthProvider.GetAuthenticatedUser(code),
            AuthStrategies.Discord => await discordAuthProvider.GetAuthenticatedUser(code),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
        };

        var user = await userService.TryGetUserByEmail(authenticatedUser.Email) ?? await userService.CreateUser(new User
        {
            Id = default,
            Name = authenticatedUser.Name,
            Email = authenticatedUser.Email,
            AvatarUrl = authenticatedUser.AvatarUrl,
            AuthToken = tokenService.GenerateToken(authenticatedUser.Email),
            CreatedAt = default
        });

        return $"{user.AuthToken}";
    }
}