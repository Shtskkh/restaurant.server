using Microsoft.AspNetCore.Mvc;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : Controller
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await orderService.GetAll();
        
        if (orders.Count == 0)
            return NoContent();
        
        return Ok(orders);
    }
}