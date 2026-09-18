using AutoMapper;
using Management.Contracts;
using Management.Entities.Exceptions;
using Management.Entities.Models;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Management.Shared.RequestFeatures;

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

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = mapper.Map<Employee>(employeeForCreation);

        repositoryManager.Employee.CreateEmployeeForCompany(companyId, employee);
        await repositoryManager.SaveAsync();

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

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        repositoryManager.Employee.DeleteEmployee(employee);
        await repositoryManager.SaveAsync();
        loggerManager.LogInfo("Employee has been deleted");
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = repositoryManager.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        var employeeDto = mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        var employeeDto = mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public (EmployeeForUpdateDto employeeToPatch, Employee employee) GetEmployeeForPatch(Guid companyId, Guid id, bool companyTrackChanges, bool employeeTrackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, companyTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = repositoryManager.Employee.GetEmployee(companyId, id, employeeTrackChanges) ?? throw new EmployeeNotFoundException(id);

        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employee);
        return (employeeToPatch, employee);
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employee)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool companyTrackChanges, bool employeeTrackChanges)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, companyTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, employeeTrackChanges) ?? throw new EmployeeNotFoundException(id);

        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employee);
        return (employeeToPatch, employee);
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        loggerManager.LogInfo("Getting employees...");
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employees = repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        loggerManager.LogInfo("Getting employees...");
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employees = await repositoryManager.Employee.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesWithParametersAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = await repositoryManager.Employee.GetEmployeesWithParametersAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employee);
        return employeesDto;
    }

    public async Task<(IEnumerable<EmployeeDto> employeesDto, MetaData metaData)> GetEmployeesWithParametersReturnTupleAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange) throw new MaxAgeRangeBadRequestException();
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employees = await repositoryManager.Employee.GetEmployeesWithParametersReturnPageListAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return (employeesDto, employees.MetaData);
    }

    public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employee)
    {
        mapper.Map(employeeToPatch, employee);
        repositoryManager.Save();
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employee)
    {
        mapper.Map(employeeToPatch, employee);
        await repositoryManager.SaveAsync();
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool employeeTrackChanges, bool companyTrackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, companyTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = repositoryManager.Employee.GetEmployee(companyId, id, employeeTrackChanges) ?? throw new EmployeeNotFoundException(id);
        mapper.Map(employeeForUpdate, employee);
        repositoryManager.Save();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool employeeTrackChanges, bool companyTrackChanges)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(companyId, companyTrackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employee = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, employeeTrackChanges) ?? throw new EmployeeNotFoundException(id);
        mapper.Map(employeeForUpdate, employee);
        await repositoryManager.SaveAsync();
    }
}
