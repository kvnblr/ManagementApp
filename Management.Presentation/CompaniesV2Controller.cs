using Asp.Versioning;
using Management.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[Route("api/{v:apiversion}/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
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
