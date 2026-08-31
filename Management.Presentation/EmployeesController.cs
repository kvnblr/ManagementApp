using Management.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[ApiController]
[Route("/api/companies/{companyId}/[controller]")]
public class EmployeesController(IServiceManager serviceManager) : ControllerBase
{

}
