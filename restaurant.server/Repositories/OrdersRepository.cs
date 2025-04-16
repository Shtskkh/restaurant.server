using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<IEnumerable<Order>> GetAll();
    Task<Order?> GetById(int id);
    Task<IEnumerable<DishesInOrder>> GetDishesInOrder(int id);
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

    public async Task<Order?> GetById(int id)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(o => o.IdEmployeeNavigation)
            .Include(o => o.IdStatusNavigation)
            .Include(o => o.IdTableNavigation)
            .FirstOrDefaultAsync(o => o.IdOrder == id);
    }
    public async Task<IEnumerable<DishesInOrder>> GetDishesInOrder(int id)
    {
        return await context.DishesInOrders
            .AsNoTracking()
            .Where(o => o.IdOrder == id)
            .Include(d => d.IdStatusNavigation)
            .Include(d => d.IdDishNavigation)
            .ToListAsync();
    }
}