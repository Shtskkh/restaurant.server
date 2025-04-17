using Microsoft.AspNetCore.Mvc;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController(IDishesService dishesService) : Controller
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var dishes = await dishesService.GetAll();
        
        if (dishes.Count == 0)
            return NoContent();
        
        return Ok(dishes);
    }
}