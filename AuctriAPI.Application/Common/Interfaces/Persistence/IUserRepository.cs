using AuctriAPI.Core.Entitites;

namespace AuctriAPI.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    User? GetUserById(Guid id);
    void Add(User user);
    bool UserExists(string email);
}