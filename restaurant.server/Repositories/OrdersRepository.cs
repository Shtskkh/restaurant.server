using System.Collections;
using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<IEnumerable<OrderModel>> GetAll();
    Task<OrderModel?> GetById(int id);
    Task<IEnumerable<DishInOrderModel>> GetDishesInOrder(int id);
}

public class OrdersRepository(RestaurantContext context) : IOrdersRepository
{
    public async Task<IEnumerable<OrderModel>> GetAll()
    {
        var orders =
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
        
        return await orders.ToListAsync();
    }

    public async Task<OrderModel?> GetById(int id)
    {
        var orders =
            from order in context.Orders.AsNoTracking()
            where order.IdOrder == id
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
        
        return await orders.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<DishInOrderModel>> GetDishesInOrder(int id)
    {
        var dishes =
            from orderDish in context.DishesInOrders.AsNoTracking()
            where orderDish.IdOrder == id
            join dish in context.Dishes.AsNoTracking()
                on orderDish.IdDish equals dish.IdDish
            join status in context.Statuses.AsNoTracking()
                on orderDish.IdStatus equals status.IdStatus
            select new DishInOrderModel
            {
                IdDish = dish.IdDish,
                Title = dish.Title,
                Count = orderDish.Count,
                Comment = orderDish.Comment,
                Status = status.Title,
                TotalCost = orderDish.Count * dish.Cost
            };
        
        return await dishes.ToListAsync();
    }
}