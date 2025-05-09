using BattleshipBackend.Models;

namespace BattleshipBackend.Interfaces;

public interface IOAuthProvider
{
    Task<Uri> GetAuthUrl();
    Task<AuthenticatedUser> GetAuthenticatedUser(string code);
}