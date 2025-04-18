using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : Controller
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<OrderModel>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await orderService.GetAll();
        
        if (orders.Count == 0)
            return NoContent();
        
        return Ok(orders);
    }

    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(typeof(OrderModel), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest();
        
        var order = await orderService.GetById(id);
        if (order == null)
            return NotFound();
        
        return Ok(order);
    }
}