using BattleshipBackend.Models;
using MongoDB.Driver;

namespace BattleshipBackend.Interfaces;

public interface IMongoDbContext
{
    IMongoCollection<User> UsersCollection { get; }
}