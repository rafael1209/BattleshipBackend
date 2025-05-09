using BattleshipBackend.Models;
using MongoDB.Bson;

namespace BattleshipBackend.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserById(ObjectId id);
    Task<User?> TryGetUserByEmail(string email);
    Task<User?> TryGetUserByAuthToken(string authToken);
    Task<User> CreateUser(User user);
}