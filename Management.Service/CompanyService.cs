using AutoMapper;
using Management.Contracts;
using Management.Entities.ErrorModels.Exceptions;
using Management.Entities.Exceptions;
using Management.Entities.Models;
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

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();

        var companies = mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companies)
            repositoryManager.Company.CreateCompany(company);

        repositoryManager.Save();

        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        var ids = string.Join(",", companiesDto.Select(c => c.Id));

        return (companiesDto, ids);
    }

    public void DeleteCompany(Guid id, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(id, trackChanges) ?? throw new CompanyNotFoundException(id);
        repositoryManager.Company.DeleteCompany(company);
        repositoryManager.Save();
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var companies = repositoryManager.Company.GetAllCompanies(trackChanges);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();
        var companies = repositoryManager.Company.GetByIds(ids, trackChanges);
        if (ids.Count() != companies.Count())
            throw new CollectionByIdsBadRequestException();

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

    public void UpdateCompany(Guid id, CompanyForUpdateDto companyForUpdate, bool trackChanges)
    {
        var company = repositoryManager.Company.GetCompany(id, trackChanges) ?? throw new CompanyNotFoundException(id);
        mapper.Map(companyForUpdate, company);
        repositoryManager.Save();
    }
}
