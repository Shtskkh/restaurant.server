using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DishesController(IDishesService dishesService) : Controller
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<DishModel>), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetAll()
    {
        var dishes = await dishesService.GetAll();
        if (dishes.Count == 0)
            return NoContent();

        return Ok(dishes);
    }

    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(typeof(DishModel), 200)]
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

        var dish = await dishesService.GetById(id);
        if (dish == null)
            return NotFound(new ProblemDetails
            {
                Title = "Блюдо с таким ID не найдено.",
                Status = 404
            });

        return Ok(dish);
    }
}