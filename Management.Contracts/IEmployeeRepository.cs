using Management.Entities.Models;

namespace Management.Contracts;

public interface IEmployeeRepository
{
    IEnunmerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
}
