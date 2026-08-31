using AutoMapper;
using Management.Contracts;
using Management.Entities.Exceptions;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;

namespace Management.Service;

internal sealed class EmployeeService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper)
    : IEmployeeService
{
    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = repositoryManager.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        var employeeDto = mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        loggerManager.LogInfo("Getting employees...");
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employees = repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }
}
