using Management.Contracts;
using Management.Entities.Models;

namespace Management.Repository;

public class CompanyRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(c => c.Name).ToList();

    public Company GetCompany(Guid id, bool trackChanges) =>
        FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();
}
