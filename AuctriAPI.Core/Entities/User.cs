using Microsoft.AspNetCore.Identity;
using AuctriAPI.Core.Constants;

namespace AuctriAPI.Core.Entitites;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    
    // Note: IdentityUser already includes properties like:
    // - Id (Guid)
    // - UserName
    // - Email
    // - PasswordHash
    // - SecurityStamp
    // and more
}
