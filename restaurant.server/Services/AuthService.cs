using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IAuthService
{
    Task<string?> Authenticate(string login, string password);
}

public class AuthService(IStaffRepository staffRepository, IJwtService jwtService) : IAuthService
{
    public async Task<string?> Authenticate(string login, string password)
    {
        var staff = await staffRepository.GetLoginInfo(login);
        if (staff == null || staff.Password != password)
            return null;

        return jwtService.GenerateJwtToken();
    }
}