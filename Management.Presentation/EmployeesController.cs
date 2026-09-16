using Management.Presentation.ActionFilters;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[ApiController]
[Route("/api/companies/{companyId}/[controller]")]
public class EmployeesController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
    {
        var employees = await serviceManager.Employee.GetEmployeesAsync(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = nameof(GetEmployeeForCompany))]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = await serviceManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employeeForCreation)
    {

        var employee = await serviceManager.Employee.CreateEmployeeForCompanyAsync(companyId, employeeForCreation, trackChanges: false);

        return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, id = employee.Id }, employee);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await serviceManager.Employee.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        await serviceManager.Employee.UpdateEmployeeForCompanyAsync(companyId, id, employee, employeeTrackChanges: false, companyTrackChanges: true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null) return BadRequest("patchDoc object sent from client is null.");
        var (employeeToPatch, employee) = await serviceManager.Employee.GetEmployeeForPatchAsync(companyId, id, companyTrackChanges: false, employeeTrackChanges: true);
        patchDoc.ApplyTo(employeeToPatch);

        TryValidateModel(employeeToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await serviceManager.Employee.SaveChangesForPatchAsync(employeeToPatch, employee);
        return NoContent();
    }
}
