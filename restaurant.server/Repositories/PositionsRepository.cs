using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IPositionsRepository
{
    Task<List<Position>> GetAll(); 
    Task<Position?> GetById(int id);
    Task<Position?> GetByTitle(string title);
    Task Add(Position position);
    Task Update(Position position);
    Task Delete(int id);
}

public class PositionsRepository(RestaurantContext context) : IPositionsRepository
{
    public async Task<List<Position>> GetAll()
    {
        return await context.Positions.AsNoTracking().ToListAsync();
    }

    public async Task<Position?> GetById(int id)
    {
        return await context.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.IdPosition == id);
    }

    public async Task<Position?> GetByTitle(string title)
    {
        return await context.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Title == title);
    }

    public async Task Add(Position position)
    {
        context.Positions.Add(position);
        await context.SaveChangesAsync();
    }

    public async Task Update(Position position)
    {
        context.Entry(position).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var position = await context.Positions.FirstOrDefaultAsync(p => p.IdPosition == id);
        if (position != null)
        {
            context.Positions.Remove(position);
            await context.SaveChangesAsync();
        }
    }
}