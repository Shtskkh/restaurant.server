using Microsoft.AspNetCore.Mvc;
using restaurant.server.DTOs;
using restaurant.server.Services;

namespace restaurant.server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SuppliesController(ISuppliesService suppliesService) : Controller
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<SupplyModel>), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetAll()
    {
        var supplies = await suppliesService.GetAllAsync();
        if (supplies.Count == 0)
            return NoContent();

        return Ok(supplies);
    }
}