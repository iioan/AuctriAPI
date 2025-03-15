using AuctriAPI.Application.Common.Interfaces.Security;
using System.Security.Cryptography;

namespace AuctriAPI.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    // BCrypt automatically handles salt generation and verification
    // WorkFactor controls the complexity (12 is a good starting point)
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}