using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController(IDishesService dishesService) : Controller
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<DishModel>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var dishes = await dishesService.GetAll();
        
        if (dishes.Count == 0)
            return NoContent();
        
        return Ok(dishes);
    }
    
    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(typeof(DishModel), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest();
        
        var dish = await dishesService.GetById(id);
        if (dish == null)
            return NotFound();
        
        return Ok(dish);
    }
}