using AutoMapper;
using Management.Contracts;
using Management.Entities.Models;
using Management.Service.Contracts;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Management.Service;

public class AuthenticationService(
        ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        IConfiguration configuration
        ) : IAuthenticationService
{
    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = mapper.Map<User>(userForRegistration);
        var result = await userManager.CreateAsync(user, userForRegistration.Password);
        if (result.Succeeded)
            await userManager.AddToRolesAsync(user, userForRegistration.Roles);
        return result;
    }
}
