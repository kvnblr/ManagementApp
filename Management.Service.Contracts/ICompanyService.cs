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
}
