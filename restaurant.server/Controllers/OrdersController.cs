using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController(IOrdersService ordersService) : Controller
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<OrderModel>), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await ordersService.GetAllAsync();
        if (orders.Count == 0)
            return NoContent();

        return Ok(orders);
    }

    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(typeof(OrderModel), 200)]
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

        var order = await ordersService.GetByIdAsync(id);
        if (order == null)
            return NotFound(new ProblemDetails
            {
                Title = "Заказ с таким ID не найден.",
                Status = 404
            });

        return Ok(order);
    }
}