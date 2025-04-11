using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface ITablesRepository
{
    Task<List<Table>> GetAll();
    Task<Table?> GetById(int id);
    Task Add(Table table);
    Task Update(Table table);
    Task Delete(int id);
}

public class TablesRepository(RestaurantContext context) : ITablesRepository
{
    public async Task<List<Table>> GetAll()
    {
        return await context.Tables.AsNoTracking().ToListAsync();
    }

    public async Task<Table?> GetById(int id)
    {
        return await context.Tables.AsNoTracking().FirstOrDefaultAsync(t => t.IdTable == id);
    }

    public async Task Add(Table table)
    {
        try
        {
            context.Tables.Add(table);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить стол.");
        }
    }

    public async Task Update(Table table)
    {
        context.Tables.Entry(table).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var table = await GetById(id);
        if (table != null)
        {
            context.Tables.Remove(table);
            await context.SaveChangesAsync();
        }
    }
}