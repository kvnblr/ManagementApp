using Management.Contracts;
using Management.Entities.LinkModels;
using Management.Entities.Models;
using Management.Shared.DataTransferObjects;
using Microsoft.Net.Http.Headers;

namespace Management.Api.Utility;

public class EmployeeLinks(
        LinkGenerator linkGenerator,
        IDataShaper<EmployeeDto> dataShaper)
    : IEmployeeLinks
{
    public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
    {
        var shapedEmployees = dataShaper.ShapedData(employeesDto, fields)
            .Select(e => e.Entity)
            .ToList();

        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
        if (mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
        {
            var employeeDtoList = employeesDto.ToList();
            for (var index = 0; index < employeeDtoList.Count(); index++)
            {
                var employeeLinks = new List<Link> {
                    new Link(linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", values: new { companyId, employeeDtoList[index].Id, fields }),"self","GET"),
                    new Link(linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForCompany", values: new { companyId, employeeDtoList[index].Id }),"delete_employee","DELETE"),
                    new Link(linkGenerator.GetUriByAction(httpContext, "UpdateEmployeeforCompany", values: new { companyId, employeeDtoList[index].Id }),"update_employee","UPDATE"),
                    new Link(linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployeeForCompany", values: new { companyId, employeeDtoList[index].Id }),"partially_update_employee","PATCH")
               };

                shapedEmployees[index].Add("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
            employeeCollection.Links.Add(new Link(linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", values: new { }), "self", "GET"));

            return new LinkResponse { HasLinks = true, LinkedEntities = employeeCollection };
        }

        return new LinkResponse { ShapedEntities = shapedEmployees };

    }
}
