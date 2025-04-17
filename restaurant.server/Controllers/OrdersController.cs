﻿using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Неверный id");
        
        var order = await orderService.GetById(id);
        if (order == null)
            return NotFound();
        
        return Ok(order);
    }
}