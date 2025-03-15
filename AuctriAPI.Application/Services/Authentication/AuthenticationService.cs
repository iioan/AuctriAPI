using AuctriAPI.Application.Common.Interfaces.Authentication;
using AuctriAPI.Application.Common.Interfaces.Persistence;
using AuctriAPI.Application.Common.Interfaces.Security;
using AuctriAPI.Application.Services.DateTime;
using AuctriAPI.Core.Constants;
using AuctriAPI.Core.Entitites;
using Microsoft.AspNetCore.Identity;

namespace AuctriAPI.Application.Services.Authentication;

public class AuthenticationService(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider,
    SignInManager<User> signInManager) : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
            throw new Exception("User not found");

        // Validate password using SignInManager (handles lockouts, etc.)
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!result.Succeeded)
        {
            throw new Exception("Invalid credentials");
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);
        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email!,
            token);
    }

    public async Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        // check if user alr exists
        if (await _userRepository.UserExistsAsync(email))
            throw new Exception("User already exists");

        // create user (generate unique Guid)
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email, // Using email as username, can be changed
            Role = UserRole.User,
            CreatedDateTime = _dateTimeProvider.UtcNow.DateTime,
            UpdatedDateTime = _dateTimeProvider.UtcNow.DateTime
        };

        // Persist to DB
        var result = await _userRepository.AddAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        
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