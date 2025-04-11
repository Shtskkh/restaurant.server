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
        try
        {
            context.Positions.Add(position);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить должность");
        }
    }

    public async Task Update(Position position)
    {
        var existingPosition = await GetById(position.IdPosition);
        if (existingPosition == null)
            throw new Exception("Должность не найдена.");
        
        context.Positions.Entry(existingPosition).CurrentValues.SetValues(position);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var position = await GetById(id);
        if (position != null)
        {
            context.Positions.Remove(position);
            await context.SaveChangesAsync();
        }
    }
}