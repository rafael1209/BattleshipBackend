using BattleshipBackend.Enums;

namespace BattleshipBackend.Interfaces;

public interface IAuthService
{
    Task<Uri> GetAuthUrl(AuthStrategies strategy);
    Task<string> AuthorizeUser(AuthStrategies strategy, string code);
}