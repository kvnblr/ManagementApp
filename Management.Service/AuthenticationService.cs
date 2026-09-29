using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Management.Contracts;
using Management.Entities.Models;
using Management.Service.Contracts;
using Management.Shared;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Management.Service;

public class AuthenticationService(
        ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        IConfiguration configuration
        ) : IAuthenticationService
{
    private User? _user;

    public async Task<string> CreateToken()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> { new(ClaimTypes.Name, _user.UserName) };
        var roles = await userManager.GetRolesAsync(_user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var jwtSettings = configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
                );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var user = mapper.Map<User>(userForRegistration);
        var result = await userManager.CreateAsync(user, userForRegistration.Password);
        if (result.Succeeded)
            await userManager.AddToRolesAsync(user, userForRegistration.Roles);
        return result;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication)
    {
        _user = await userManager.FindByNameAsync(userForAuthentication.UserName);
        var result = (_user != null && await userManager.CheckPasswordAsync(_user, userForAuthentication.Password));
        if (!result)
            logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password");
        return result;
    }
}
