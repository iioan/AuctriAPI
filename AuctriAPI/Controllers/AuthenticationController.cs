using AuctriAPI.Application.Services.Authentication;
using AuctriAPI.Contracts.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctriAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(IAuthenticationService authenticationService, IMapper mapper) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IMapper _mapper = mapper;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // Implementation
        var authResult = await _authenticationService.RegisterAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var response = _mapper.Map<AuthenticationResponse>(authResult);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        // Implementation
        var authResult = await _authenticationService.LoginAsync(
            request.Email,
            request.Password
        );

        var response = _mapper.Map<AuthenticationResponse>(authResult);
        return Ok(response);
    }

    [HttpGet("test"), Authorize]
    public IActionResult Test()
    {
        return Ok("Test");
    }
}