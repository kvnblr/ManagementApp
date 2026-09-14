using System.ComponentModel.DataAnnotations;

namespace Management.Shared.DataTransferObjects;

public record EmployeeForCreationDto
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Age is a required field.")]
    int Age { get; init; }

    [Required(ErrorMessage = "Eployee position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
    public string? Position { get; init; }
}
