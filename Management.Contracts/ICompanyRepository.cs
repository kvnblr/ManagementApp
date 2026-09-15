using Management.Entities.Models;

namespace Management.Contracts;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    Company GetCompany(Guid id, bool trackChanges);

    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<Company> GetCompanyAsync(Guid id, bool trackChanges);

    void CreateCompany(Company company);
    void DeleteCompany(Company company);
}
