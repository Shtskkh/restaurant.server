using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IOrdersService
{
    Task<List<OrderModel>> GetAll();
    Task<OrderModel?> GetById(int id);
}

public class OrdersService(IOrdersRepository ordersRepository) : IOrdersService
{
    public async Task<List<OrderModel>> GetAll()
    {
        return await ordersRepository.GetAll();
    }

    public async Task<OrderModel?> GetById(int id)
    {
        var order = await ordersRepository.GetById(id);

        if (order == null)
            return null;

        var dishes = await ordersRepository.GetDishesInOrder(order.IdOrder);

        order.DishesInOrder = dishes;
        return order;
    }
}