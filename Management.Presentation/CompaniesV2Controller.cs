using Asp.Versioning;
using Management.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[ApiVersion("2.0")]
[Route("api/{v:apiversion}/companies")]
[ApiController]
public class CompaniesV2Controller(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await serviceManager.Company.GetAllCompaniesAsync(trackChanges: false);
        var companiesV2 = companies.Select(c => $"{c.Name} V2");
        return Ok(companiesV2);
    }
}
