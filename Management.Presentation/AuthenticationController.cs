using Management.Presentation.ActionFilters;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IServiceManager service) : ControllerBase
{

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var result = await service.Authentication.RegisterUser(userForRegistration);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.TryAddModelError(error.Code, error.Description);
            return BadRequest(ModelState);
        }
        return StatusCode(201);
    }
}
