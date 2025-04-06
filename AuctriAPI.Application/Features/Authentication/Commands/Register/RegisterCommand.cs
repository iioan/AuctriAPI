using AuctriAPI.Application.Common.Entities;
using MediatR;

namespace AuctriAPI.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<AuthenticationResult>;
