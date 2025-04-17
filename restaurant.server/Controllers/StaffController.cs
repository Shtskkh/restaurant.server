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

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Неверный id");
        
        var staff = await staffService.GetById(id);
        if (staff == null)
            return NotFound();
        
        return Ok(staff);
    }

    [HttpGet("GetByLogin/{login}")]
    public async Task<IActionResult> GetByLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return BadRequest();
        
        var staff = await staffService.GetByLogin(login);
        if (staff == null)
            return NotFound();
        
        return Ok(staff);
    }
    
    [HttpGet("Position/{position}")]
    public async Task<IActionResult> GetByPosition(string position)
    {
        if (string.IsNullOrWhiteSpace(position))
            return BadRequest();
        
        var staff = await staffService.GetByPosition(position);
        if (staff.Count == 0)
            return NotFound();
        
        return Ok(staff);
    }
}