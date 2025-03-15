using AuctriAPI.Core.Entitites;
using Microsoft.AspNetCore.Identity;

namespace AuctriAPI.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<IdentityResult> AddAsync(User user, string password);
    Task<bool> UserExistsAsync(string email);
}