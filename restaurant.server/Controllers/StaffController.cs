using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StaffController(IStaffService staffService) : Controller
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<StaffModel>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await staffService.GetAll());
    }
    
    [HttpGet("GetById")]
    [ProducesResponseType(typeof(StaffModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 1)
            return BadRequest();
        
        var staff = await staffService.GetById(id);
        if (staff == null)
            return NotFound();
        
        return Ok(staff);
    }
}