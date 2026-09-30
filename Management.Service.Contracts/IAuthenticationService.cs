using Management.Shared;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Management.Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication);
    Task<string> CreateToken();
    Task<TokenDto> CreateToken(bool populateExp);
}
