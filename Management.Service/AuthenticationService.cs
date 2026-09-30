using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Management.Contracts;
using Management.Entities.ConfigurationModels;
using Management.Entities.Exceptions;
using Management.Entities.Models;
using Management.Service.Contracts;
using Management.Shared;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Management.Service;

public class AuthenticationService(
        ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        IOptions<JwtConfiguration> configuration
        ) : IAuthenticationService
{
    private readonly JwtConfiguration _jwtConfiguration = configuration.Value;
    private User? _user;

    public async Task<TokenDto> CreateRefreshToken(TokenDto tokenDto)
    {
        // var jwtSettings = configuration.GetSection("JwtSettings");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
            ValidateLifetime = true,
            // ValidIssuer = jwtSettings["validIssuer"],
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            // ValidAudience = jwtSettings["validAudience"]
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(tokenDto.AccessToken, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token");

        var user = await userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            throw new RefereshTokenBadRequestException();

        _user = user;

        return await CreateToken(populateExp: false);
    }

    public async Task<string> CreateToken()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> { new(ClaimTypes.Name, _user.UserName) };
        var roles = await userManager.GetRolesAsync(_user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        // var jwtSettings = configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
                // issuer: jwtSettings["validIssuer"],
                issuer: _jwtConfiguration.ValidIssuer,
                // audience: jwtSettings["validAudience"],
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                 // expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                 expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials
                );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> { new(ClaimTypes.Name, _user.UserName) };
        var roles = await userManager.GetRolesAsync(_user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        // var jwtSettings = configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
                // issuer: jwtSettings["validIssuer"],
                issuer: _jwtConfiguration.ValidIssuer,
               // audience: jwtSettings["validAudience"],
               audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                 // expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                 expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials
                );

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

        await userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
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
