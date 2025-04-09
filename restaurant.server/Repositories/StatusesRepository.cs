using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public class StatusesRepository(RestaurantContext context)
{
    public async Task<List<Status>> GetAll()
    {
        return await context.Statuses.AsNoTracking().ToListAsync();
    }

    public async Task<Status?> GetById(int id)
    {
        return await context.Statuses.AsNoTracking().FirstOrDefaultAsync(s => s.IdStatus == id);
    }

    public async Task Add(Status status)
    {
        context.Statuses.Add(status);
        await context.SaveChangesAsync();
    }

    public async Task Update(Status status)
    {
        context.Entry(status).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var status = await GetById(id);
        if (status != null)
        {
            context.Statuses.Remove(status);
            await context.SaveChangesAsync();
        }
    }
}