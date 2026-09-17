using Management.Entities.Models;
using Management.Shared.DataTransferObjects;
using Management.Shared.RequestFeatures;

namespace Management.Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);
    EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
    void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
    void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool employeeTrackChanges, bool companyTrackChanges);
    (EmployeeForUpdateDto employeeToPatch, Employee employee) GetEmployeeForPatch(Guid companyId, Guid id, bool companyTrackChanges, bool employeeTrackChanges);
    void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employee);

    Task<(IEnumerable<EmployeeDto> employeesDto, MetaData metaData)> GetEmployeesWithParametersReturnTupleAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<IEnumerable<EmployeeDto>> GetEmployeesWithParametersAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool employeeTrackChanges, bool companyTrackChanges);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employee)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool companyTrackChanges, bool employeeTrackChanges);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employee);
}
