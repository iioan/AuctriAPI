using AuctriAPI.Application.Common.Entities;
using AuctriAPI.Application.Common.Interfaces.Authentication;
using AuctriAPI.Application.Common.Interfaces.Persistence;
using AuctriAPI.Core.Entitites;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuctriAPI.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    SignInManager<User> signInManager
) : IRequestHandler<LoginCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly SignInManager<User> _signInManager = signInManager;


    public async Task<AuthenticationResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email);
        if (user == null)
            throw new Exception("User not found");

        // Validate password using SignInManager (handles lockouts, etc.)
        var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, true);
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
}