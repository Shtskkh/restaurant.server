using System.Security.Claims;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IAuthService
{
    Task<string?> Authenticate(string login, string password);
    Task<string?> Refresh(string login, string refreshToken);
}

public class AuthService(
    IStaffRepository staffRepository,
    IPositionsRepository positionsRepository,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<string?> Authenticate(string login, string password)
    {
        var s = await staffRepository.GetByLogin(login);
        if (s == null || s.Password != password)
            return null;
        
        var p = await positionsRepository.GetById(s.IdPosition);
        var claims = new List<Claim>
        {
            new (ClaimTypes.Role, p!.Title)
        };

        return jwtTokenService.GenerateAccessToken(claims);
    }

    public async Task<string?> Refresh(string login, string accessToken)
    {
        var tokenValidationResult = await jwtTokenService.ValidateAccessToken(accessToken);
        
        if (!tokenValidationResult.IsValid) return null;
        
        var s = await staffRepository.GetByLogin(login);
        if (s == null)
            return null;
        
        var p = await positionsRepository.GetById(s.IdPosition);
        var claims = new List<Claim>
        {
            new (ClaimTypes.Role, p!.Title)
        };
        
        return jwtTokenService.GenerateAccessToken(claims);
    }
    
}