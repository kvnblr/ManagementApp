namespace Management.Shared.DataTransferObjects;

public record EmployeeForUpdateDto
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Age is a required field.")]
    [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
    int Age { get; init; }

    [Required(ErrorMessage = "Eployee position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
    public string? Position { get; init; }

}
