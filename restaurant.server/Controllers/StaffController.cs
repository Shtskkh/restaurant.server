using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Models;
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
        var staff = await staffService.GetAll();

        return Ok(staff);
    }

    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(typeof(StaffModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest(new ProblemDetails
            {
                Title = "ID должен быть больше 0.",
                Status = 400
            });

        var staff = await staffService.GetById(id);
        if (staff == null)
            return NotFound(new ProblemDetails
            {
                Title = "Сотрудник с таким ID не найден.",
                Status = 404
            });

        return Ok(staff);
    }

    [HttpGet("GetAllPositions")]
    [ProducesResponseType(typeof(List<Position>), 200)]
    public async Task<IActionResult> GetAllPositions()
    {
        var positions = await staffService.GetAllPositions();
        return Ok(positions);
    }
}