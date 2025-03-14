using AuctriAPI.Application.Common.Interfaces.Authentication;
using AuctriAPI.Application.Common.Interfaces.Persistence;
using AuctriAPI.Application.Common.Interfaces.Security;
using AuctriAPI.Application.Services.DateTime;
using AuctriAPI.Core.Constants;
using AuctriAPI.Core.Entitites;

namespace AuctriAPI.Application.Services.Authentication;

public class AuthenticationService(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public AuthenticationResult Login(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);
        if (user == null)
            throw new Exception("User not found");

        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            throw new Exception("Invalid password");
        
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);
        return new AuthenticationResult(Guid.NewGuid(), user.FirstName, user.LastName, email, token);
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // check if user alr exists
        if (_userRepository.UserExists(email))
            throw new Exception("User already exists");

        // create user (generate unique Guid)
        string username = email.Split('@')[0];
        // create JWT token
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Username = username,
            PasswordHash = _passwordHasher.HashPassword(password),
            Role = UserRole.User,
            CreatedDateTime = _dateTimeProvider.UtcNow.DateTime,
            UpdatedDateTime = _dateTimeProvider.UtcNow.DateTime
        };

        // Persist to DB
        _userRepository.Add(user);

        // Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            token);
    }
}