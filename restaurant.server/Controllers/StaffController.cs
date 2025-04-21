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
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetAll()
    {
        var staff = await staffService.GetAll();
        if (staff.Count == 0)
            return NoContent();
        
        return Ok(await staffService.GetAll());
    }
    
    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(typeof(StaffModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest();
        
        var staff = await staffService.GetById(id);
        if (staff == null)
            return NotFound();
        
        return Ok(staff);
    }
}