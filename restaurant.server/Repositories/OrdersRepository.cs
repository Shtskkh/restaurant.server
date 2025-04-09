using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<List<Order>> GetAll();
    Task<Order?> GetById(int id);
    Task Add(Order order);
    Task Update(Order order);
    Task Delete(int id);
}

public class OrdersRepository(RestaurantContext context) : IOrdersRepository
{
    public async Task<List<Order>> GetAll()
    {
        return await context.Orders.AsNoTracking().ToListAsync();
    }

    public async Task<Order?> GetById(int id)
    {
        return await context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.IdOrder == id);
    }

    public async Task Add(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();
    }

    public async Task Update(Order order)
    {
        context.Entry(order).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var order = await GetById(id);
        if (order != null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }
}