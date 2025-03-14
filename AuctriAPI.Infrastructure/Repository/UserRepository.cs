using AuctriAPI.Application.Common.Interfaces.Persistence;
using AuctriAPI.Core.Entitites;
using AuctriAPI.Infrastructure.Persistence;

namespace AuctriAPI.Infrastructure.Repository;

public class UserRepository(AuctriDbContext dbContext) : IUserRepository
{
    private readonly AuctriDbContext _dbContext = dbContext;

    public User? GetUserByEmail(string email)
    {
        return _dbContext.Users.SingleOrDefault(u => u.Email == email);
    }

    public User? GetUserById(Guid id)
    {
        return _dbContext.Users.Find(id);
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public bool UserExists(string email)
    {
        return _dbContext.Users.Any(u => u.Email == email);
    }
}