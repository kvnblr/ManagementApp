using AutoMapper;
using Management.Contracts;
using Management.Entities.Models;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Management.Service;

public class ServiceManager(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper,
        IDataShaper<EmployeeDto> dataShaper,
        IEmployeeLinks employeeLinks,
        UserManager<User> userManager,
        IConfiguration configuration)
    : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService =
        new(() => new CompanyService(repositoryManager, loggerManager, mapper));
    private readonly Lazy<IEmployeeService> _employeeService =
        new(() => new EmployeeService(repositoryManager, loggerManager, mapper, dataShaper, employeeLinks));
    private readonly Lazy<IAuthenticationService> _authenticationService =
        new(() => new AuthenticationService(loggerManager, mapper, userManager, configuration));

    public ICompanyService Company => _companyService.Value;

    public IEmployeeService Employee => _employeeService.Value;

    public IAuthenticationService Authentication => _authenticationService.Value;

}
