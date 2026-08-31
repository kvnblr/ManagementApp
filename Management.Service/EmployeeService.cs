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
    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employees = repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return employeesDto;
    }
}
