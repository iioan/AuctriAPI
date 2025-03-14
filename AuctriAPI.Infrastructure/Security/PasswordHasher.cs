using AuctriAPI.Application.Common.Interfaces.Security;
using System.Security.Cryptography;

namespace AuctriAPI.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
    private const char Delimiter = '.';

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _algorithm,
            KeySize);

        return string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash),
            Iterations,
            _algorithm);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var elements = passwordHash.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);
        var iterations = int.Parse(elements[2]);
        var algorithm = new HashAlgorithmName(elements[3]);
        
        var testHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            algorithm,
            hash.Length);
        
        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }
}