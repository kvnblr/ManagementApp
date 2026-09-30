using Microsoft.AspNetCore.Identity;

namespace Management.Entities.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
}

