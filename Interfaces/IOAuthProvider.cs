namespace BattleshipBackend.Interfaces;

public interface IOAuthProvider
{
    Task<Uri> GetAuthUrl();
    Task<string> GetEmailAddress(string code);
}