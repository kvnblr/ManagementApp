using Management.Entities.LinkModels;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Management.Contracts;

public interface IEmployeeLinks
{
    LinkResponse TryGenerateLinks(
            IEnumerable<EmployeeDto> employeesDto,
            string fields,
            Guid companyId,
            HttpContext httpContext);
}
