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
        var staff = await staffService.GetAllAsync();

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

        var staff = await staffService.GetByIdAsync(id);
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
        var positions = await staffService.GetAllPositionsAsync();
        return Ok(positions);
    }

    [HttpPost("Add")]
    [ProducesResponseType(typeof(int), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Add([FromForm] CreateStaffModel newEmployee)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffResult = await staffService.AddAsync(newEmployee);

        if (!staffResult.IsSuccess)
            return BadRequest(staffResult.ErrorMessage);

        return Ok(staffResult.Data);
    }
}