using Management.Contracts;
using Management.Service.Contracts;

namespace Management.Service;

internal sealed class CompanyService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    : ICompanyService
{

}
