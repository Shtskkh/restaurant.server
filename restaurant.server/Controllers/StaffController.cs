using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class StaffController(RestaurantContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var employees = await context.Staff
            .Select(e => new
            {
                e.IdEmployee,
                e.IdPosition,
                e.LastName,
                e.FirstName,
                e.MiddleName,
                e.PhoneNumber
            })
            .ToListAsync();
        
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var employee = await context.Staff
            .Select(e => new
            {
                e.IdEmployee,
                e.IdPosition,
                e.LastName,
                e.FirstName,
                e.MiddleName,
                e.PhoneNumber
            })
            .FirstOrDefaultAsync(e => e.IdEmployee == id);
        
        if (employee is null)
            return NotFound();
        
        return Ok(employee);
    }

    [HttpGet("{login}")]
    public async Task<ActionResult<Staff>> GetByLogin(string login)
    {
        var employee = await context.Staff.FirstOrDefaultAsync(x => x.Login == login);
        if (employee == null)
            return NotFound();
        return employee;
    }
}