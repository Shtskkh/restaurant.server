using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginRequestModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var accessToken = await authService.Authenticate(request.Login, request.Password);
        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized();
        
        return Ok(new { access_token = accessToken });
    }
}