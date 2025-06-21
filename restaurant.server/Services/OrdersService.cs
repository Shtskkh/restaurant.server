using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;
using restaurant.server.Utils;

namespace restaurant.server.Services;

public interface IOrdersService
{
    Task<List<OrderModel>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(int id);
    Task<ServiceResult<int>> AddAsync(AddOrderModel order);
}

public class OrdersService(
    IOrdersRepository ordersRepository,
    ITablesRepository tablesRepository,
    IStatusesRepository statusesRepository,
    IStaffRepository staffRepository,
    IDishesRepository dishesRepository,
    ILogger<OrdersService> logger) : IOrdersService
{
    public async Task<List<OrderModel>> GetAllAsync()
    {
        return await ordersRepository.GetAllAsync();
    }

    public async Task<OrderModel?> GetByIdAsync(int id)
    {
        var order = await ordersRepository.GetByIdAsync(id);

        if (order == null)
            return null;

        var dishes = await ordersRepository.GetDishesInOrderAsync(order.IdOrder);

        order.DishesInOrder = dishes;
        return order;
    }

    public async Task<ServiceResult<int>> AddAsync(AddOrderModel order)
    {
        if (order.Dishes.Count == 0) return ServiceResult<int>.Fail("There are no dishes in the order.");

        logger.LogInformation("Starting to add order...");

        var table = await tablesRepository.GetByNumberAsync(order.Table);
        if (!table.IsSuccess) return ServiceResult<int>.Fail(table.ErrorMessage);

        var status = await statusesRepository.GetByTitleAsync("Принят");
        if (!status.IsSuccess) return ServiceResult<int>.Fail(status.ErrorMessage);

        var employee = await staffRepository.GetByLoginAsync(order.EmployeeLogin);
        if (!employee.IsSuccess) return ServiceResult<int>.Fail(employee.ErrorMessage);

        var newOrder = new Order
        {
            Date = order.Date,
            IdTable = table.Data!.IdTable,
            IdStatus = status.Data!.IdStatus,
            IdEmployee = employee.Data!.IdEmployee
        };

        var orderResult = await ordersRepository.AddOrderAsync(newOrder);

        if (!orderResult.IsSuccess)
        {
            logger.LogError("Adding order failed, error: {Error}.", orderResult.ErrorMessage);
            return ServiceResult<int>.Fail(orderResult.ErrorMessage);
        }

        foreach (var dish in order.Dishes)
        {
            var dishResult = await AddDishesInOrderAsync(orderResult.Data.IdOrder, dish, status.Data);
            if (!dishResult.IsSuccess) return ServiceResult<int>.Fail(dishResult.ErrorMessage);
        }

        logger.LogInformation("Adding order finished successfully. ID: {OrderId}", orderResult.Data);
        return ServiceResult<int>.Success(orderResult.Data.IdOrder);
    }

    private async Task<ServiceResult<bool>> AddDishesInOrderAsync(int orderId, AddDishInOrderModel dish, Status status)
    {
        var dishResult = await dishesRepository.GetByTitleAsync(dish.Title);
        if (!dishResult.IsSuccess) return ServiceResult<bool>.Fail(dishResult.ErrorMessage);

        var dishInOrder = new DishesInOrder
        {
            IdOrder = orderId,
            IdDish = dishResult.Data!.IdDish,
            Comment = dish.Comment,
            Count = dish.Count,
            IdStatus = status.IdStatus
        };

        var result = await ordersRepository.AddDishesInOrderAsync(dishInOrder);
        return !result.IsSuccess ? ServiceResult<bool>.Fail(result.ErrorMessage) : ServiceResult<bool>.Success(true);
    }
}