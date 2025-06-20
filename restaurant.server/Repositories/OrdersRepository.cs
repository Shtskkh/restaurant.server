using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<List<OrderModel>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(int idOrder);
    Task<List<DishInOrderModel>> GetDishesInOrderAsync(int idOrder);
}

public class OrdersRepository(RestaurantContext context) : IOrdersRepository
{
    public async Task<List<OrderModel>> GetAllAsync()
    {
        var ordersModels =
            from order in context.Orders.AsNoTracking()
            join table in context.Tables.AsNoTracking()
                on order.IdTable equals table.IdTable
            join status in context.Statuses.AsNoTracking()
                on order.IdStatus equals status.IdStatus
            join employee in context.Staff.AsNoTracking()
                on order.IdEmployee equals employee.IdEmployee
            select new OrderModel
            {
                IdOrder = order.IdOrder,
                Date = order.Date,
                TableNumber = table.Number,
                Status = status.Title,
                Employee = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}"
            };

        return await ordersModels.ToListAsync();
    }

    public async Task<OrderModel?> GetByIdAsync(int idOrder)
    {
        var orderModel =
            from order in context.Orders.AsNoTracking()
            where order.IdOrder == idOrder
            join table in context.Tables.AsNoTracking()
                on order.IdTable equals table.IdTable
            join status in context.Statuses.AsNoTracking()
                on order.IdStatus equals status.IdStatus
            join employee in context.Staff.AsNoTracking()
                on order.IdEmployee equals employee.IdEmployee
            select new OrderModel
            {
                IdOrder = order.IdOrder,
                Date = order.Date,
                TableNumber = table.Number,
                Status = status.Title,
                Employee = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}"
            };

        return await orderModel.FirstOrDefaultAsync();
    }

    public async Task<List<DishInOrderModel>> GetDishesInOrderAsync(int idOrder)
    {
        var dishesInOrderModels =
            from dishInOrder in context.DishesInOrders.AsNoTracking()
            where dishInOrder.IdOrder == idOrder
            join dish in context.Dishes.AsNoTracking()
                on dishInOrder.IdDish equals dish.IdDish
            join status in context.Statuses.AsNoTracking()
                on dishInOrder.IdStatus equals status.IdStatus
            select new DishInOrderModel
            {
                IdDish = dish.IdDish,
                Title = dish.Title,
                Count = dishInOrder.Count,
                Comment = dishInOrder.Comment,
                Status = status.Title,
                TotalCost = dishInOrder.Count * dish.Cost
            };

        return await dishesInOrderModels.ToListAsync();
    }
}