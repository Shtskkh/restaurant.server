using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using restaurant.server.DTOs;

namespace restaurant.server.Services;

public interface IJwtTokenService
{
    public string GenerateAccessToken(List<Claim> claims);
}

public class JwtTokenService(IOptions<JwtSettingsModel> jwtSettings) : IJwtTokenService
{
    private readonly JwtSettingsModel _jwtSettings = jwtSettings.Value;
    
    public string GenerateAccessToken(List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}