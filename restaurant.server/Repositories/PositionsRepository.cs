using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IPositionsRepository
{
    Task<List<Position>> GetAll();
}

public class PositionsRepository(RestaurantContext context) : IPositionsRepository
{
    public async Task<List<Position>> GetAll()
    {
        return await context.Positions.AsNoTracking().ToListAsync();
    }
}