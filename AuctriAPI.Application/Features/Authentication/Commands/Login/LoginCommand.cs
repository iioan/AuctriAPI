using AuctriAPI.Application.Common.Entities;
using MediatR;

namespace AuctriAPI.Application.Features.Authentication.Commands.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<AuthenticationResult>;