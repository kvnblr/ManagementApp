using Management.Contracts;
using Management.Service.Contracts;

namespace Management.Service;

internal sealed class EmployeeService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    : IEmployeeService
{

}
