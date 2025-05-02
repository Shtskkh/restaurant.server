using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(IAuthService authService) : Controller
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthSuccessResponseModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequestModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await authService.Authenticate(request.Login, request.Password);
        if (string.IsNullOrEmpty(token))
            return NotFound(new ProblemDetails
            {
                Title = "Неверный логин или пароль.",
                Status = 404
            });

        return Ok(new AuthSuccessResponseModel { Token = token });
    }
}