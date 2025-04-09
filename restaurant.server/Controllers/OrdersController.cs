using Microsoft.AspNetCore.Mvc;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrdersService ordersService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await ordersService.GetAll());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await ordersService.GetById(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }
}