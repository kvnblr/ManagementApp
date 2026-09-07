using AutoMapper;
using Management.Contracts;
using Management.Entities.Exceptions;
using Management.Entities.Models;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;

namespace Management.Service;

internal sealed class EmployeeService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper)
    : IEmployeeService
{
    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = mapper.Map<Employee>(employeeForCreation);

        repositoryManager.Employee.CreateEmployeeForCompany(companyId, employee);
        repositoryManager.Save();

        var employeeDto = mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = repositoryManager.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        repositoryManager.Employee.DeleteEmployee(employee);
        repositoryManager.Save();
        loggerManager.LogInfo("Employee has been deleted");
    }

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

    public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool employeeTrackChanges, bool companyTrackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, companyTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = repositoryManager.Employee.GetEmployee(companyId, id, employeeTrackChanges) ?? throw new EmployeeNotFoundException(id);
        mapper.Map(employeeForUpdate, employee);
        repositoryManager.Save();
    }
}
