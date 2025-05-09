using BattleshipBackend.Interfaces;
using BattleshipBackend.Models;
using x3rt.DiscordOAuth2;
using x3rt.DiscordOAuth2.Entities.Enums;

namespace BattleshipBackend.Services.Auth;

public class DiscordAuthService(IConfiguration configuration) : IOAuthProvider
{
    private readonly string _redirectUri = configuration["Discord:RedirectUri"];

    public async Task<Uri> GetAuthUrl()
    {
        var scopes = new ScopesBuilder(OAuthScope.Identify, OAuthScope.Email);
        var oAuth = new DiscordOAuth(_redirectUri, scopes, "");
        var url = oAuth.GetAuthorizationUrl();

        return new Uri(url);
    }

    public async Task<AuthenticatedUser> GetAuthenticatedUser(string code)
    {
        var scopes = new ScopesBuilder(OAuthScope.Identify, OAuthScope.Email);
        var oAuth = new DiscordOAuth(_redirectUri, scopes, "");

        var token = await oAuth.GetTokenAsync(code);
        var user = await oAuth.GetUserAsync(token);

        var avatarUrl = user.Avatar != null
            ? $"https://cdn.discordapp.com/avatars/{user.Id}/{user.Avatar}.png"
            : "https://cdn.discordapp.com/embed/avatars/0.png";

        return new AuthenticatedUser(user.Username, user.Email, avatarUrl);
    }
}
