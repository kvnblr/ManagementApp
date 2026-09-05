using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController(IServiceManager service) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = service.Company.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = service.Company.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(IEnumerable<Guid> ids)
    {
        var companies = service.Company.GetByIds(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreation)
    {
        if (companyForCreation is null) return BadRequest("CompanyForCreationDto object is null");

        var company = service.Company.CreateCompany(companyForCreation);
        return CreatedAtRoute("CompanyById", new { id = company.Id }, company);
    }
}
