using Management.Shared.DataTransferObjects;

namespace Management.Service.Contracts;

public interface ICompanyService
{
    (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    CompanyDto GetCompany(Guid id, bool trackChanges);
    CompanyDto CreateCompany(CompanyForCreationDto companyForCreation);
    void DeleteCompany(Guid id, bool trackChanges);
    void UpdateCompany(Guid id, CompanyForUpdateDto companyForUpdate, bool trackChanges);

    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto companyForCreation);
    Task DeleteCompanyAsync(Guid id, bool trackChanges);
    Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto companyForUpdate, bool trackChanges);
}
