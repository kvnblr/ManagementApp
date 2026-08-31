
namespace Management.Entities.Exceptions;

public sealed class EmployeeNotFoundException(Guid id)
    : NotFoundException($"The employee with id: {id} doesn't exists in the database")
{ }
