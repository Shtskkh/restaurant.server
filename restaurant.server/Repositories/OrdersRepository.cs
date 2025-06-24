using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Utils;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<List<OrderModel>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(int idOrder);
    Task<List<DishInOrderModel>> GetDishesInOrderAsync(int idOrder);
    Task<RepositoryResult<Order>> AddOrderAsync(Order order);
    Task<RepositoryResult<DishesInOrder>> AddDishesInOrderAsync(DishesInOrder dishes);
    Task<RepositoryResult<List<OrderModel>>> GetCurrentOrdersAsync();
    Task<RepositoryResult<List<OrderModel>>> GetCurrentOrdersAsync(string employeeLogin);
}

public class OrdersRepository(RestaurantContext context, ILogger<OrdersRepository> logger) : IOrdersRepository
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

    public async Task<RepositoryResult<Order>> AddOrderAsync(Order order)
    {
        logger.LogInformation("Adding a order with ID: {orderId}", order.IdOrder);
        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            return RepositoryResult<Order>.Success(order);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error when adding order: {OrderId}", order.IdOrder);
            return RepositoryResult<Order>.Fail("Database error: " + ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while adding order: {OrderId}", order.IdOrder);
            return RepositoryResult<Order>.Fail("Error: " + ex.Message);
        }
    }

    public async Task<RepositoryResult<DishesInOrder>> AddDishesInOrderAsync(DishesInOrder dishInOrder)
    {
        logger.LogInformation("Adding a dish with ID: {dishId} to the order with ID: {dishInOrderId}...",
            dishInOrder.IdDish, dishInOrder.IdOrder);
        try
        {
            await context.DishesInOrders.AddAsync(dishInOrder);
            await context.SaveChangesAsync();
            return RepositoryResult<DishesInOrder>.Success(dishInOrder);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Database error when adding dish: {DishId} in order: {OrderID}", dishInOrder.IdDish,
                dishInOrder.IdOrder);
            return RepositoryResult<DishesInOrder>.Fail("Database error: " + e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error while adding dish: {DishId} in order: {OrderID}", dishInOrder.IdDish,
                dishInOrder.IdOrder);
            return RepositoryResult<DishesInOrder>.Fail("Error: " + e.Message);
        }
    }

    public async Task<RepositoryResult<List<OrderModel>>> GetCurrentOrdersAsync()
    {
        logger.LogInformation("Getting all current orders...");
        try
        {
            var ordersModels = await (
                from order in context.Orders.AsNoTracking()
                join table in context.Tables.AsNoTracking()
                    on order.IdTable equals table.IdTable
                join status in context.Statuses.AsNoTracking()
                    on order.IdStatus equals status.IdStatus
                join employee in context.Staff.AsNoTracking()
                    on order.IdEmployee equals employee.IdEmployee
                where status.Title == "Принят" || status.Title == "Готовится"
                select new OrderModel
                {
                    IdOrder = order.IdOrder,
                    Date = order.Date,
                    TableNumber = table.Number,
                    Status = status.Title,
                    Employee = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}"
                }).ToListAsync();

            return RepositoryResult<List<OrderModel>>.Success(ordersModels);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error while getting an current orders.");
            return RepositoryResult<List<OrderModel>>.Fail("Error: " + e.Message);
        }
    }

    public async Task<RepositoryResult<List<OrderModel>>> GetCurrentOrdersAsync(string employeeLogin)
    {
        logger.LogInformation("Getting all current orders for employee with login: {employeeLogin}...", employeeLogin);
        try
        {
            var ordersModels = await (
                from order in context.Orders.AsNoTracking()
                join table in context.Tables.AsNoTracking()
                    on order.IdTable equals table.IdTable
                join status in context.Statuses.AsNoTracking()
                    on order.IdStatus equals status.IdStatus
                join employee in context.Staff.AsNoTracking()
                    on order.IdEmployee equals employee.IdEmployee
                where (status.Title == "Принят" || status.Title == "Готовится") && employee.Login == employeeLogin
                select new OrderModel
                {
                    IdOrder = order.IdOrder,
                    Date = order.Date,
                    TableNumber = table.Number,
                    Status = status.Title,
                    Employee = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}"
                }).ToListAsync();

            return RepositoryResult<List<OrderModel>>.Success(ordersModels);
        }
        catch (Exception e)
        {
            logger.LogError(e,
                "Unexpected error while getting an current orders for employee with login: {employeeLogin}.",
                employeeLogin);
            return RepositoryResult<List<OrderModel>>.Fail("Error: " + e.Message);
        }
    }
}