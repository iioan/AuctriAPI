using AuctriAPI.Application.Common.Entities;
using AuctriAPI.Application.Common.Interfaces.Authentication;
using AuctriAPI.Application.Common.Interfaces.Persistence;
using AuctriAPI.Application.Services.DateTime;
using AuctriAPI.Core.Constants;
using AuctriAPI.Core.Entitites;
using MediatR;

namespace AuctriAPI.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // check if user alr exists
        if (await _userRepository.UserExistsAsync(command.Email))
            throw new Exception("User already exists");

        // create user (generate unique Guid)
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            UserName = command.Email, // Using email as username, can be changed
            Role = UserRole.User,
            CreatedDateTime = _dateTimeProvider.UtcNow.DateTime,
            UpdatedDateTime = _dateTimeProvider.UtcNow.DateTime
        };

        // Persist to DB
        var result = await _userRepository.AddAsync(user, command.Password);
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