namespace BattleshipBackend.Interfaces;

public interface ITokenService
{
    string GenerateToken(string value);
}