using Management.Contracts;
using Management.Service.Contracts;

namespace Management.Service;

public class ServiceManager(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService =
        new(() => new CompanyService(repositoryManager, loggerManager));
    private readonly Lazy<IEmployeeService> _employeeService =
        new(() => new EmployeeService(repositoryManager, loggerManager));

    public ICompanyService Company => _companyService.Value;

    public IEmployeeService Employee => _employeeService.Value;

}
