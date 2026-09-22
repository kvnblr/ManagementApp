using AutoMapper;
using Management.Contracts;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;

namespace Management.Service;

public class ServiceManager(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper,
        IDataShaper<EmployeeDto> dataShaper,
        IEmployeeLinks employeeLinks)
    : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService =
        new(() => new CompanyService(repositoryManager, loggerManager, mapper));
    private readonly Lazy<IEmployeeService> _employeeService =
        new(() => new EmployeeService(repositoryManager, loggerManager, mapper, dataShaper, employeeLinks));

    public ICompanyService Company => _companyService.Value;

    public IEmployeeService Employee => _employeeService.Value;

}
