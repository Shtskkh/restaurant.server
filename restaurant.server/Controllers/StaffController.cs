using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController(IStaffService staffService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await staffService.GetAll());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var staff = await staffService.GetById(id);
        if (staff == null)
            return NotFound();
        return Ok(staff);
    }

    [HttpGet("{login}")]
    public async Task<IActionResult> GetByLogin(string login)
    {
        var staff = await staffService.GetByLogin(login);
        if (staff == null)
            return NotFound();
        return Ok(staff);
    }

    [HttpGet("position/{position}")]
    public async Task<IActionResult> GetByPosition(string position)
    {
        var staff = await staffService.GetByPosition(position);
        if (staff.Count == 0)
            return NotFound();
        return Ok(staff);
    }
}