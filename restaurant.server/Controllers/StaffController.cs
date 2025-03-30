using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Админ")]
public class StaffController(IStaffService staffService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await staffService.GetAll());
    }
}