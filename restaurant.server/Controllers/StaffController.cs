using Microsoft.AspNetCore.Mvc;
using restaurant.server.Repositories;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController(IStaffService staffService) : Controller
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var staff = await staffService.GetAll();
        
        if (staff.Count == 0)
            return NoContent();
        
        return Ok(staff);
    }
}