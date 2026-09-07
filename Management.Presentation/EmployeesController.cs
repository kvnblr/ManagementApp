using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
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

    [HttpGet("{id:guid}", Name = nameof(GetEmployeeForCompany))]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = serviceManager.Employee.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
    {
        if (employeeForCreation is null) return BadRequest("EmployeeForCreation object is null");

        var employee = serviceManager.Employee.CreateEmployeeForCompany(companyId, employeeForCreation, trackChanges: false);

        return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, id = employee.Id }, employee);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        serviceManager.Employee.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForUpdateDto object is null.");

        serviceManager.Employee.UpdateEmployeeForCompany(companyId, id, employee, employeeTrackChanges: false, companyTrackChanges: true);
        return NoContent();
    }
}
