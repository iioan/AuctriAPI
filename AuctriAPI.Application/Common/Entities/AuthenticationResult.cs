namespace AuctriAPI.Application.Common.Entities; 
    
public record AuthenticationResult (
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);
