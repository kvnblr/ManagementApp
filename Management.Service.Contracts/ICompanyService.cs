using Management.Entities.Models;

namespace Management.Service.Contracts;

public interface ICompanyService
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
}
