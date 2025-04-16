using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<IEnumerable<Order>> GetAll();
}

public class OrdersRepository(RestaurantContext context) : IOrdersRepository
{
    public async Task<IEnumerable<Order>> GetAll()
    {
        return await context.Orders.AsNoTracking()
            .Include(o => o.IdEmployeeNavigation)
            .Include(o => o.IdStatusNavigation)
            .Include(o => o.IdTableNavigation)
            .ToListAsync();
    }
}