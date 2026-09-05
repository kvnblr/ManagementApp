using Management.Contracts;
using Management.Entities.Models;

namespace Management.Repository;

public class CompanyRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public void CreateCompany(Company company) => Create(company);

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(c => c.Name).ToList();

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();

    public Company GetCompany(Guid id, bool trackChanges) =>
        FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();
}
