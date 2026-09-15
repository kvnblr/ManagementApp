using Microsoft.EntityFrameworkCore;
using Management.Contracts;
using Management.Entities.Models;

namespace Management.Repository;

public class CompanyRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public void CreateCompany(Company company) => Create(company);

    public void DeleteCompany(Company company) => Delete(company);

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(c => c.Name).ToList();

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
        await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges).ToList();

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
         => await FindByCondition(c => ids.Contains(c.Id), trackChanges).ToListAsync();

    public Company GetCompany(Guid id, bool trackChanges) =>
        FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault()!;

    public async Task<Company> GetCompanyAsync(Guid id, bool trackChanges) =>
         await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync()!;

}
