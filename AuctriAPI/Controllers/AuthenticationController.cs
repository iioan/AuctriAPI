using System.Security.Claims;
using AuctriAPI.Application.Features.Authentication.Commands.Login;
using AuctriAPI.Application.Features.Authentication.Commands.Register;
using AuctriAPI.Contracts.Authentication;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctriAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(
    IMapper mapper,
    IMediator mediator) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
            
        var authResult = await _mediator.Send(command);
        var response = _mapper.Map<AuthenticationResponse>(authResult);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password);
        
        var authResult = await _mediator.Send(command);
        var response = _mapper.Map<AuthenticationResponse>(authResult);
        
        return Ok(response);
    }

    [HttpGet("test")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Test()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok($"Authenticated user ID: {userId}");
    }

    [HttpGet("test-no-auth")]
    public IActionResult TestNoAuth()
    {
        return Ok("Test Without Auth");
    }
}