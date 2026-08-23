using Management.Service.Contracts;
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

}
