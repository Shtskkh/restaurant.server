using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(RestaurantContext context) : Controller
{
    [HttpPost("{login}")]
    public async Task<ActionResult> Login(string login)
    {
        var employee = await context.Staff
            .Select(e => new {e.Login, e.Password})
            .FirstOrDefaultAsync(x => x.Login == login);
        var password = HttpContext.Request.Form["password"];
        
        if (employee == null)
            return NotFound();
        
        if (employee.Password != password)
            return BadRequest();
        
        return Ok();
    }
}