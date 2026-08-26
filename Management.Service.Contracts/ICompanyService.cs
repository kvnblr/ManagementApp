using Management.Shared.DataTransferObjects;

namespace Management.Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
}
