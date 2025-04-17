using restaurant.server.Controllers;
using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IOrderService
{
    Task<List<OrderModel>> GetAll();
    Task<OrderModel?> GetById(int id);
}

public class OrdersService(IOrdersRepository ordersRepository) : IOrderService
{
    public async Task<List<OrderModel>> GetAll()
    {
        var orders = await ordersRepository.GetAll();
        return orders.ToList();
    }

    public async Task<OrderModel?> GetById(int id)
    {
        var order = await ordersRepository.GetById(id);
        if (order == null)
            return null;
        
        var dishes = await ordersRepository.GetDishesInOrder(id);
        order.Dishes = dishes.ToList();
        
        return order;
    }
}