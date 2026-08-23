using Management.Contracts;
using Management.Entities.Models;
using Management.Service.Contracts;

namespace Management.Service;

internal sealed class CompanyService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    : ICompanyService
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        var companies = repositoryManager.Company.GetAllCompanies(trackChanges);
        return companies;
    }
}
