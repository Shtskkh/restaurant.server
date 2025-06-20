using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IOrdersService
{
    Task<List<OrderModel>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(int id);
}

public class OrdersService(IOrdersRepository ordersRepository) : IOrdersService
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
}