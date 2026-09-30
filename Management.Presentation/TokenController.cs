using Management.Presentation.ActionFilters;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Management.Presentation;

[ApiController]
[Route("api/token")]
public class TokenController(IServiceManager service) : ControllerBase
{

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var token = await service.Authentication.CreateRefreshToken(tokenDto);
        return Ok(token);
    }
}

