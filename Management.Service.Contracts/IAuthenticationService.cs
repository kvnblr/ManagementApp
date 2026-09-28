using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Management.Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
}
