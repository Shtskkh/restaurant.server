using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStatusesRepository
{
    Task<List<Status>> GetAll();
    Task<Status?> GetById(int id);
    Task Add(Status status);
    Task Update(Status status);
    Task Delete(int id);
}
public class StatusesRepository(RestaurantContext context) : IStatusesRepository
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
        try
        {
            context.Statuses.Add(status);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить статус.");
        }
    }

    public async Task Update(Status status)
    {
        var existingStatus = await GetById(status.IdStatus);
        if (existingStatus == null)
            throw new Exception("Статус не найден");
        
        context.Statuses.Entry(status).CurrentValues.SetValues(status);
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