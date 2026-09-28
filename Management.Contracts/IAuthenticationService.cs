using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Management.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
}
