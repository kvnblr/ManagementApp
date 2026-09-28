using Management.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IServiceManager service) : ControllerBase
{

}
