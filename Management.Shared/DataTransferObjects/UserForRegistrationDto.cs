using System.ComponentModel.DataAnnotations;

namespace Management.Shared.DataTransferObjects;

public record UserForRegistrationDto
{
    public string? FirstName { get; init; }
    public string? Lastname { get; init; }
    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; init; }
    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public ICollection<string>? Roles { get; init; }
}

