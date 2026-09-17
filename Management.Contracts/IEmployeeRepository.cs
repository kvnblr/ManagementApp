using Management.Entities.Models;
using Management.Shared.RequestFeatures;

namespace Management.Contracts;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
    Employee GetEmployee(Guid companyId, Guid id, bool trackChanges);

    Task<PagedList<Employee>> GetEmployeesWithParametersReturnPageListAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<IEnumerable<Employee>> GetEmployeesWithParametersAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}
