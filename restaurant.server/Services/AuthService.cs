using System.Security.Claims;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IAuthService
{
    Task<string?> Authenticate(string login, string password);
}

public class AuthService(
    IStaffRepository staffRepository,
    IPositionsRepository positionsRepository,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<string?> Authenticate(string login, string password)
    {
        var s = await staffRepository.GetByLogin(login);
        if (s == null || s.Login != login || s.Password != password)
            return null;
        
        var p = await positionsRepository.GetById(s.IdPosition);
        var claims = new List<Claim>
        {
            new (ClaimTypes.Role, p!.Title)
        };

        return jwtTokenService.GenerateAccessToken(claims);
    }
}