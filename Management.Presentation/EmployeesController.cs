using Management.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[ApiController]
[Route("/api/companies/{companyId}/[controller]")]
public class EmployeesController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = serviceManager.Employee.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = serviceManager.Employee.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }

}
