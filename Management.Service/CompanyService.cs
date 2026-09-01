using AutoMapper;
using Management.Contracts;
using Management.Entities.Exceptions;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;

namespace Management.Service;

internal sealed class CompanyService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper)
    : ICompanyService
{
    public CompanyDto CreateCompany(CompanyForCreationDto companyForCreation)
    {
        var company = mapper.Map<Company>(companyForCreation);

        repositoryManager.Company.CreateCompany(company);
        repositoryManager.Save();

        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = repositoryManager.Company.GetAllCompanies(trackChanges);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid id, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(id, trackChanges);
        if (company == null) throw new CompanyNotFoundException(id);
        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }
}
