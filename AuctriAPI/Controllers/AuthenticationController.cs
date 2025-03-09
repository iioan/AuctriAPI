using AuctriAPI.Application.Services.Authentication;
using AuctriAPI.Contracts.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctriAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        // Implementation
        var authResult = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
            );

        var response = new AuthenticationResult(
            authResult.Id,
            authResult.FirstName,
            authResult.LastName,
            authResult.Email,
            authResult.Token
            );

        return Ok(response);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        // Implementation
        var authResult = _authenticationService.Login(
            request.Email,
            request.Password
            );

        var response = new AuthenticationResult(
            authResult.Id,
            authResult.FirstName,
            authResult.LastName,
            authResult.Email,
            authResult.Token
            );
        return Ok(response);
    }
    
    [HttpGet("test"), Authorize]
    public IActionResult Test()
    {
        return Ok("Test");
    }
}
