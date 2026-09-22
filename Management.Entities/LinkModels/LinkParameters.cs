using Management.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Management.Entities.LinkModels;

public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);


